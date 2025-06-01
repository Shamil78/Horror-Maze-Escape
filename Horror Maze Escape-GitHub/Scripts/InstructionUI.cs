using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionUI : MonoBehaviour
{
    public GameObject instructionPanel;
    public TextMeshProUGUI instructionText;
    public Button nextButton;
    public Button skipButton;

    private List<string> instructions = new List<string>
    {
        "Use WASD to move. Press 'E' to interact with objects. Find all chests in the maze before exiting. Kill zombies to earn points.",

        "Open inventory using 'I'. Select an item, then press 'E' to equip, unequip, or consume it if it's a potion.",

        "Open the shop using 'I'. Spend your kill points to purchase items that help in your survival.",

        "Press 'Tab' to open the Main Menu or pause the game. Use the Options menu to view keybindings and settings.",

        "Keep an eye on your Sanity bar and Health bar. If health drops to zero, the game ends.",

        "When Sanity reaches 50, you lose 2 health every 30 seconds. At 75, lose 2 health every 10 seconds. At 100, lose 5 health every 10 seconds."
    };

    private int currentIndex = 0;

    private void Start()
    {
        instructionPanel.SetActive(false); // Hide on start

        nextButton.onClick.AddListener(ShowNextInstruction);
        skipButton.onClick.AddListener(SkipInstructions);

        StartCoroutine(ShowInstructionsWithDelay(10f)); // Show after 10 seconds
    }

    private IEnumerator ShowInstructionsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (instructions.Count > 0)
        {
            currentIndex = 0;
            instructionText.text = instructions[currentIndex];
            instructionPanel.SetActive(true);

            // Pause game and show cursor
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void ShowNextInstruction()
    {
        currentIndex++;
        if (currentIndex < instructions.Count)
        {
            instructionText.text = instructions[currentIndex];
        }
        else
        {
            CloseInstructionPanel();
        }
    }

    private void SkipInstructions()
    {
        CloseInstructionPanel();
    }

    private void CloseInstructionPanel()
    {
        instructionPanel.SetActive(false);

        // Resume game and hide cursor
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
