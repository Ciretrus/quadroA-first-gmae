using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField][Range(0.001f, 1f)] private float m_smoothTime = 0.01f;
    [SerializeField][Range(400, 1600)] private int m_sensivity;
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_cameraPos;

    private float m_rotationX = 0f;
    private Vector3 m_velocity;

    private void Awake()
    {
        transform.position = m_cameraPos.position;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // TODO Move to new input system
        /*Vector2 mousePosition = m_input.Movement.Point.ReadValue<Vector2>();
        float mouseX = mousePosition.x * m_sensivity * Time.deltaTime;
        float mouseY = mousePosition.y * m_sensivity * Time.deltaTime;*/

        float mouseX = Input.GetAxis("Mouse X") * m_sensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_sensivity * Time.deltaTime;

        m_rotationX -= mouseY;
        m_rotationX = Mathf.Clamp(m_rotationX, -90f, 90f);

        m_player.Rotate(Vector3.up * mouseX);
        transform.rotation = Quaternion.Euler(m_rotationX, m_player.eulerAngles.y, 0f);

        transform.position = Vector3.SmoothDamp(transform.position, m_cameraPos.position, ref m_velocity, m_smoothTime);
    }
}
