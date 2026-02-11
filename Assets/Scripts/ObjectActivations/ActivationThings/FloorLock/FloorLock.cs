using UnityEngine;
using DG.Tweening;

public class FloorLock :Usable
{
    [SerializeField] float m_openTimer = 0.3f;
    [SerializeField] Vector3 m_angleRotation = new Vector3(0,0,45);
    [SerializeField] Transform m_rotateObjectl;
    private void Awake()
    {
        
    }
    public override void Use()
    {
        gameObject.AddComponent<Rigidbody>();
        m_rotateObjectl.DORotate(m_angleRotation, m_openTimer);
    }
}
