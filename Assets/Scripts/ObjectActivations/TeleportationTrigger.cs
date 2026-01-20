using UnityEngine;

public class TeleportationTrigger : Usable
{
    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        Teleport();
    }

    private void Teleport()
    {
        Debug.Log("Teleported");
    }
}
