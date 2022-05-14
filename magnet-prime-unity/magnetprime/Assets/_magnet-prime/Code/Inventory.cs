using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Interactable_Collectable> items;
    public static Inventory instance;

    private void Start()
    {
        instance = this;   
    }

    public void AddItem(Interactable_Collectable newItem)
    {
        items.Add(newItem);
    }

    public bool SearchForItem(Interactable_Collectable itemToSearch)
    {
        if (items.Count > 0)
        {
            foreach(Interactable_Collectable item in items)
            {
                if (item == itemToSearch)
                    return true;
            }
        }
        return false;
    }
}
