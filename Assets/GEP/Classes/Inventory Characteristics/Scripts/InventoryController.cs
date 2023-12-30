using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private ItemGrid itemGrid;

    private void Update()
    {
        if (itemGrid == null) { return; }

        Debug.Log(itemGrid.GetTileGridPosition(Input.mousePosition));
    }
}
