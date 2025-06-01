using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Trigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private bool destroyOnTriggerExit = false; // 🔄 Now destruction happens after exit
    [SerializeField] private string tagFilter = "";

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private float textDisplayDuration = 3f;

    [Header("Enter Messages & Delays")]
    [SerializeField] private string[] enterMessages;
    [SerializeField] private float[] enterMessageDelays;

    [Header("Exit Messages & Delays")]
    [SerializeField] private string[] exitMessages;
    [SerializeField] private float[] exitMessageDelays;

    [Header("Events")]
    [SerializeField] private UnityEvent onTriggerEnter = new UnityEvent();
    [SerializeField] private UnityEvent onTriggerExit = new UnityEvent();
    [SerializeField] private bool enableDebugLogs = false;

    private Coroutine messageRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter)) return;

        if (enableDebugLogs)
            Debug.Log($"Triggered by: {other.gameObject.name}", this);

        onTriggerEnter?.Invoke();

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(DisplayMessages(enterMessages, enterMessageDelays, false)); // ❌ Don't destroy on enter
    }

    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter)) return;

        if (enableDebugLogs)
            Debug.Log($"Exited trigger: {other.gameObject.name}", this);

        onTriggerExit?.Invoke();

        if (messageRoutine != null)
            StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(DisplayMessages(exitMessages, exitMessageDelays, destroyOnTriggerExit)); // ✅ Destroy on exit
    }

    private IEnumerator DisplayMessages(string[] messages, float[] delays, bool shouldDestroy)
    {
        if (dialogText == null || messages.Length == 0) yield break;

        // 🔹 Show messages one by one
        for (int i = 0; i < messages.Length; i++)
        {
            dialogText.text = messages[i];
            dialogText.gameObject.SetActive(true);

            float delay = (i < delays.Length) ? delays[i] : 1.5f;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(textDisplayDuration); // Final message display time
        dialogText.gameObject.SetActive(false);

        // 🔥 ✅ Only destroy **after exit messages** are finished
        if (shouldDestroy)
        {
            if (enableDebugLogs)
                Debug.Log($"Destroying {gameObject.name} after exit messages finished.", this);

            Destroy(gameObject);
        }
    }
}
