using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public EnemyWaveSpawner waveSpawner; // Reference to the wave spawner

    private void Start()
    {
        if (waveSpawner != null)
        {
            waveSpawner.RegisterTrigger(this); // Register this trigger with the spawner
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waveSpawner.StartWave(); // Start the wave
        }
    }

    public void DestroyTrigger()
    {
        Destroy(gameObject); // Called by spawner to destroy this trigger
    }
}
