using UnityEngine;

public class ObjectTake : Usable
{
    [SerializeField] private string m_objectName;
    public override void Use() 
    {
        Inventory.instance.AddItem(m_objectName);
        Destroy(gameObject);
    }
}
