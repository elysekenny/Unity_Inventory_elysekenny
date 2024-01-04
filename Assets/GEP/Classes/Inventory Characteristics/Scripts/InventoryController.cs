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

    InventoryHighlight inventoryHighlight;
    InventoryItem item_to_highlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        if (itemGrid == null) { return; }

        ItemDrag();
        HandleHighlight();

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

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTilePosition();

        if (selected_item == null)
        {
            item_to_highlight = itemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if(item_to_highlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(item_to_highlight);
                inventoryHighlight.SetPosition(itemGrid, item_to_highlight);
                //inventoryHighlight.SetColour(item_to_highlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {

        }
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
        Vector2Int tile_grid_pos = GetTilePosition();

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
            if (complete)
            {
                selected_item = null;
                if (overlapItem != null)
                {
                    selected_item = overlapItem;
                    overlapItem = null;
                    rectTransform = selected_item.GetComponent<RectTransform>();
                }
            }
        }
    }

    private Vector2Int GetTilePosition()
    {
        Vector2 position = Input.mousePosition;

        if (selected_item != null)
        {
            position.x -= (selected_item.item_data.Width - 1) * ItemGrid.tile_size_width / 2;
            position.y += (selected_item.item_data.Height - 1) * ItemGrid.tile_size_height / 2;
        }

        return itemGrid.GetTileGridPosition(position);
    }

    private void ItemDrag()
    {
        if (selected_item != null) { rectTransform.position = Input.mousePosition; }
    }

    private void UpdateText()
    {
        TextMeshProUGUI name_text = item_name.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description_text = item_description.GetComponent<TextMeshProUGUI>();

        if(item_to_highlight == null)
        {
            //clear the text
            name_text.text = null;
            description_text.text = null;
        }
        else
        {
            //name_text.color = item_to_highlight.item_data.SlotColour;
            name_text.text = item_to_highlight.item_data.DisplayName;
            description_text.text = item_to_highlight.item_data.DisplayDescription;

        }
    }
}
