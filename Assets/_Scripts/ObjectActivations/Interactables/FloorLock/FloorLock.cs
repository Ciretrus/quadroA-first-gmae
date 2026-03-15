using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class FloorLock : Interactable
{
    [SerializeField] private Rigidbody m_lock;
    [SerializeField] private Transform m_rotateObject;
    [SerializeField] private TeleportTrigger m_teleportTrigger;

    [Header("Lock opening settings")]
    [SerializeField] private float m_openTimer = 0.3f;
    [SerializeField] private Vector3 m_angleRotation = new Vector3(-33f, 0f, 180f);
    [SerializeField] private Vector3 m_force = new Vector3(0f, 4f, 2f);

    [Header("Sounds")]
    [SerializeField] private AudioClip[] m_closedSound;
    [SerializeField] private AudioClip[] m_openedSound;

    private Tweener m_tween;
    private bool m_hasKey => ServiceLocator.Resolve<Inventory>().ContainsItem(GlobalConstants.Collectables.LockKey);
    private bool m_isUnlocked;

    private void Awake()
    {
        m_lock.isKinematic = true;
        m_interactableSound.ChangeSounds(m_closedSound);
    }

    public override void Use()
    {
        if (m_isUnlocked || !m_hasKey)
        {
            m_interactableSound.PlaySound();
            return;
        }

        m_interactableSound.ChangeSounds(m_openedSound);
        m_interactableSound.PlaySound();
        m_interactableSound.ChangeSounds();

        m_tween?.Kill();

        m_isUnlocked = true;
        m_lock.isKinematic = false;
        m_lock.AddForce(m_force,ForceMode.Impulse);
        m_tween = m_rotateObject.DOLocalRotate(m_angleRotation, m_openTimer);
        // Don't remove key for showing off inventory at presentation :D
        //ServiceLocator.Resolve<Inventory>().RemoveItem(GlobalConstants.Collectables.LockKey);

        m_teleportTrigger.conditionMet = true;
    }
}
