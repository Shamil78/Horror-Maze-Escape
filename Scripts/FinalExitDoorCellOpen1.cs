using UnityEngine;
using System.Collections; // Required for coroutines
using TMPro;

public class FinalExitDoorCellOpen : MonoBehaviour
{
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject TheDoor;
    public AudioSource CreakSound;
    public GameObject ExtraCross;
    public NavMeshObstacleController obstacleController; // Reference to new script
    public GameObject FadeScreenIn;
    public Transform teleportDestination; // 🟢 New: Destination to teleport the player
    public TextMeshProUGUI TextBox;

    [Header("Voice Over Clips")]
    public AudioSource audioSource;
    public AudioClip line1Audio;
    public AudioClip line2Audio;

    private bool isOpen = false;
    private Animation doorAnimation;

    private void Start()
    {
        doorAnimation = TheDoor.GetComponent<Animation>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionDisplay.SetActive(true);
            ActionText.SetActive(true);
            ExtraCross.SetActive(true);
        }

        if (Input.GetButtonDown("Action") && !isOpen)
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);
    }

    private void OpenDoor()
    {
        isOpen = true;
        this.GetComponent<BoxCollider>().enabled = false;
        ActionDisplay.SetActive(false);
        ActionText.SetActive(false);

        CreakSound.Play();
        doorAnimation.Play("DoorOpen");

        // Disable NavMeshObstacle in the parent GameObject
        if (obstacleController != null)
        {
            obstacleController.DisableObstacle();
        }

        // Start coroutine to teleport player after door animation
        StartCoroutine(TeleportPlayerAfterDoorOpens());
    }

    private IEnumerator TeleportPlayerAfterDoorOpens()
    {
        // 🕒 Wait for door animation to finish
        AnimationClip doorClip = doorAnimation.GetClip("DoorOpen");
        if (doorClip != null)
        {
            FadeScreenIn.SetActive(true);
            yield return new WaitForSeconds(doorClip.length);
            FadeScreenIn.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1.5f); // Fallback delay if animation isn't found
        }

        // 🟢 Teleport the player
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find the FPC
        if (player != null && teleportDestination != null)
        {
            player.transform.position = teleportDestination.position; // Move player
            player.transform.rotation = teleportDestination.rotation; // Align rotation

            StartCoroutine(NextSequence());
        }
    }
    IEnumerator NextSequence()
    {
        yield return new WaitForSeconds(2);
        TextBox.text = "...Why am I still here.";
        audioSource.clip = line1Audio;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        TextBox.text = "";

        yield return new WaitForSeconds(4f);
        TextBox.text = "I need to get out of here.";
        audioSource.clip = line2Audio;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        TextBox.text = "";
    }

    public void CloseDoor()
    {
        isOpen = false;
        this.GetComponent<BoxCollider>().enabled = true;

        // Enable NavMeshObstacle again
        if (obstacleController != null)
        {
            obstacleController.EnableObstacle();
        }
    }
}
