using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class ItemsActivations : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private Camera m_camera;
    [SerializeField] private UIController m_uiController;
    [SerializeField] private ThiefEye m_thiefEye;
    [SerializeField] private float m_rayDistance = 1f;
    [SerializeField] private OutlineFader m_outlineFader;    

    private CameraMovement m_cameraMovement;
    private Vector3 m_screenCenter;
    private Usable m_usable;
    private DiaryInteractable m_interactable;
    private bool m_isUIBlocked;
    private Outline m_lastOutlineObject;

    private void Awake()
    {
        m_cameraMovement = m_camera.GetComponent<CameraMovement>();
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
            if (hit.collider.gameObject.TryGetComponent(out m_usable))
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
                                ChangeDrawingMode(m_usable);
                                break;
                            }
                    }
                    m_usable.Use();

                    PlayInteractionSound(m_usable);
                }
            }
            else
            {
                TryDiaryNotif(hit.collider);
                DisableLastOutlineObject();
            }
        }
        else
        {
            m_uiController.ShowObjectActivationText(false);
            //if (m_usable != null)
            //{
            //    m_usable.GetComponent<Outline>().enabled = false;
            //}
            DisableLastOutlineObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            TryDiaryNotif(other);
        }
    }

    public void ChangeUIMode(InputAction.CallbackContext context)
    {
        ChangeMovementState();

        m_isUIBlocked = !m_isUIBlocked;
    }

    private void TryDiaryNotif(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out m_interactable))
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
    }

    private void ChangeMovementState()
    {
        m_playerController.enabled = !m_playerController.enabled;
        m_cameraMovement.enabled = !m_cameraMovement.enabled;

        ChangeCursorState();
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

        if (m_isUIBlocked)
        {
            m_camera.transform.localPosition = Vector3.zero;

            m_uiController.ShowFigureText(false, "");
        }
        else
        {
            Quaternion newRot = usable.gameObject.transform.rotation;
            transform.rotation = Quaternion.Euler(0f, newRot.eulerAngles.y + 180, 0f);
            m_camera.transform.localRotation = Quaternion.identity;

            Vector3 newPos = usable.transform.position;
            Vector3 position = transform.position;
            transform.position = new Vector3(newPos.x, position.y, newPos.z);
            transform.position -= transform.forward;

            Vector3 cameraPos = m_camera.transform.position;
            m_camera.transform.position = new Vector3(cameraPos.x, newPos.y, cameraPos.z);
        }

        m_isUIBlocked = !m_isUIBlocked;
    }

    private void PlayInteractionSound(Usable usable)
    {
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