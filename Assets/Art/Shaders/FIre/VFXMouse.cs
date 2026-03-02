using UnityEngine;
using UnityEngine.VFX;

public class VFXMouse : MonoBehaviour
{
    [SerializeField] private VisualEffect m_vfx;
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private Transform m_spherePosition;
    [SerializeField] private Transform m_firePosition;
    [SerializeField] private float m_distanceActivation = 1f;
    private float m_force = 1f;


    void Update()
    {
        float distance = Vector3.Distance(m_firePosition.position, m_spherePosition.position);
        if (distance < m_distanceActivation)
        {
            m_force = Mathf.Clamp(-(m_distanceActivation - distance) * 6,-10,0);
            m_vfx.SetFloat("Force", m_force);
            m_vfx.SetVector3("SpherePosition", m_spherePosition.position);
            m_vfx.SetBool("IsMovable", true);
        }
        else
        {
            m_vfx.SetBool("IsMovable", false);
        }


        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            m_spherePosition.position = new Vector3(hit.point.x,hit.point.y, m_spherePosition.position.z);
        }
    }
}