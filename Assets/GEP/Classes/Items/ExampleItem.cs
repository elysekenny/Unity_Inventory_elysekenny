using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItem : MonoBehaviour, IPickupable
{
    public ItemData ItemData;
    public bool is_dirty = false;
    public void Pickup(InventoryHolder inventory)
    {
        if (inventory.InventorySystem.AddToInventory(ItemData)) 
        {
            Destroy(gameObject);
        }

        is_dirty= true;
    }

}
