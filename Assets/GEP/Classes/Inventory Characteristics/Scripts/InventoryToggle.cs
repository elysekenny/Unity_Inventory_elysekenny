using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    private bool inventory_is_open = false;
    public GameObject inventory_screen;

    private void Start()
    {
        inventory_is_open = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            inventory_is_open = !inventory_is_open;
        }

        inventory_screen.SetActive(inventory_is_open);
        Cursor.visible = inventory_is_open;

        if(inventory_is_open)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
