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
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, 
            targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;

    }

    public void SetColour(InventoryItem targetItem)
    {
        Image highlighterColour = targetItem.GetComponent<Image>();
        highlighterColour.color = targetItem.item_data.SlotColour;
    }
}
