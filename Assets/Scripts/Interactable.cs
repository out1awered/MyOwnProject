using System.Collections;
using System.Collections.Generic;


using UnityEngine;

public class Interactable : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private DoorController doorController;

    public bool isKey = false;

    public string GetInteractionText()
    {
        if (doorController != null)
        {
            if (doorController.IsLocked())
            {
                if (inventoryManager != null &&
                    inventoryManager.HasKey())
                {
                    return "Press E to Unlock Door";
                }

                return "Door Locked - Key Required";
            }

            if (doorController.IsOpen())
            {
                return "Press E to Close Door";
            }
            else
            {
                return "Press E to Open Door";
            }
        }

        if (isKey)
        {
            return "Press E to Pick Up Key";
        }

        return "Press E to Pick Up";
    }

    void Start()
    {
        inventoryManager =
            FindFirstObjectByType<InventoryManager>();

        doorController =
            GetComponentInParent<DoorController>();
    }

    public void Interact()
    {
        if (doorController != null)
        {
            if (doorController.IsLocked())
            {
                if (inventoryManager == null ||
                    !inventoryManager.UseKey())
                {
                    
                    return;
                }

                doorController.UnlockDoor();
                
            }

            doorController.ToggleDoor();
            return;
        }

        if (isKey)
        {
            if (inventoryManager == null)
            {
                Debug.LogWarning("InventoryManager not found!");
                return;
            }

            inventoryManager.AddKey();
            gameObject.SetActive(false);
            return;
        }

        if (inventoryManager != null)
        {
            inventoryManager.AddItem();
        }

        gameObject.SetActive(false);
    }
}

