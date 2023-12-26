using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    private int inventory_width;
    private int inventory_height;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for(int i =0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(ItemData itemToAdd)
    {
        if(HasFreeSlot(out InventorySlot freeSlot))
        {
            //adds item to next slot
            freeSlot.UpdateInventorySlot(itemToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }
        else
        {
            //no slots are free!
            return false;
        }
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.Item_Data == null);
        return freeSlot == null ? false : true;
    }
}
