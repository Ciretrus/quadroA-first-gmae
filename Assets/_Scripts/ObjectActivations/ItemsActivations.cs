using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class ItemsActivations : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Camera m_camera;
    [SerializeField] private UIController m_uiController;
    [SerializeField] private PlateController m_plateController;
    [SerializeField] private DrawingRuneController m_drawingRuneController;
    [Header("Raycast settings")]
    [SerializeField] private float m_rayDistance = 1f;
    [Header("Rune-drawing settings")]
    [SerializeField] private float m_runeOffset = 1.5f;
    
    private PlayerController m_playerController;
    private CameraMovement m_cameraMovement;
    private DiaryInteractable m_diaryInteractable;
    private RuneDraw m_runeDraw;
    private Usable m_usable;
    private Vector3 m_cameraPos = new Vector3(0f, 1.5f, 0f);
    private Vector3 m_screenCenter;
    private bool m_isUIBlocked;

    private void Awake()
    {
        m_cameraMovement = m_camera.GetComponent<CameraMovement>();

        ServiceLocator.Register(m_camera);
    }

    private void Start()
    {
        m_playerController = ServiceLocator.Resolve<PlayerController>();
    }

    private void OnEnable()
    {
        foreach (RuneDraw rune in m_drawingRuneController.runes)
        {
            rune.DisableDrawing += ChangeDrawingMode;
        }
    }

    private void OnDisable()
    {
        foreach (RuneDraw rune in m_drawingRuneController.runes)
        {
            rune.DisableDrawing -= ChangeDrawingMode;
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
            if (hit.collider.TryGetComponent(out m_usable) && m_usable.enabled)
            {
                m_uiController.ShowObjectActivationText(true);

                // TODO Rework
                if (m_playerController.input.UI.Interact.WasPerformedThisFrame())
                {
                    switch (m_usable.type)
                    {
                        case UsableType.NonBlocking: Debug.Log("Interacted with Non-Blocking UI thing"); break;
                        case UsableType.Blocking:
                            {
                                if (m_usable.TryGetComponent(out m_runeDraw))
                                {
                                    ChangeDrawingMode(m_usable);
                                }
                                break;
                            }
                    }
                    m_usable.Use();

                    PlayInteractionSound(m_usable);
                }
            }
            else if (hit.collider.TryGetComponent(out m_diaryInteractable))
            {
                DiaryNotif(m_diaryInteractable);
            }
            else
            {
                m_uiController.ShowObjectActivationText(false);
            }
        }
        else
        {
            m_uiController.ShowObjectActivationText(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out m_diaryInteractable))
        {
            DiaryNotif(m_diaryInteractable);
        }

        if (other.TryGetComponent(out Pushable pushable) && !m_plateController.isSolved)
        {
            m_plateController.AddToSequence(pushable);
        }
    }

    public void ChangeUIMode(InputAction.CallbackContext context)
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

    private void ChangeDrawingMode(Usable usable)
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
            Quaternion usableRot = usable.gameObject.transform.rotation;
            transform.rotation = Quaternion.Euler(0f, usableRot.eulerAngles.y + 180f, 0f);
            m_camera.transform.rotation = transform.rotation;

            Vector3 usablePos = usable.transform.position;
            Vector3 newPos = new Vector3(usablePos.x, transform.position.y, usablePos.z);
            transform.position = newPos - (transform.forward * m_runeOffset);
            m_playerController.cameraPos.transform.position = new Vector3(transform.position.x, usablePos.y, transform.position.z);
            // TODO Remove (CameraMovement being disabled in ChangeMovementState())
            m_camera.transform.position = m_playerController.cameraPos.transform.position;

            m_drawingRuneController.currentRune = m_runeDraw.drawableLine.rune;
        }

        m_isUIBlocked = !m_isUIBlocked;
    }

    private void PlayInteractionSound(Usable usable)
    {
        Debug.LogWarning(usable.interactableSound);

        if (usable.interactableSound == null)
        {
            return;
        }
                
        usable.interactableSound.PlayPitchedSound();
    }
}