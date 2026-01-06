using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class Resume : MonoBehaviour
{
    public GameObject fadeOut;
    public GameObject loadText;
    public AudioSource buttonClick;
    public GameObject loadButton;

    public GameObject pauseMenuPanel; // Assign the panel (PauseResumeSystem) here in the Inspector
    public GameObject OptionMenu;
    public Pistol pistolScript; // ✅ Assign your Pistol.cs script in the Inspector

    private int loadInt;
    private bool isPaused = false;
    private bool pauseInputBuffered = false;

    void Start()
    {
        loadInt = PlayerPrefs.GetInt("AutoSave", 2);
        if (loadInt > 0)
        {
            loadButton.SetActive(true);
        }

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Resume") && !pauseInputBuffered)
        {
            pauseInputBuffered = true;
            StartCoroutine(HandlePauseInput());
        }
    }

    IEnumerator HandlePauseInput()
    {
        yield return null; // Wait one frame to consume initial input (like mouse click)

        if (!isPaused)
            PauseGame();
        else
            ContinueGameButton();

        pauseInputBuffered = false;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // ✅ Disable shooting script
        if (pistolScript != null)
            pistolScript.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ContinueGameButton()
    {
        StartCoroutine(ContinueGameStart());
    }

    IEnumerator ContinueGameStart()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        loadText.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // ✅ Re-enable shooting script
        if (pistolScript != null)
            pistolScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fadeOut.SetActive(false);
        loadText.SetActive(false);
    }

    public void NewGameButton()
    {
        StartCoroutine(NewGameStart());
    }

    IEnumerator NewGameStart()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        loadText.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        loadText.SetActive(false);
        fadeOut.SetActive(false);
    }

    public void BackButton()
    {
        buttonClick.Play();
        OptionMenu.SetActive(false);
    }

    public void OptionGameButton()
    {
        StartCoroutine(OptionGameStart());
    }

    IEnumerator OptionGameStart()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        loadText.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        OptionMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        fadeOut.SetActive(false);
        loadText.SetActive(false);
    }

    public void LoadGameButton()
    {
        StartCoroutine(LoadGameStart());
    }

    IEnumerator LoadGameStart()
    {
        buttonClick.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        loadText.SetActive(true);
        yield return new WaitForSecondsRealtime(1);

        Time.timeScale = 1f;
        SceneManager.LoadScene(loadInt);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fadeOut.SetActive(false);
        loadText.SetActive(false);
    }

    public void Loadsite()
    {
        Application.OpenURL("https://assetstore.unity.com/");
    }
}
