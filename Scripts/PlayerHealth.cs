using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false; // Prevents multiple death calls

    [Header("Death Settings")]
    public string nextSceneName; // Scene to load when player dies
    public float delay = 0.5f; // Delay before loading next scene
    public GameObject DeathBlackOut; // UI fade effect

    private Animator deathAnimator; // Animator for the DeathBlackOut UI

    [Header("Sanity System")]
    public float sanity = 0f; // 0 to 100
    public float maxSanity = 100f;
    public float sanityDamageInterval50 = 30f;
    public float sanityDamageInterval75 = 20f;
    private float lastSanityDamageTime = 0f;

    private void Start()
    {
        currentHealth = maxHealth;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }

        if (DeathBlackOut != null)
        {
            deathAnimator = DeathBlackOut.GetComponent<Animator>(); // Get Animator component
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            float interval = -1f;
            float damageAmount = 0f;

            if (sanity >= 100)
            {
                interval = 10f;
                damageAmount = 5f;
            }
            else if (sanity >= 75)
            {
                interval = sanityDamageInterval75; // 10 seconds
                damageAmount = 2f;
            }
            else if (sanity >= 50)
            {
                interval = sanityDamageInterval50; // 30 seconds
                damageAmount = 2f;
            }

            if (interval > 0 && Time.time - lastSanityDamageTime >= interval)
            {
                TakeDamage(damageAmount);
                lastSanityDamageTime = Time.time;
            }
        }
    }

    public void IncreaseSanity(float amount)
    {
        sanity += amount;
        sanity = Mathf.Clamp(sanity, 0, maxSanity);

        Debug.Log("Sanity Increased: " + sanity);

        // Optionally update a UI sanity bar here
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateSanity(sanity, maxSanity);
        }
    }

    private int damageReduction = 0; // Default: No damage reduction

    public void ReduceEnemyDamage(int amount)
    {
        damageReduction += amount; // Increase damage resistance
        Debug.Log("Damage reduction increased by: " + amount);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // Ignore damage if already dead

        // Apply damage reduction
        float finalDamage = Mathf.Max(damage - damageReduction, 0); // Ensure damage never goes negative
        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }


    public void IncreaseHealth(float amount)
    {
        if (isDead) return; // Don't heal if dead

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void DecreaseSanity(float amount)
    {
        sanity -= amount;
        sanity = Mathf.Clamp(sanity, 0, maxSanity);

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateSanity(sanity, maxSanity);
        }

        Debug.Log("Sanity decreased: " + sanity);
    }


    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died!");

        // Activate DeathBlackOut UI and play animation
        if (DeathBlackOut != null)
        {
            DeathBlackOut.SetActive(true);
            if (deathAnimator != null)
            {
                deathAnimator.Play("DeathBlackOut"); // Play the animation
            }
        }

        // Load next scene after delay
        Invoke("LoadNextScene", delay);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
