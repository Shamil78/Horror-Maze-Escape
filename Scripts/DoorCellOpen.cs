using UnityEngine;

public class DoorCellOpen : MonoBehaviour
{
    public GameObject ActionDisplay;
    public GameObject ActionText;
    public GameObject TheDoor;
    public AudioSource CreakSound;
    public GameObject ExtraCross;
    public NavMeshObstacleController obstacleController; // Reference to new script

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
