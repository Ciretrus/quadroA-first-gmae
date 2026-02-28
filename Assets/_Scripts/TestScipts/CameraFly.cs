using UnityEngine;
using UnityEngine.IO;

public class CameraFly : MonoBehaviour
{
    public float moveSpeed = 10f;       // скорость перемещения
    public float lookSpeed = 3f;        // скорость вращения
    public float fastSpeed = 30f;       // ускорение при Shift
    public float slowSpeed = 5f;        // замедление при Ctrl

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked; // скрываем курсор
    }

    void Update()
    {
        // Вращение камеры мышью
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -89f, 89f); // ограничение по вертикали
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Определяем текущую скорость
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) currentSpeed = fastSpeed;
        if (Input.GetKey(KeyCode.LeftControl)) currentSpeed = slowSpeed;

        // Движение камеры
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += forward;
        if (Input.GetKey(KeyCode.S)) move -= forward;
        if (Input.GetKey(KeyCode.D)) move += right;
        if (Input.GetKey(KeyCode.A)) move -= right;
        if (Input.GetKey(KeyCode.E)) move += up;    // вверх
        if (Input.GetKey(KeyCode.Q)) move -= up;    // вниз

        transform.position += move * currentSpeed * Time.deltaTime;
    }
}
