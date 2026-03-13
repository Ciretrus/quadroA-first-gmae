using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class ItemsActivations : MonoBehaviour
{
    [Header("Raycast settings")]
    [SerializeField] private float m_rayDistance = 1f;
    [Header("Rune-drawing settings")]
    [SerializeField] private float m_runeOffset = 1.5f;

    private PlayerController m_playerController;
    private Camera m_camera;
    private CameraMovement m_cameraMovement;
    private UIController m_uiController;
    private DrawingController m_drawingRuneController;
    private PlateController m_plateController;
    private DrawCanvas m_runeDraw;
    private Interactable m_interactable;
    private Vector3 m_cameraPos = new Vector3(0f, 1.5f, 0f);
    private Vector3 m_screenCenter;
    private bool m_isUIBlocked;

    private void OnEnable()
    {
        m_drawingRuneController = ServiceLocator.Resolve<DrawingController>();

        foreach (DrawCanvas canvas in m_drawingRuneController.canvases)
        {
            canvas.DisableDrawing += ChangeDrawingMode;
        }
    }

    private void Start()
    {
        m_playerController = ServiceLocator.Resolve<PlayerController>();
        m_camera = ServiceLocator.Resolve<Camera>();
        m_uiController = ServiceLocator.Resolve<UIController>();
        m_plateController = ServiceLocator.Resolve<PlateController>();

        m_cameraMovement = m_camera.GetComponent<CameraMovement>();
    }

    private void OnDisable()
    {
        foreach (DrawCanvas canvas in m_drawingRuneController.canvases)
        {
            canvas.DisableDrawing -= ChangeDrawingMode;
        }
    }

    private void Update()
    {
        m_screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = m_camera.ScreenPointToRay(m_screenCenter);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, m_rayDistance);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out m_interactable) && m_interactable.enabled)
            {
                m_uiController.ShowObjectActivationCursor(true);

                // TODO Rework
                if (m_playerController.input.UI.Interact.WasPerformedThisFrame())
                {
                    switch (m_interactable.type)
                    {
                        case InteractableType.NonBlocking: break;
                        case InteractableType.Blocking:
                            {
                                if (m_interactable.TryGetComponent(out m_runeDraw))
                                {
                                    ChangeDrawingMode(m_interactable);
                                }
                                else if (m_interactable.GetComponent<Note>())
                                {
                                    ChangeUIMode();
                                }
                                break;
                            }
                    }
                    m_interactable.Use();

                    PlayInteractionSound(m_interactable);
                }
            }
            else if (hit.collider.TryGetComponent(out DiaryInteractable diaryInteractable))
            {
                DiaryNotif(diaryInteractable);
            }
            else
            {
                m_uiController.ShowObjectActivationCursor(false);
            }
        }
        else
        {
            m_uiController.ShowObjectActivationCursor(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DiaryInteractable diaryInteractable))
        {
            DiaryNotif(diaryInteractable);
        }

        if (other.TryGetComponent(out Pushable pushable) && !m_plateController.isSolved)
        {
            pushable.PlaySound();
            m_plateController.AddToSequence(pushable);
        }

        if (other.TryGetComponent(out PregameTutorialTrigger tutorial))
        {
            m_uiController.ShowTutorial(tutorial.hintText);
        }
    }

    public void ChangeUIMode(InputAction.CallbackContext context)
    {
        ChangeUIMode();
    }

    public void ChangeUIMode()
    {
        ChangeMovementState();
        ChangeCursorState();

        m_isUIBlocked = !m_isUIBlocked;
    }

    public void ChangeMovementState()
    {
        m_playerController.ChangeMovementState();
        m_cameraMovement.enabled = !m_cameraMovement.enabled;
    }

    private void DiaryNotif(DiaryInteractable m_interactable)
    {
        ThiefEye thiefEye = ServiceLocator.Resolve<ThiefEye>();
        int layerMask = 1 << m_interactable.gameObject.layer;

        if ((layerMask & thiefEye.layerThiefEye) != 0 && !thiefEye.hasStarted)
        {
            return;
        }

        if (!m_interactable.wasTriggered)
        {
            m_uiController.ShowDiaryNotification();
            m_interactable.TriggerDiaryRecord();
        }
    }

    private void ChangeCursorState()
    {
        if (m_isUIBlocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ChangeDrawingMode(Interactable interactable)
    {
        ChangeMovementState();
        ChangeCursorState();

        if (m_isUIBlocked)
        {
            m_playerController.cameraPos.transform.localPosition = m_cameraPos; 

            m_drawingRuneController.currentRune = null;
        }
        else
        {
            Quaternion interactableRot = interactable.gameObject.transform.rotation;
            transform.rotation = Quaternion.Euler(0f, interactableRot.eulerAngles.y + 180f, 0f);
            m_camera.transform.rotation = transform.rotation;

            Vector3 usablePos = interactable.transform.position;
            Vector3 newPos = new Vector3(usablePos.x, transform.position.y, usablePos.z);
            transform.position = newPos - (transform.forward * m_runeOffset);
            m_playerController.cameraPos.transform.position = new Vector3(transform.position.x, usablePos.y, transform.position.z);
            // TODO Remove (CameraMovement being disabled in ChangeMovementState())
            m_camera.transform.position = m_playerController.cameraPos.transform.position;

            m_drawingRuneController.currentRune = m_runeDraw.drawableLine.data;
        }

        m_isUIBlocked = !m_isUIBlocked;
    }

    private void PlayInteractionSound(Interactable interactable)
    {
        Debug.LogWarning(interactable.interactableSound);

        if (interactable.interactableSound == null)
        {
            return;
        }
                
        interactable.interactableSound.PlaySound();
    }
}