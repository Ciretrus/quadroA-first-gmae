using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class FloorLock : Usable
{
    [SerializeField] private Rigidbody m_lock;
    [SerializeField] private TeleportTrigger m_teleportTrigger;
    [SerializeField] private Vector3 m_angleRotation = new Vector3(0, 0, 45);
    [SerializeField] private Transform m_rotateObject;
    [SerializeField] private float m_openTimer = 0.3f;

    private bool m_hasKey => Inventory.instance.HasItem(GlobalConstants.LockKey);
    private bool m_isUnlocked;
    private Tweener m_tween;

    private void Awake()
    {
        m_lock.isKinematic = true;
    }

    public override void Use()
    {
        if (m_isUnlocked || !m_hasKey)
        {
            return;
        }

        m_tween?.Kill();
        m_isUnlocked = true;
        m_lock.isKinematic = false;
        m_tween = m_rotateObject.DORotate(m_angleRotation, m_openTimer);
        Inventory.instance.RemoveItem(GlobalConstants.LockKey);

        m_teleportTrigger.conditionMet = true;
    }
}
