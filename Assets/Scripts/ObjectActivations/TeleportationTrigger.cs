using UnityEngine;

public class TeleportationTrigger : Usable
{
    [SerializeField] private Transform m_teleportPoint;
    private bool m_hasKey => Inventory.instance.HasItem(GlobalConstants.ChestKey);

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        if (m_hasKey)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        Vector3 position = m_teleportPoint.position;
        PlayerController.instance.SetPosition(position);
    }
}
