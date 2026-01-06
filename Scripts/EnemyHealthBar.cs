using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar; // Assign the UI slider in the inspector
    public Transform target; // Player or camera to face

    private EnemyHealth enemyHealth;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        target = Camera.main.transform;
    }

    private void Update()
    {
        if (enemyHealth != null)
        {
            healthBar.value = (float)enemyHealth.CurrentHealth / enemyHealth.maxHealth;
        }

        // Keep the health bar facing the player
        if (target != null)
        {
            transform.LookAt(target);
            transform.Rotate(0, 180, 0); // Rotate to face correctly
        }
    }
}
