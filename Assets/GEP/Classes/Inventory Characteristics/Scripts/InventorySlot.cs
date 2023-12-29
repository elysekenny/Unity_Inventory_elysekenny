using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot 
{
    [SerializeField] private ItemData item_data;

    public ItemData Item_Data => item_data;

    public InventorySlot(ItemData source)
    {
        item_data = source;
    }

    public InventorySlot()
    {
        //empty slot
        ClearSlot();
    }

    public void ClearSlot()
    { 
        //used for when an item is dropped
        item_data = null;
    }

    public void UpdateInventorySlot(ItemData data)
    {
        item_data = data;
    }

}
