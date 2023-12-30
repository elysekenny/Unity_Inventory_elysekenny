using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    private bool inventory_is_open;
    private Canvas screen;
    private void Awake()
    {
        inventory_is_open = false;
        screen = GetComponent<Canvas>();
    }

    private void Update()
    {
        if(inventory_is_open)
        {
            //if the button is pressed hide the inventory
            screen.enabled = false;
        }
        else
        {
            //show the inventory
            screen.enabled = true;
        }
    }
}
