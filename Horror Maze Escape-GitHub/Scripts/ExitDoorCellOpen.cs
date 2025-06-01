using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Required for coroutines

public class ExitDoorCellOpen : MonoBehaviour
{
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject TheDoor;
    public AudioSource CreakSound;
    public GameObject ExtraCross;
    public NavMeshObstacleController obstacleController; // Reference to new script
    public string nextSceneName; // Name of the next scene
    public GameObject FadeScreenIn;

    private bool isOpen = false;

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
        TheDoor.GetComponent<Animation>().Play("DoorOpen");
        CreakSound.Play();

        // 🔴 Disable NavMeshObstacle in the parent GameObject
        if (obstacleController != null)
        {
            obstacleController.DisableObstacle();
        }

        // Start coroutine to change scene after 1 second
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(.5f);
        FadeScreenIn.SetActive(true);
        yield return new WaitForSeconds(1f); // Wait for 1 second
        SceneManager.LoadScene(nextSceneName); // Load the specified scene
    }

    public void CloseDoor()
    {
        isOpen = false;
        this.GetComponent<BoxCollider>().enabled = true;

        // 🔵 Enable NavMeshObstacle again
        if (obstacleController != null)
        {
            obstacleController.EnableObstacle();
        }
    }
}