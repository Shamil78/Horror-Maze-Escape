using UnityEngine;
using UnityEngine.Events;

public class KeyTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string tagFilter = "";

    [Header("Events")]
    [SerializeField] private UnityEvent onTriggerEnter = new UnityEvent();
    [SerializeField] private UnityEvent onTriggerExit = new UnityEvent();
    [SerializeField] private bool enableDebugLogs = false;

    private bool playerInside = false;
    private bool triggerActivated = false; // ✅ New flag to track activation

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter)) return;

        playerInside = true;
        triggerActivated = false; // ✅ Reset activation state when re-entering

        if (enableDebugLogs)
            Debug.Log($"Player entered: {other.gameObject.name}", this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter)) return;

        playerInside = false;
        triggerActivated = false; // ✅ Reset activation when leaving

        if (enableDebugLogs)
            Debug.Log($"Player exited: {other.gameObject.name}", this);

        onTriggerExit?.Invoke();
    }

    private void Update()
    {
        if (playerInside && Input.GetButtonDown("Action") && !triggerActivated)
        {
            ActivateTrigger();
        }
    }

    private void ActivateTrigger()
    {
        triggerActivated = true; // ✅ Prevent multiple activations in one entry
        onTriggerEnter?.Invoke();
    }
}
