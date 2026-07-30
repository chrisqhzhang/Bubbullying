using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventory;
    bool inventoryIsOpen = false;

    // Update is called once per frame
    void Update()
    {
        OpenInventory();
    }

    void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryIsOpen = !inventoryIsOpen;
            inventory.SetActive(inventoryIsOpen);
        }
    }
}
