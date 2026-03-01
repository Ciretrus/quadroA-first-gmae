using System;
using UnityEngine;

public class TeleportTrigger : Usable
{
    public event Action<Action> Trigger;

    [SerializeField] private Transform m_teleportPoint;
    [SerializeField] private bool m_requiresCondition = false;

    public bool conditionMet { set { m_requiresCondition = !value; } }

    private void Awake()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        if (!m_requiresCondition)
        {
            Trigger.Invoke(Teleport);
        }
    }

    private void Teleport()
    {
        Transform transform = m_teleportPoint.transform;
        PlayerController.instance.SetPosition(transform);
    }
}
