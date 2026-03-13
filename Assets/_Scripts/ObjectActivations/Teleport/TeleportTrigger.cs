using System;
using UnityEngine;

public class TeleportTrigger : Interactable
{
    public event Action<Action> Trigger;

    [SerializeField] private Transform m_teleportPoint;
    [SerializeField] private bool m_requiresCondition = false;

    private bool m_isTeleporting = false;

    public bool conditionMet { set { m_requiresCondition = !value; } }

    private void Awake()
    {
        Initialize(InteractableType.NonBlocking);
    }

    public override void Use()
    {
        if (!m_requiresCondition && !m_isTeleporting)
        {
            m_isTeleporting = true;
            Trigger.Invoke(Teleport);
        }
    }

    private void Teleport()
    {
        Transform transform = m_teleportPoint.transform;
        ServiceLocator.Resolve<PlayerController>().SetPosition(transform);
        m_isTeleporting = false;
    }
}
