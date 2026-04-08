using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private Transform m_camera;

    void LateUpdate()
    {
        transform.position = m_camera.position+m_offset;
        transform.rotation = m_camera.rotation;
    }
}
