using UnityEngine;

public class TakeableObject : Usable
{
    [Header("Enter in lowercase!")]
    [SerializeField] private string m_objectName;

    private void Awake()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use() 
    {
        Inventory.instance.AddItem(m_objectName);
        Destroy(gameObject);
    }
}
