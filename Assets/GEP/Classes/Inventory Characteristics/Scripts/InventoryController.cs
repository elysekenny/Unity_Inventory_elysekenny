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
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject item_prefab;
    [SerializeField] GameObject slot;
    [SerializeField] Transform canvas_transform;
    public GameObject item_name;
    public GameObject item_description;

    private void Update()
    {
        if (itemGrid == null) { return; }

        ItemDrag();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            //CreateSlotBackground();
            CreateItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectItem();
        }

        UpdateText();

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
            itemGrid.PlaceItem(selected_item, tile_grid_pos.x, tile_grid_pos.y);
            selected_item = null;
        }
    }

    private void ItemDrag()
    {
        if (selected_item != null) { rectTransform.position = Input.mousePosition; }
    }

    private void UpdateText()
    {
        item_name = GameObject.Find("Item Name");
        item_description = GameObject.Find("Description");

        TextMeshPro name_text = item_name.GetComponent<TextMeshPro>();
        TextMeshPro description_text = item_description.GetComponent<TextMeshPro>();

        name_text.SetText(selected_item.item_data.DisplayName);
        description_text.SetText(selected_item.item_data.DisplayDescription);
    }
}
