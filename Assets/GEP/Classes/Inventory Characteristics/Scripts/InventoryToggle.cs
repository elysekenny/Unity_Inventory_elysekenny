using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    private bool inventory_is_open;
    private CanvasRenderer screen;
    private void Awake()
    {
        inventory_is_open = false;
        screen = GetComponent<CanvasRenderer>();
    }

    private void Update()
    {
        if(inventory_is_open)
        {
            //if the button is pressed hide the inventory
        }
        else
        {
            //show the inventory
        }
    }
}
