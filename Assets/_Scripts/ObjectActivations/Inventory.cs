using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

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

    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName]++;
        }
        else
        {
            inventory.Add(itemName, 1);
        }
        Debug.Log(itemName + inventory[itemName]);
    }

    public void RemoveItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            if (inventory[itemName] > 0)
            {
                inventory[itemName]--;
            }
            else
            {
                inventory.Remove(itemName);
            }
        }
        else 
        {
            Debug.LogError($"thing wasn't found, can't delete it - {itemName}");   
        }
    }

    public bool HasItem(string itemName) 
    {
        return inventory.ContainsKey(itemName);
    }
}
