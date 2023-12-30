using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    const float tile_size = 32;
    Vector2 GridPos = new Vector2();
    Vector2Int TileGridPos = new Vector2Int();

    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public Vector2Int GetTileGridPosition(Vector2 mousePos)
    {
        GridPos.x = mousePos.x - rectTransform.position.x;
        GridPos.y = rectTransform.position.y - mousePos.y;

        TileGridPos.x = (int)(GridPos.x / tile_size);
        TileGridPos.y = (int)(GridPos.y / tile_size);

        return TileGridPos;
    }
}
