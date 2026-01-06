using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int CurrentHealth => currentHealth;
    public event System.Action OnDeath;
    public int maxHealth = 100;
    private int currentHealth;
    private CapsuleCollider[] enemyColliders; // Store both colliders

    private void Start()
    {
        currentHealth = maxHealth;
        enemyColliders = GetComponents<CapsuleCollider>(); // Get all CapsuleColliders
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.GetComponent<Animator>().SetBool("Death", true);
        UIManager.Instance.EnemyKilled(10); // Adds 10 points per kill
        OnDeath?.Invoke(); // Notify wave spawner

        // Disable all Capsule Colliders
        foreach (CapsuleCollider collider in enemyColliders)
        {
            collider.enabled = false;
        }

        Destroy(gameObject, 3f); // Destroy the enemy after 3 seconds
    }

}