using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class ItemsActivations : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private Camera m_camera;
    [SerializeField] private UIController m_uiController;
    [SerializeField] private ThiefEye m_thiefEye;
    [SerializeField] private PlateController m_plateController;
    [SerializeField] private DrawingRuneController m_drawingRuneController;
    [SerializeField] private RuneDraw[] m_runes;
    [SerializeField] private OutlineFader m_outlineFader;
    [SerializeField] private float m_rayDistance = 1f;
    [SerializeField] private float m_offset = 1.5f;

    private CameraMovement m_cameraMovement;
    private Vector3 m_screenCenter;
    private Usable m_usable, m_lastUsable;
    private Outline m_lastOutlineObject;
    private DiaryInteractable m_diaryInteractable;
    private RuneDraw m_runeDraw;
    private bool m_isUIBlocked;

    private void Awake()
    {
        m_cameraMovement = m_camera.GetComponent<CameraMovement>();

        foreach (RuneDraw rune in m_runes)
        {
            rune.OnDisableDrawing += ChangeDrawingMode;
        }
    }

    private void OnDisable()
    {
        foreach (RuneDraw rune in m_runes)
        {
            rune.OnDisableDrawing -= ChangeDrawingMode;
        }
    }

    private void Update()
    {
        m_screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = m_camera.ScreenPointToRay(m_screenCenter);
        RaycastHit hit;
        Outline outlineObject;

        Physics.Raycast(ray, out hit, m_rayDistance);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out m_usable) && m_usable.enabled)
            {
                m_uiController.ShowObjectActivationText(true);
                outlineObject = m_usable.GetComponent<Outline>();
                //outlineObject.enabled = true;                
                m_outlineFader.Enable(outlineObject);
                m_lastOutlineObject = outlineObject;

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

            if (m_usable != m_lastUsable && m_lastUsable != null)
            {
                m_lastUsable.GetComponent<Outline>().enabled = false;
            }
            m_lastUsable = m_usable;
        }
        else
        {
            m_uiController.ShowObjectActivationText(false); 

            if (m_lastUsable != null)
            {
                m_lastUsable.GetComponent<Outline>().enabled = false;
            }
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
        m_playerController.enabled = !m_playerController.enabled;
        m_cameraMovement.enabled = !m_cameraMovement.enabled;
    }

    private void DiaryNotif(DiaryInteractable m_interactable)
    {
        int layerMask = 1 << m_interactable.gameObject.layer;
        if ((layerMask & m_thiefEye.layerThiefEye) != 0 && !m_thiefEye.hasStarted)
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
            m_camera.transform.localPosition = Vector3.zero;

            m_uiController.ShowFigureText(false, ""); 
            m_drawingRuneController.currentRune = null;
        }
        else
        {
            Quaternion newRot = usable.gameObject.transform.rotation;
            transform.rotation = Quaternion.Euler(0f, newRot.eulerAngles.y + 180, 0f);
            m_camera.transform.localRotation = Quaternion.identity;

            Vector3 newPos = usable.transform.position;
            Vector3 position = transform.position;
            transform.position = new Vector3(newPos.x, position.y, newPos.z);
            transform.position -= transform.forward * m_offset;

            Vector3 cameraPos = m_camera.transform.position;
            m_camera.transform.position = new Vector3(cameraPos.x, newPos.y, cameraPos.z);

            m_drawingRuneController.currentRune = m_runeDraw.drawableLine.rune;
        }

        m_isUIBlocked = !m_isUIBlocked;
    }

    private void PlayInteractionSound(Usable usable)
    {
        Debug.LogWarning(usable.interactableSound);

        if (usable.interactableSound == null)
            return;
                
        usable.interactableSound.PlayPitchedSound();
    }

    private void DisableLastOutlineObject()
    {
        if (m_lastOutlineObject)
        {
            //m_lastOutlineObject.enabled = false;
            m_outlineFader.Disable(m_lastOutlineObject);
        }
    }
}