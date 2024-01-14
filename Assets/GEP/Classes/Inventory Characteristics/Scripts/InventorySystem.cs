using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public ItemData latest_item;
    public bool inventory_updated = false;

    public InventorySystem()
    {
        inventorySlots = new List<InventorySlot>(2);

        for(int i =0; i < 2; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(ItemData itemToAdd)
    {
        //store previous item at first index, and hold most recent picked up item in index 1
        inventorySlots[0].UpdateInventorySlot(inventorySlots[1].Item_Data);
        inventorySlots[1].UpdateInventorySlot(itemToAdd);

        latest_item = itemToAdd;
        inventory_updated = true;
        return true;
    }

}
