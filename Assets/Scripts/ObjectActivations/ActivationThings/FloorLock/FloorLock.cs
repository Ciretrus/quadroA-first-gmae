using UnityEngine;
using DG.Tweening;

public class FloorLock :Usable
{
    [SerializeField] private Vector3 m_angleRotation = new Vector3(0, 0, 45);
    [SerializeField] private Transform m_rotateObject;
    [SerializeField] private float m_openTimer = 0.3f;

    private Tweener m_tween;
    private Rigidbody m_lock;

    public bool m_isUnlocked = false;

    private void Awake()
    {
        m_lock = GetComponent<Rigidbody>();
        m_lock.isKinematic = true;
    }

    public override void Use()
    {   
        if (m_isUnlocked) return;

        m_tween?.Kill();
        m_isUnlocked = true;
        m_lock.isKinematic = false;

        m_tween = m_rotateObject.DORotate(m_angleRotation, m_openTimer);
    }
}
