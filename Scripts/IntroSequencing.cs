using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroSequencing : MonoBehaviour
{
    public TextMeshProUGUI TriggerMassages;
    public GameObject dataDisplay;
    public GameObject placeDisplay;

    [Header("Voice Over Clips")]
    public AudioSource audioSource;
    public AudioClip line1Audio;
    public AudioClip line2Audio;
    public AudioClip line3Audio;
    public AudioClip line4Audio;
    public AudioClip line5Audio;
    public AudioClip BigThud;
    public AudioClip BodyFell;
    public GameObject fadeOut;
    public GameObject LoadText;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(SequenceBegin());
    }

    IEnumerator SequenceBegin()
    {
        yield return new WaitForSeconds(3);
        placeDisplay.SetActive(true);
        yield return new WaitForSeconds(1);
        dataDisplay.SetActive(true);
        yield return new WaitForSeconds(4);
        placeDisplay.SetActive(false);
        dataDisplay.SetActive(false);
        yield return new WaitForSeconds(1);

        TriggerMassages.gameObject.SetActive(true);

        // Line 1
        TriggerMassages.text = "The night of 31st October 2004 changed me forever.";
        audioSource.clip = line1Audio;
        audioSource.Play();
        yield return new WaitForSeconds(line1Audio.length);
        yield return new WaitForSeconds(3);
        TriggerMassages.text = "";
        yield return new WaitForSeconds(5);

        // Line 2
        TriggerMassages.text = "I headed out investigate the hauting sounds in the woods. ";
        audioSource.clip = line2Audio;
        audioSource.Play();
        yield return new WaitForSeconds(line2Audio.length);
        yield return new WaitForSeconds(3);
        TriggerMassages.text = "";
        yield return new WaitForSeconds(6);

        // Line 3
        TriggerMassages.text = "I stumbled upon a clearing with a cabin the distance.";
        audioSource.clip = line3Audio;
        audioSource.Play();
        yield return new WaitForSeconds(line3Audio.length);
        yield return new WaitForSeconds(3);
        TriggerMassages.text = "";
        yield return new WaitForSeconds(6);

        // Line 4
        TriggerMassages.text = "I could hear those sounds again coming from there.";
        audioSource.clip = line4Audio;
        audioSource.Play();
        yield return new WaitForSeconds(line4Audio.length);
        yield return new WaitForSeconds(3);
        TriggerMassages.text = "";
        yield return new WaitForSeconds(6);

        // Line 5
        TriggerMassages.text = "Little did I knew that was only the beginning.";
        audioSource.clip = line5Audio;
        audioSource.Play();
        yield return new WaitForSeconds(line5Audio.length);
        yield return new WaitForSeconds(4);
        TriggerMassages.text = "";
        yield return new WaitForSeconds(7.9f);

        audioSource.clip = BigThud;
        audioSource.Play();
        yield return new WaitForSeconds(1);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(.5f);
        audioSource.clip = BodyFell;
        audioSource.Play();
        yield return new WaitForSeconds(1);

        LoadText.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(2);
    }
}
