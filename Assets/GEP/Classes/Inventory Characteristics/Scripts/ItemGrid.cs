using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour
{
    public InventoryItem[,] item_slots;

    public const float tile_size_width = 83.64f;
    public const float tile_size_height = 95;

    public Vector2 GridPos = new Vector2();
    Vector2Int TileGridPos = new Vector2Int();

    RectTransform rectTransform;

    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;

    [SerializeField] GameObject line;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridWidth, gridHeight);
    }
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = item_slots[x, y];

        if (toReturn == null) { return null; }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int i = 0; i < item.item_data.Width; i++)
        {
            for (int j = 0; j < item.item_data.Height; j++)
            {
                item_slots[item.onGridPositionX + i, item.onGridPositionY + j] = null;
            }
        }
    }

    private void Init(int width, int height)
    {
        DrawGrid();

        item_slots = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tile_size_width, height * tile_size_height);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePos)
    {
        GridPos.x = mousePos.x - rectTransform.position.x;
        GridPos.y = rectTransform.position.y - mousePos.y;

        TileGridPos.x = (int)(GridPos.x / tile_size_width);
        TileGridPos.y = (int)(GridPos.y / tile_size_height);

        return TileGridPos;
    }

    public bool PlaceItem(InventoryItem item_to_place, int pos_x, int pos_y, ref InventoryItem overlapItem)
    {
        //if out of boundaries do not allow item to be placed
        if (!boundaryCheck(pos_x, pos_y, item_to_place.item_data.Width, item_to_place.item_data.Height)) { return false; }

        //if item does not pass the overlap check item cannot be placed!
        if (!OverlapCheck(pos_x, pos_y, item_to_place.item_data.Width, item_to_place.item_data.Height, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(item_to_place, pos_x, pos_y);

        return true;
    }

    public void PlaceItem(InventoryItem item_to_place, int pos_x, int pos_y)
    {
        RectTransform rectTransform = item_to_place.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < item_to_place.item_data.Width; x++)
        {
            for (int y = 0; y < item_to_place.item_data.Height; y++)
            {
                item_slots[pos_x + x, pos_y + y] = item_to_place;
            }
        }

        item_to_place.onGridPositionX = pos_x;
        item_to_place.onGridPositionY = pos_y;
        Vector2 position = CalculatePositionOnGrid(item_to_place, pos_x, pos_y);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem item_to_place, int pos_x, int pos_y)
    {
        Vector2 position = new Vector2();
        position.x = pos_x * tile_size_width + (tile_size_width) * item_to_place.item_data.Width / 2;
        position.y = -(pos_y * tile_size_height + (tile_size_height) * item_to_place.item_data.Height / 2);
        return position;
    }

    private bool OverlapCheck(int pos_x, int pos_y, int width, int height, ref InventoryItem overlapItem)
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(item_slots[pos_x + x, pos_y + y] != null)
                {
                    if(overlapItem == null)
                    {
                        overlapItem = item_slots[pos_x + x, pos_y + y];
                    }
                    else
                    {
                        if(overlapItem != item_slots[pos_x + x, pos_y + y])
                        {
                            return false;
                        }                     
                    }
                    
                }
            }
        }

        return true;
    }

    private bool CheckAvailableSpace(int pos_x, int pos_y, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (item_slots[pos_x + x, pos_y + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool isTileValid(int pos_x, int pos_y)
    {
        if(pos_x < 0 || pos_y < 0)
        {
            return false;
        }

        if(pos_x >= gridWidth || pos_y >= gridHeight)
        {
            return false;
        }

        return true;
    }

    public bool boundaryCheck(int pos_X, int pos_y, int width, int height)
    {
        if(isTileValid(pos_X, pos_y) == false) { return false; }

        pos_X += width - 1;
        pos_y += height - 1;

        if(isTileValid(pos_X, pos_y) == false) { return false; }

        return true;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        if(x >= 0 && x < gridWidth && y >= 0 && y < gridHeight) { return item_slots[x, y]; }

        return null;
    }

    public Vector2Int? FindSpaceForObject(InventoryItem item_to_add)
    {
        int height = gridHeight - item_to_add.item_data.Height + 1;
        int width = gridWidth - item_to_add.item_data.Width + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(CheckAvailableSpace(x, y, item_to_add.item_data.Width, item_to_add.item_data.Height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
    private void DrawGrid()
    {
        RectTransform transform;
        Vector2 size = new Vector2Int();
        Vector2 pos = new Vector2Int();

        //draw vertical lines
        for (int x = 0; x < gridWidth - 1; x++)
        {
            GameObject line_to_draw = Instantiate(line);
            transform = line_to_draw.GetComponent<RectTransform>();
            transform.SetParent(this.GetComponent<RectTransform>());
            
            pos.x = (x + 1) * tile_size_width;
            pos.y = -380;
            line_to_draw.transform.localPosition = pos;            

            size.x = 1;
            size.y = gridHeight * tile_size_height;
            transform.sizeDelta = size;
        }

        //draw horizontal lines
        for (int y = 0; y < gridHeight - 1; y++)
        {
            GameObject line_to_draw = Instantiate(line);
            transform = line_to_draw.GetComponent<RectTransform>();
            transform.SetParent(this.GetComponent<RectTransform>());

            pos.x = 0;
            pos.y =  -(y + 1) * tile_size_height;
            line_to_draw.transform.localPosition = pos;
            Debug.Log(pos.y);

            size.x = gridWidth * tile_size_width;
            size.y = 1;
            transform.sizeDelta = size;
            transform.SetParent(this.GetComponent<RectTransform>());
        }
    }

}
