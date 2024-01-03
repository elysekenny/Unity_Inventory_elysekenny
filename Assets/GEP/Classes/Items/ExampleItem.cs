using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItem : MonoBehaviour, IPickupable
{
    public ItemData ItemData;
    public void Pickup(InventoryHolder inventory)
    {
        if (inventory.InventorySystem.AddToInventory(ItemData)) 
        {
            Destroy(gameObject); 
        }
    }

    public ItemData getItemPickedUp()
    {
        return ItemData;
    }
}
