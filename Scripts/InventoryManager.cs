using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            menuActivated = !menuActivated;
            InventoryMenu.SetActive(menuActivated);
            Time.timeScale = menuActivated ? 0 : 1;

            // Enable/Disable cursor when inventory opens/closes
            Cursor.visible = menuActivated;
            Cursor.lockState = menuActivated ? CursorLockMode.None : CursorLockMode.Locked;

            if (menuActivated)
            {
                EventSystem.current.SetSelectedGameObject(null); // 🛠 Reset Event System to prevent input issues
            }
        }
    }

    public int AddItem(string itemName, int quantity, GameObject itemPrefab, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isfull && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemPrefab, itemSprite, itemDescription, itemType);
                if (leftOverItems > 0)
                {
                    return AddItem(itemName, leftOverItems, itemPrefab, itemSprite, itemDescription, itemType);
                }
                return 0;
            }
        }

        Debug.Log("Inventory Full! Could not add: " + itemName);
        return quantity;
    }


    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].ThisItemSelected = false;
        }
    }
}