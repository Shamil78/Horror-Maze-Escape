using UnityEngine;

public enum ItemType
{
    Consumable,
    Weapon,
    HealthPotion,
    SanityPotion,
    AmmoBox,
    AttackPowerBoost,
    DefenseBoost
}

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Sprite itemIcon;
    [TextArea][SerializeField] private string itemDescription;
    [SerializeField] private ItemType itemType; // 🆕 Item Type (Consumable or Weapon)

    private InventoryManager inventoryManager;
    private bool playerNearby = false; // ✅ Track if player is near

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true; // ✅ Mark player as near the item
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false; // ❌ Mark player as NOT near the item
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E)) // ✅ Pick up only when 'E' is pressed
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        GameObject newItemPrefab = Instantiate(itemPrefab); // ✅ Create a fresh copy before storing
        int leftOverItems = inventoryManager.AddItem(itemName, quantity, newItemPrefab, itemIcon, itemDescription, itemType);

        if (leftOverItems <= 0)
        {
            Destroy(gameObject); // ✅ Remove item from the world
        }
        else
        {
            quantity = leftOverItems;
        }
    }
}