using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public GameObject itemPrefab;
    public Sprite itemSprite;
    public bool isfull;
    public string itemDescription;
    public ItemType itemType;

    [SerializeField] private int maxNumberOfItems;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool ThisItemSelected;

    private InventoryManager inventoryManager;
    private PlayerHealth playerHealth;
    private Transform weaponParent;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        weaponParent = GameObject.Find("InventoryWeapon").transform;
    }
    public int AddItem(string itemName, int quantity, GameObject itemPrefab, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (isfull) return quantity;

        this.itemName = itemName;
        this.itemPrefab = itemPrefab;
        this.itemSprite = itemSprite;
        this.itemType = itemType; // 🆕 Store type
        itemImage.sprite = itemSprite;
        itemImage.enabled = true;
        this.itemDescription = itemDescription;

        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            isfull = true;
            quantityText.text = this.quantity.ToString();
            quantityText.gameObject.SetActive(true);
            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.gameObject.SetActive(true);
        return 0;
    }

    private void Update()
    {
        if (ThisItemSelected && Input.GetKeyDown(KeyCode.E))
        {
            UseItem();
        }
    }
    public void UseItem()
    {
        switch (itemType)
        {
            case ItemType.HealthPotion:
                playerHealth.IncreaseHealth(playerHealth.maxHealth * 0.2f); // 20% of max health
                quantity--;

                if (quantity <= 0)
                {
                    ClearSlot();
                }
                else
                {
                    quantityText.text = quantity.ToString();
                }
                break;  
                
            case ItemType.AttackPowerBoost:
                Pistol equippedPistol = FindFirstObjectByType<Pistol>(); // Find the active pistol
                if (equippedPistol != null)
                {
                    equippedPistol.IncreasePistolDamage(25); // Boost pistol damage
                }
                else
                {
                    Debug.LogWarning("No pistol found! Can't apply Attack Power Boost.");
                }
                break;

            case ItemType.AmmoBox:
                RefillAmmo(15); // Assuming ammo is managed in PlayerHealth
                break;

            case ItemType.DefenseBoost:
                playerHealth.ReduceEnemyDamage(3); // Reduce zombie attack damage by 1
                quantity--;
                if (quantity <= 0) ClearSlot();
                else quantityText.text = quantity.ToString();
                break;

            case ItemType.Weapon:
                EquipWeapon();
                ClearSlot();
                break;

            case ItemType.SanityPotion:
                if (playerHealth != null)
                {
                    playerHealth.DecreaseSanity(20f); // Reduce sanity by 20
                    Debug.Log("Sanity Potion used. Sanity reduced by 20.");
                }
                break;

            default:
                Debug.Log("Unknown item type used.");
                return;
        }

        // Reduce item quantity and update UI
        quantity--;
        if (quantity <= 0) ClearSlot();
        else quantityText.text = quantity.ToString();
    }

    private void EquipWeapon()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning("Item prefab is missing!");
            return;
        }

        foreach (Transform child in weaponParent)
        {
            Destroy(child.gameObject);
        }

        GameObject newWeapon = Instantiate(itemPrefab, weaponParent);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.SetParent(weaponParent);

        Pistol pistol = newWeapon.GetComponent<Pistol>();
        if (pistol != null)
        {
            pistol.canShoot = true;
            Debug.Log("Pistol equipped! Can Shoot: " + pistol.canShoot);
        }

        itemPrefab = null;
    }

    public void ClearSlot()
    {
        itemName = "";
        quantity = 0;
        isfull = false;
        itemImage.enabled = false;
        quantityText.gameObject.SetActive(false);
        selectedShader.SetActive(false);
        ThisItemSelected = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            ThisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            ItemDescriptionImage.sprite = itemSprite;
        }
    }
    public void RefillAmmo(int amount)
    {
        Pistol pistol = FindFirstObjectByType<Pistol>(); // Get the Pistol script
        if (pistol != null)
        {
            pistol.IncreaseStoredAmmo(amount);
        }
    }

}