using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    InventoryItem[,] item_slots;

    public const float tile_size_width = 83.64f;
    public const float tile_size_height = 95;

    public Vector2 GridPos = new Vector2();
    Vector2Int TileGridPos = new Vector2Int();

    RectTransform rectTransform;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridWidth, gridHeight);
    }
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = item_slots[x, y];

        if(toReturn == null) { return null; }

        for(int i = 0; i < toReturn.item_data.Width; i++)
        {
            for(int j = 0; j < toReturn.item_data.Height; j++)
            {
                item_slots[toReturn.onGridPositionX + i, toReturn.onGridPositionY + j] = null;
            }
        }

        return toReturn;
    }

    private void Init(int width, int height)
    {
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

    public bool PlaceItem(InventoryItem item_to_place, int pos_x, int pos_y)
    {
        //if out of boundaries do not allow item to be placed
        if(!boundaryCheck(pos_x, pos_y, item_to_place.item_data.Width, item_to_place.item_data.Height)) { return false; }

        RectTransform rectTransform = item_to_place.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for(int x = 0; x< item_to_place.item_data.Width; x++)
        {
            for(int y = 0; y < item_to_place.item_data.Height; y++)
            {
                item_slots[pos_x + x, pos_y + y] = item_to_place;
            }
        }

        item_to_place.onGridPositionX = pos_x;
        item_to_place.onGridPositionY = pos_y;

        Vector2 position = new Vector2();
        position.x = pos_x *tile_size_width + (tile_size_width) * item_to_place.item_data.Width / 2;
        position.y = -(pos_y * tile_size_height + (tile_size_height) * item_to_place.item_data.Height / 2);

        rectTransform.localPosition = position;

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

    bool boundaryCheck(int pos_X, int pos_y, int width, int height)
    {
        if(isTileValid(pos_X, pos_y) == false) { return false; }

        pos_X += width - 1;
        pos_y += height - 1;

        if(isTileValid(pos_X, pos_y) == false) { return false; }

        return true;
    }
}
