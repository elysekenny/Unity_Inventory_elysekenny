using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour
{
    InventoryController inventoryController;
    ItemGrid item_grid;

    private void Awake()
    {
        item_grid = GetComponent<ItemGrid>();
    }
}
