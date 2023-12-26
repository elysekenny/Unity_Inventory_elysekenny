using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerInteraction : MonoBehaviour
{
    private InventoryHolder inventory;

    private void Awake()
    {
        inventory = GetComponent<InventoryHolder>();
    }
    void OnCollisionEnter(Collision collision)
    {
        IPickupable pickupable = collision.gameObject.GetComponent<IPickupable>();
        if (pickupable != null)
        {
            pickupable.Pickup(inventory);
        }
    }
}
