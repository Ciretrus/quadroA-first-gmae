using System;
using UnityEngine;

public class TeleportTrigger : Usable
{
    public event Action<Action> Trigger;

    [SerializeField] private Transform m_teleportPoint;

    protected bool m_conditionMet;

    public bool conditionMet { set { m_conditionMet = value; } }

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        if (m_conditionMet)
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
