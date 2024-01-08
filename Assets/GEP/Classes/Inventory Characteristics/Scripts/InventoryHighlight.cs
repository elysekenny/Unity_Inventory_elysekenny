using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool is_showing)
    {
        highlighter.gameObject.SetActive(is_showing);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.item_data.Width * ItemGrid.tile_size_width;
        size.y = targetItem.item_data.Height * ItemGrid.tile_size_height;
        highlighter.sizeDelta= size;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;

    }

    //overloaded method
    public void SetPosition(ItemGrid target_grid, InventoryItem target_item, int pos_x, int pos_y)
    {
        Vector2 pos = target_grid.CalculatePositionOnGrid(target_item, pos_x, pos_y);
        highlighter.localPosition = pos;
    }

    public void SetColour(InventoryItem targetItem)
    {
        Image highlighterColour = highlighter.GetComponent<Image>();
        Color colour = targetItem.item_data.SlotColour;
        //makes slot translucent
        colour.a = 100;
        highlighterColour.color = colour;
    }

    public void SetParentForHighlight(ItemGrid targetGrid)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

}
