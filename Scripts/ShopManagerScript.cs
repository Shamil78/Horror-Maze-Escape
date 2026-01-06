using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManagerScript : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public int ItemID;
        public int Price;
        public int Quantity;
        public GameObject ItemPrefab;
        public Sprite ItemSprite;
        public string ItemName;
        public string ItemDescription;
        public ItemType ItemType;
    }

    public List<ShopItem> shopItems = new List<ShopItem>();
    public Dictionary<int, ShopItem> shopItemDictionary = new Dictionary<int, ShopItem>();

    public TMP_Text CoinsTxt;

    public GameObject[] itemPrefabs; // Assign in Inspector
    public Sprite[] itemSprites; // Assign in Inspector

    private void Start()
    {
        UpdateScoreUI();

        // ✅ Initialize shop items
        AddShopItem(1, 10, "HealthPotion", "Heals 20% of your maximum health. A refreshing potion that restores vitality in the darkest moments.", ItemType.HealthPotion, itemPrefabs[0], itemSprites[0]);
        AddShopItem(2, 20, "AmmoBox", "Replenishes 15 bullets to your current weapon. Don’t run out when it matters most.", ItemType.AmmoBox, itemPrefabs[1], itemSprites[1]);
        AddShopItem(3, 30, "AttackPowerBoost", "Enhances your pistol's firepower by 25. When regular bullets just aren’t enough.", ItemType.AttackPowerBoost, itemPrefabs[2], itemSprites[2]);
        AddShopItem(4, 30, "DefenseBoost", "Reduces incoming zombie damage. A mystical Effect that dulls the pain of every bite.", ItemType.DefenseBoost, itemPrefabs[3], itemSprites[3]);
        AddShopItem(5, 20, "SanityPotion", "Lowers your terror by 20 points. A calming brew to hold back the madness creeping in.", ItemType.SanityPotion, itemPrefabs[4], itemSprites[4]); 
    }

    private void AddShopItem(int id, int price, string name, string description, ItemType type, GameObject prefab, Sprite sprite)
    {
        ShopItem newItem = new ShopItem
        {
            ItemID = id,
            Price = price,
            Quantity = 0,
            ItemName = name,
            ItemDescription = description,
            ItemType = type,
            ItemPrefab = prefab,
            ItemSprite = sprite
        };

        shopItems.Add(newItem);
        shopItemDictionary[id] = newItem;
    }

    public void Buy()
    {
        GameObject buttonRef = EventSystem.current.currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        if (shopItemDictionary.TryGetValue(buttonInfo.ItemID, out ShopItem item))
        {
            int playerScore = UIManager.Instance.PlayerScore; // Get Player Score

            if (playerScore >= item.Price)
            {
                // Deduct points & update UI
                UIManager.Instance.UpdateScore(-item.Price);

                item.Quantity++;
                buttonInfo.UpdateButtonUI();
                UpdateScoreUI();

                // Add to Inventory
                InventoryManager inventory = Object.FindFirstObjectByType<InventoryManager>();
                inventory.AddItem(item.ItemName, 1, item.ItemPrefab, item.ItemSprite, item.ItemDescription, item.ItemType);
            }
            else
            {
                Debug.Log("Not enough points to buy this item!");
            }
        }
    }

    private void UpdateScoreUI()
    {
        CoinsTxt.text = "Point: " + UIManager.Instance.PlayerScore;
    }
}
