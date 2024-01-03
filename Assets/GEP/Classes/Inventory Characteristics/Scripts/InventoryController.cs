using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemGrid itemGrid;

    InventoryItem selected_item;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject item_prefab;
    [SerializeField] Transform canvas_transform;

    private void Update()
    {
        if (itemGrid == null) { return; }

        ItemDrag();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            CreateItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectItem();
        }

    }

    private void CreateItem()
    {
        InventoryItem item_to_add = Instantiate(item_prefab).GetComponent<InventoryItem>();
        selected_item = item_to_add;

        rectTransform = item_to_add.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas_transform);

        int selected_item_ID = UnityEngine.Random.Range(0, items.Count);
        item_to_add.Set(items[selected_item_ID]);
    }

    private void SelectItem()
    {
        Vector2Int tile_grid_pos = itemGrid.GetTileGridPosition(Input.mousePosition);

        if (selected_item == null)
        {
            //pick up item
            selected_item = itemGrid.PickUpItem(tile_grid_pos.x, tile_grid_pos.y);
            if (selected_item != null)
            {
                rectTransform = selected_item.GetComponent<RectTransform>();
            }

        }
        else
        {
            //place item
            itemGrid.PlaceItem(selected_item, tile_grid_pos.x, tile_grid_pos.y);
            selected_item = null;
        }
    }

    private void ItemDrag()
    {
        if (selected_item != null) { rectTransform.position = Input.mousePosition; }
    }
}
