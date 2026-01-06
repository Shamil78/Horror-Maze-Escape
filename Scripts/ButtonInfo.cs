using UnityEngine;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public TMP_Text PriceTxt;
    public TMP_Text QuantityTxt;
    private ShopManagerScript shopManager;

    private void Start()
    {
        shopManager = FindFirstObjectByType<ShopManagerScript>();
        UpdateButtonUI(); // 🛠 Ensure UI updates when inventory opens
    }

    public void UpdateButtonUI()
    {
        if (shopManager.shopItemDictionary.TryGetValue(ItemID, out ShopManagerScript.ShopItem item))
        {
            PriceTxt.text = "Pt:" + item.Price.ToString();
            QuantityTxt.text = item.Quantity.ToString();
        }
    }
}