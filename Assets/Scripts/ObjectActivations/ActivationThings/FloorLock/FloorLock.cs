using UnityEngine;
using DG.Tweening;

public class FloorLock :Usable
{
    [SerializeField] float m_openTimer = 0.3f;
    [SerializeField] Vector3 m_angleRotation = new Vector3(0,0,45);
    [SerializeField] Transform m_rotateObjectl;
    private Rigidbody m_lock;
    private Rigidbody m_bin;
    public bool m_isUnlocked = false;

    private void Awake()
    {
        m_bin = m_rotateObjectl.GetComponent<Rigidbody>();
        m_lock = GetComponent<Rigidbody>();
        m_lock.isKinematic = true;
        m_bin.isKinematic = true;
    }
    public override void Use()
    {   
        if (m_isUnlocked) return;
        m_isUnlocked = true;
        m_lock.isKinematic = false;
        m_bin.isKinematic = false;
        m_rotateObjectl.DORotate(m_angleRotation, m_openTimer);
    }
}
