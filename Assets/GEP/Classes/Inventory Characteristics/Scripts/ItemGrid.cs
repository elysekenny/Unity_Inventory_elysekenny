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
        item_slots[x, y] = null;
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

    public void PlaceItem(InventoryItem item_to_place, int pos_x, int pos_y)
    {
        RectTransform rectTransform = item_to_place.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        item_slots[pos_x, pos_y] = item_to_place;

        Vector2 position = new Vector2();
        position.x = pos_x *tile_size_width + (tile_size_width) * item_to_place.item_data.Width / 2;
        position.y = -(pos_y * tile_size_height + (tile_size_height) * item_to_place.item_data.Height / 2);

        rectTransform.localPosition = position;
    }
}
