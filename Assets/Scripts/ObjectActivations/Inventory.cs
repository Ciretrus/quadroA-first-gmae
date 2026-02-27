using System.Collections.Generic;
using UnityEngine;


public class InventoryItem
{
    public string itemName;
    public Sprite image;
    public int count;

    public InventoryItem(string itemName, Sprite image, int count = 1)
    {
        this.itemName = itemName;
        this.image = image;
        this.count = count;
    }
    
}
public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }

    private List<InventoryItem> inventory = new List<InventoryItem>();
    public int Counter { get { return inventory.Count; }}
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(InventoryItem item)
    {
        if (ContainsItem(item.itemName))
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemName == item.itemName) inventory[i].count++;
            }
        }
        else
        {
            inventory.Add(item);
        }
        Debug.Log(item.itemName + item.count);
    }

    public void RemoveItem(string itemName)
    {
        if (ContainsItem(itemName))
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemName == itemName)
                {   
                    if (inventory[i].count > 1) { inventory[i].count--; }
                    else { inventory.Remove(inventory[i]); }
                }
            }
        }
        else 
        {
            Debug.LogError($"thing didnt found, can't delete it - {itemName}");   
        }
    }
    
    public bool ContainsItem(string itemName) 
    {
        for (int i = 0; i < inventory.Count; i++) 
        {
            if (inventory[i].itemName == itemName) return true;
        }
        return false;
    }
    public InventoryItem GetItem(int index) { return inventory[index]; }
    
}
