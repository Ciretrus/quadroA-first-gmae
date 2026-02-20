using System.Collections.Generic;
using UnityEngine;

public class Conditions : MonoBehaviour
{
    public static Conditions instance { get; private set; }

    private HashSet<string> conditions = new HashSet<string>();

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

    public void AddCondition(string itemName)
    {
        if (!conditions.Contains(itemName))
        {
            conditions.Add(itemName);
        }
        else
        {
            Debug.Log(itemName + " already here");
        }
    }

    public void RemoveCondition(string itemName)
    {
        if (conditions.Contains(itemName))
        {
            conditions.Remove(itemName);
        }
        else 
        {
            Debug.LogError($"thing wasn't found, can't delete it - {itemName}");   
        }
    }

    public bool HasCondition(string itemName) 
    {
        return conditions.Contains(itemName);
    }
}
