using System;
using UnityEngine;

public class TeleportTrigger : Usable
{
    public event Action<Action> OnTrigger;

    [SerializeField] private Transform m_teleportPoint;

    protected bool m_conditionMet;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        if (m_conditionMet)
        {
            OnTrigger.Invoke(Teleport);
        }
    }

    private void Teleport()
    {
        Vector3 position = m_teleportPoint.position;
        PlayerController.instance.SetPosition(position);
    }
}
