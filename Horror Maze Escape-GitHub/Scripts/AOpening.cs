using UnityEngine;
using System.Collections;
using TMPro;

public class AOpening : MonoBehaviour
{
    public GameObject FadeScreenIn;
    public TextMeshProUGUI TextBox;
    public GameObject BlackUI;
    public GameObject FadeOut;

    [Header("Voice Over Clips")]
    public AudioSource audioSource;
    public AudioClip line1Audio;
    public AudioClip line2Audio;

    void Start()
    {
        StartCoroutine(ScenePlayer());
    }
    IEnumerator ScenePlayer()
    {
        yield return new WaitForSeconds(3f);
        BlackUI.SetActive(false);
        FadeScreenIn.SetActive(true);
        yield return new WaitForSeconds(2f);
        TextBox.text = "...Where Am I";
        audioSource.clip = line1Audio;
        audioSource.Play();
        FadeScreenIn.SetActive(false);
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        FadeOut.SetActive(false);
        FadeScreenIn.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        TextBox.text = "";

        FadeScreenIn.SetActive(false);
        yield return new WaitForSeconds(4f);
        TextBox.text = "I need to get out of here.";
        audioSource.clip = line2Audio;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        TextBox.text = "";
    }
}
