using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField][Range(400, 1600)] private int m_sensivity;
    [SerializeField] private Transform m_player;
    private float m_rotationX = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // new input system
        // couldn't get it to work
        /*Vector2 mousePosition = m_input.Movement.Point.ReadValue<Vector2>();
        float mouseX = mousePosition.x * m_sensivity * Time.deltaTime;
        float mouseY = mousePosition.y * m_sensivity * Time.deltaTime;*/

        float mouseX = Input.GetAxis("Mouse X") * m_sensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * m_sensivity * Time.deltaTime;

        m_rotationX -= mouseY;
        m_rotationX = Mathf.Clamp(m_rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(m_rotationX, 0f, 0f);
        m_player.Rotate(Vector3.up * mouseX);
    }
}
