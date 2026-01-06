using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuFunctions : MonoBehaviour
{
    public GameObject fadeOut;
    public GameObject loadText;
    public AudioSource buttonClick;

    public GameObject loadButton;
    private int loadInt;

    void Start()
    {
        loadInt = PlayerPrefs.GetInt("AutoSave", 2);
        if (loadInt > 0)
        {
            loadButton.SetActive(true);
        }
    }

    public void NewGameButton()
    {
        StartCoroutine(NewGameStart());
    }

    IEnumerator NewGameStart()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2);
        loadText.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1); // First scene in your build
    }

    public void LoadGameButton()
    {
        StartCoroutine(LoadGameStart());
    }

    IEnumerator LoadGameStart()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2);
        loadText.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(loadInt);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loadText.SetActive(false);
        fadeOut.SetActive(false);
    }

    public void Loadsite()
    {
        Application.OpenURL("https://assetstore.unity.com/");
    }
}