using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData item_data;

    internal void Set(ItemData itemData)
    {
        this.item_data = itemData;

        GetComponent<Image>().sprite = itemData.Icon;

        Vector2 size = new Vector2();
        size.x = itemData.Width * ItemGrid.tile_size_width;
        size.y = itemData.Height * ItemGrid.tile_size_height;
        GetComponent<RectTransform>().sizeDelta = size;

    }
}
