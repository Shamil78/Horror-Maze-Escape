using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton for easy access
    public int PlayerScore => playerScore;

    [Header("UI Elements")]
    public Slider healthBar; // Player health bar
    public TextMeshProUGUI healthText; // Player health text
    public TextMeshProUGUI killCounterText; // Enemy kills
    public TextMeshProUGUI scoreText; // Score display
    public TextMeshProUGUI ammoText; // Ammo count display

    private int enemyKillCount = 0;
    private int playerScore = 0;

    [Header("UI Elements")]
    public Slider sanityBar; // Add this
    public TextMeshProUGUI SanityText;

    public void UpdateSanity(float currentSanity, float maxSanity)
    {
        if (sanityBar != null)
        {
            // Activate sanity UI only if sanity is greater than 0
            bool shouldShow = currentSanity > 0f;

            sanityBar.gameObject.SetActive(shouldShow);

            if (shouldShow)
            {
                sanityBar.value = currentSanity / maxSanity;
                SanityText.text = $"{currentSanity}";
            }
        }
    }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}";
    }

    public void EnemyKilled(int scoreValue)
    {
        enemyKillCount++;
        playerScore += scoreValue;
        UpdateUI();
    }

    public void UpdateAmmo(int currentAmmo, int storedAmmo)
    {
        ammoText.gameObject.SetActive(true);
        Debug.Log($"Updating Ammo UI: {currentAmmo} / {storedAmmo}");
        if (ammoText == null)
        {
            Debug.LogError("AmmoText UI is not assigned in UIManager!");
            return;
        }
        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager.Instance is NULL! Make sure UIManager is in the scene.");
        }

        ammoText.text = $"Ammo: {currentAmmo} Reload: {storedAmmo}";
    }

    private void UpdateUI()
    {
        killCounterText.text = $"Kills: {enemyKillCount}";
        scoreText.text = $"Point: {playerScore}";
    }
    public void UpdateScore(int amount)
    {
        playerScore += amount;
        scoreText.text = $"Point: {playerScore}";
    }

}