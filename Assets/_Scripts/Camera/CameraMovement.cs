using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField][Range(0.001f, 1f)] private float m_smoothTime = 0.01f;
    [SerializeField][Range(400, 1600)] private int m_sensivity;
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_cameraPos;
    
    [Header ("CameraShake")]
    [SerializeField] private float m_duration = 0.5f;
    [SerializeField] private float m_strength = 1f;
    [SerializeField] private int m_frequency = 10;
    [SerializeField] private float m_randomness = 90;

    private Tween m_shakeTween;
    private float m_rotationX = 0f;
    private Vector3 m_velocity;
    private Transform m_shakerPos;
    private Vector3 m_shakerStartPos;

    private void Awake()
    {
        var shaker = new GameObject();
        m_shakerPos = shaker.transform;
        m_shakerPos.position = m_cameraPos.position;
        m_shakerStartPos = m_shakerPos.position;
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
        var shakerShift = m_shakerStartPos - m_shakerPos.position;
        transform.position = Vector3.SmoothDamp(transform.position, m_cameraPos.position, ref m_velocity, m_smoothTime);
        transform.position += shakerShift;
    }
    public void CameraShake()
    {
        transform.DOKill(true);
        m_shakeTween = m_shakerPos.DOShakePosition(m_duration, m_strength, m_frequency, m_randomness, false, true);
    }
}
