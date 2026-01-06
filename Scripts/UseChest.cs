using UnityEngine;
using TMPro;
using System.Collections;

public class UseChest : MonoBehaviour
{
    public GameObject actionDisplay; // UI prompt (e.g., "Press E to open")
    public TextMeshProUGUI PickUpText; // UI text
    public GameObject chestLid; // The chest object with Animation component
    public AudioSource openSound; // Sound effect when opening the chest

    private bool isOpened = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            actionDisplay.SetActive(true);
            PickUpText.gameObject.SetActive(true); // Show text UI
            PickUpText.text = "Open the Chest"; // Set text

            if (Input.GetButtonDown("Action"))  // Player pressed interaction key
            {
                OpenChest();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        actionDisplay.SetActive(false);
        PickUpText.gameObject.SetActive(false); // Hide text when player leaves
    }

    private void OpenChest()
    {
        isOpened = true;  // Prevents reopening
        actionDisplay.SetActive(false);
        PickUpText.gameObject.SetActive(false); // Hide text

        StartCoroutine(CollectKey());

        if (chestLid != null)
        {
            // Play the ChestOpen animation
            chestLid.GetComponent<Animation>().Play("ChestOpen");
        }
        if (openSound != null)
        {
            openSound.Play();
        }

        GetComponent<BoxCollider>().enabled = false; // Disable interaction after opening
    }
    IEnumerator CollectKey()
    {
        PickUpText.gameObject.SetActive(true); // Show text UI
        PickUpText.text = "Key Collected";
        yield return new WaitForSeconds(1.5f);
        PickUpText.gameObject.SetActive(false);
        PickUpText.text = "PickUp the Gun";
    }
}
