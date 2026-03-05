using UnityEngine;

public class TakeableObject : Usable
{
    [Header("Enter in lowercase!")]
    [SerializeField] private string m_objectName;
    [SerializeField] private Sprite m_image;

    private void Awake()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use() 
    {
        m_interactableSound.PlayPitchedSound();
        InventoryItem item = new InventoryItem(m_objectName, m_image);
        ServiceLocator.Resolve<Inventory>().AddItem(item);
        Destroy(gameObject);
    }
}
