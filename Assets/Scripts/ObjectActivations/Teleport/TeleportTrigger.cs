using System;
using UnityEngine;

public class TeleportTrigger : Usable
{
    public event Action<Action> OnTrigger;

    [SerializeField] private Transform m_teleportPoint;
    [SerializeField] private string m_objectName;
    [SerializeField] private string m_conditionName;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        if (m_objectName != "" || m_conditionName != "")
        {
            if (m_objectName != "" && m_conditionName != "")
            {
                if (Inventory.instance.HasItem(m_objectName) && Conditions.instance.HasCondition(m_conditionName))
                {
                    OnTrigger.Invoke(Teleport);
                    return;
                }
            }
            if (Inventory.instance.HasItem(m_objectName) || Conditions.instance.HasCondition(m_conditionName))
            {
                OnTrigger.Invoke(Teleport);
                return;
            }
        }
    }

    private void Teleport()
    {
        Vector3 position = m_teleportPoint.position;
        PlayerController.instance.SetPosition(position);
    }
}
