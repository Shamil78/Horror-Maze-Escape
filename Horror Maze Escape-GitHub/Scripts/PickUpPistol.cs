using UnityEngine;
using UnityEngine.UI;

public class PickUpPistol : MonoBehaviour
{
    public GameObject Gun;
    public Transform WeaponParent;
    public GameObject ActionDisplay;
    public GameObject PickUpText;
    public GameObject ExtraCross;
    public GameObject GuideArrow;

    private bool isEquipped = false;
    private Pistol pistolScript; // Reference to Pistol script

    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
        ActionDisplay.SetActive(false);
        PickUpText.SetActive(false);
        ExtraCross.SetActive(false);

        // Get the Pistol script and disable it at start
        pistolScript = Gun.GetComponent<Pistol>();
        pistolScript.enabled = false;
    }

    void Update()
    {
        // Drop weapon if equipped and player presses "F"
        if (isEquipped && Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    void Drop()
    {
        isEquipped = false;
        WeaponParent.DetachChildren();
        Gun.GetComponent<Rigidbody>().isKinematic = false;
        Gun.GetComponent<BoxCollider>().enabled = true;
        Gun.GetComponent<MeshCollider>().enabled = true;
        Gun.transform.rotation = Quaternion.identity;

        // Disable the Pistol script when dropping
        pistolScript.enabled = false;
    }

    void Equip()
    {
        isEquipped = true;
        Gun.GetComponent<Rigidbody>().isKinematic = true;
        Gun.GetComponent<BoxCollider>().enabled = false;
        Gun.GetComponent<MeshCollider>().enabled = false;

        // Attach gun to player hand
        Gun.transform.position = WeaponParent.position;
        Gun.transform.rotation = WeaponParent.rotation;
        Gun.transform.SetParent(WeaponParent);

        // Enable the Pistol script when picked up
        pistolScript.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionDisplay.SetActive(true);
            PickUpText.SetActive(true);
            ExtraCross.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Equip();

                ActionDisplay.SetActive(false);
                PickUpText.SetActive(false);
                GuideArrow.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionDisplay.SetActive(false);
            PickUpText.SetActive(false);
            GuideArrow.SetActive(false);
        }
    }
}
