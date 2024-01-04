using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemGrid itemGrid;

    InventoryItem selected_item;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject item_prefab;
    [SerializeField] GameObject slot;
    [SerializeField] Transform canvas_transform;
    [SerializeField] ExampleItem item_collected;
    public GameObject item_name;
    public GameObject item_description;

    private void Update()
    {
        if (itemGrid == null) { return; }

        ItemDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateItem(item_collected.getItemPickedUp());
        }


        if (Input.GetMouseButtonDown(0))
        {
            SelectItem();
        }

        UpdateText();

    }

    private void CreateItem(ItemData item_data)
    {
        InventoryItem item_to_add = Instantiate(item_prefab).GetComponent<InventoryItem>();
        selected_item = item_to_add;

        rectTransform = item_to_add.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas_transform);

        item_to_add.Set(item_data);
    }
    private void CreateSlotBackground()
    {
        Image slot_background = Instantiate(slot).GetComponent<Image>();

        rectTransform = slot_background.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas_transform);

        slot_background.color = selected_item.item_data.SlotColour;

        Vector2 size = new Vector2();
        size.x = selected_item.item_data.Width * ItemGrid.tile_size_width;
        size.y = selected_item.item_data.Height * ItemGrid.tile_size_height;
        GetComponent<RectTransform>().sizeDelta = size;
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
            bool complete = itemGrid.PlaceItem(selected_item, tile_grid_pos.x, tile_grid_pos.y, ref overlapItem);
            if (complete) { selected_item = null; }        
        }
    }

    private void ItemDrag()
    {
        if (selected_item != null) { rectTransform.position = Input.mousePosition; }
    }

    private void UpdateText()
    {
        TextMeshProUGUI name_text = item_name.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description_text = item_description.GetComponent<TextMeshProUGUI>();

        //name_text.color = selected_item.item_data.SlotColour;
        name_text.text = selected_item.item_data.DisplayName;  
        description_text.text = selected_item.item_data.DisplayDescription;
    }
}
