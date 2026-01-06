using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class ItemsActivations : MonoBehaviour
{
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private Camera m_camera;
    [SerializeField] private UIController m_uiController;
    [SerializeField] private float m_rayDistance = 1f;

    private CameraMovement m_cameraMovement;
    private Vector3 m_screenCenter;
    private Usable m_usable;
    private bool m_isUIBlocked;

    private void Awake()
    {
        m_cameraMovement = m_camera.GetComponent<CameraMovement>();
    }

    private void Update()
    {
        m_screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = m_camera.ScreenPointToRay(m_screenCenter);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, m_rayDistance);

        if (hit.collider != null)
        {
            m_usable = hit.collider.gameObject.GetComponent<Usable>();

            if (m_usable != null)
            {
                m_uiController.ShowObjectActivationText(true);
                m_usable.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (m_usable.type)
                    {
                        case UsableType.NonBlocking: Debug.Log("Interacted with Non-Blocking UI thing"); break;
                        case UsableType.Blocking:
                            {
                                ChangeUIMode(m_usable);
                                break;
                            }
                    }
                    m_usable.Use();
                }
            }
        }
        else
        {
            m_uiController.ShowObjectActivationText(false);
            if (m_usable != null)
            {
                m_usable.GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void ChangeUIMode(Usable usable)
    {
        m_playerController.enabled = !m_playerController.enabled;
        m_cameraMovement.enabled = !m_cameraMovement.enabled;

        if (m_isUIBlocked)
        {
            m_camera.transform.localPosition = Vector3.zero;
            Cursor.lockState = CursorLockMode.Locked;

            m_uiController.ShowFigureText(false, "");
        }
        else
        {
            Quaternion newRotation = usable.gameObject.transform.rotation;
            m_camera.transform.rotation = Quaternion.Euler(0f, newRotation.eulerAngles.y + 180, 0f);

            Vector3 newPosition = usable.transform.position;
            m_camera.transform.position = newPosition;
            Vector3 cameraPos = m_camera.transform.localPosition;
            float z = cameraPos.z - 1f;
            m_camera.transform.localPosition = new Vector3(cameraPos.x, cameraPos.y, z);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        m_isUIBlocked = !m_isUIBlocked;
    }
}