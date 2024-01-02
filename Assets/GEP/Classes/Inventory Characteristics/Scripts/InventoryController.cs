using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemGrid itemGrid;

    InventoryItem selected_item;
    RectTransform rectTransform;

    private void Update()
    {
        if (itemGrid == null) { return; }

        if(selected_item != null) { rectTransform.position = Input.mousePosition; }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tile_grid_pos = itemGrid.GetTileGridPosition(Input.mousePosition);

            if(selected_item == null)
            {
                //pick up item
                selected_item = itemGrid.PickUpItem(tile_grid_pos.x, tile_grid_pos.y);
                if(selected_item != null)
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
       
    }
}
