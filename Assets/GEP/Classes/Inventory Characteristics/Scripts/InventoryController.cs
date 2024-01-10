using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemGrid itemGrid;

    InventoryItem selected_item;
    InventoryItem overlapItem;
    InventoryItem item_to_remove;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject item_prefab;
    [SerializeField] GameObject pickupable_prefab;
    
    [SerializeField] Transform canvas_transform;
    public ExampleItem item_collected;
    public GameObject item_name;
    public GameObject item_description;

    [SerializeField] ItemData test_item;

    InventoryHighlight inventoryHighlight;
    InventoryItem item_to_highlight;
    Vector2Int oldPosition;

    [SerializeField] GameObject player;

    private bool on_hover = false;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
        
    }

    private void Update()
    {
        if (itemGrid == null) { return; }

        ItemDrag();
        HandleHighlight();
        AddToInventory();

        if (on_hover)
        {
            RemoveItem();
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            SelectItem();
        }

        UpdateText();

    }

    private void InsertRandomItem(ItemData item_to_insert)
    {
        if (itemGrid == null) { return; }

        CreateItem(item_to_insert);
        InventoryItem item_to_add = selected_item;
        selected_item = null;
        InsertItem(item_to_add);
    }

    private void InsertItem(InventoryItem item_to_add)
    {
        // the question mark means the variable can return null
        Vector2Int? posOnGrid = itemGrid.FindSpaceForObject(item_to_add);

        //cannot place item
        if(posOnGrid == null)   { return;}

        itemGrid.PlaceItem(item_to_add, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTilePosition();

        if(oldPosition == positionOnGrid)
        {
            return;
        }

        oldPosition = positionOnGrid;
        if (selected_item == null)
        {
            item_to_highlight = itemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if(item_to_highlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(item_to_highlight);
                inventoryHighlight.SetParentForHighlight(itemGrid);
                inventoryHighlight.SetPosition(itemGrid, item_to_highlight);
                inventoryHighlight.SetColour(item_to_highlight);

                on_hover = true;
            }
            else
            {
                inventoryHighlight.Show(false);
                on_hover = false;
            }
        }
        else
        {
            inventoryHighlight.Show(itemGrid.boundaryCheck(positionOnGrid.x, positionOnGrid.y, selected_item.item_data.Width, selected_item.item_data.Height));
            inventoryHighlight.SetSize(selected_item);
            inventoryHighlight.SetParentForHighlight(itemGrid);
            inventoryHighlight.SetPosition(itemGrid, selected_item, positionOnGrid.x, positionOnGrid.y);
            inventoryHighlight.SetColour(item_to_highlight);
        }
    }

    private void RemoveItem()
    {
        Vector2Int item_location = new Vector2Int();
        if (Input.GetMouseButtonDown(1))
        {
            //can use item to highlight because thats the hovered item
            Debug.Log(item_to_highlight.item_data.DisplayName);

            //free up slots
            for(int x = 0; x < item_to_highlight.item_data.Width; x++) 
            {
                item_location.x = item_to_highlight.onGridPositionX + x;
                for (int y = 0; y < item_to_highlight.item_data.Height; y++)
                {                 
                    item_location.y = item_to_highlight.onGridPositionY + y;
                    itemGrid.item_slots[item_location.x, item_location.y] = null;
                }
            }
            //destroy actual item in the slot
            //spawn new item
            GameObject floor_item = Instantiate(pickupable_prefab);

            //sets the item data to the data of the removeed item
            ExampleItem floor_data= floor_item.GetComponent<ExampleItem>();
            floor_data.ItemData = item_to_highlight.item_data;

            Transform floor_rect = floor_item.gameObject.GetComponent<Transform>();
            Vector3 floor_pos = new Vector3();
            floor_pos.x = player.transform.position.x;
            floor_pos.y = 1f;
            floor_pos.z = player.transform.position.z + 1;
            floor_rect.position = floor_pos;

            Destroy(item_to_highlight.gameObject);

            //removes the highlight
            inventoryHighlight.Show(false);

            //destroy image game object
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
                    rectTransform.SetAsLastSibling();
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
            name_text.color = item_to_highlight.item_data.SlotColour;
            name_text.text = item_to_highlight.item_data.DisplayName;
            description_text.text = item_to_highlight.item_data.DisplayDescription;

        }
    }

    public void AddToInventory()
    {
        InventoryHolder inventory = player.GetComponent<InventoryHolder>();
        if(inventory.InventorySystem.inventory_updated)
        {
            InsertRandomItem(inventory.InventorySystem.latest_item);
            inventory.InventorySystem.inventory_updated = false;

            Debug.Log("item added");
        }
        
    }
}
