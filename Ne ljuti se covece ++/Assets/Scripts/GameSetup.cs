using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    public Button redColorButton;
    public Button blueColorButton;
    public Button greenColorButton;
    public Button yellowColorButton;

    public Button easyDifficultyButton;
    public Button mediumDifficultyButton;
    public Button hardDifficultyButton;

    public Button startGameButton;

    private void SetButtonListeners()
    {
        redColorButton.onClick.RemoveAllListeners();
        blueColorButton.onClick.RemoveAllListeners();
        greenColorButton.onClick.RemoveAllListeners();
        yellowColorButton.onClick.RemoveAllListeners();

        redColorButton.onClick.AddListener(delegate { SetColor("red"); });
        redColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        blueColorButton.onClick.AddListener(delegate { SetColor("blue"); });
        blueColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        greenColorButton.onClick.AddListener(delegate { SetColor("green"); });
        greenColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        yellowColorButton.onClick.AddListener(delegate { SetColor("yellow"); });
        yellowColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        easyDifficultyButton.onClick.RemoveAllListeners();
        mediumDifficultyButton.onClick.RemoveAllListeners();
        hardDifficultyButton.onClick.RemoveAllListeners();

        easyDifficultyButton.onClick.AddListener(delegate { SetDifficulty("easy"); });
        easyDifficultyButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        mediumDifficultyButton.onClick.AddListener(delegate { SetDifficulty("medium"); });
        mediumDifficultyButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        hardDifficultyButton.onClick.AddListener(delegate { SetDifficulty("hard"); });
        hardDifficultyButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        startGameButton.interactable = false;
        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(StartGame);
        startGameButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
    }

    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        this.SetButtonListeners();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GoBack();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            this.OptionsMenu();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            this.HelpMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (GameController.instance.playerColor != "" && GameController.instance.gameDifficulty != "")
                this.StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            this.SetColor("red");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            this.SetColor("blue");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            this.SetColor("green");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            this.SetColor("yellow");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.SetDifficulty("easy");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.SetDifficulty("medium");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.SetDifficulty("hard");
        }
    }

    internal void SetColor(string color)
    {
        GameController.instance.playerColor = color;

        if (GameController.instance.gameDifficulty != "")
            startGameButton.interactable = true;
    }

    internal void SetDifficulty(string difficulty)
    {
        GameController.instance.gameDifficulty = difficulty;
    
        if (GameController.instance.playerColor != "")
            startGameButton.interactable = true;
    }

    internal void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void GoBack()
    {
        AudioManager.instance.PlayButtonPressedSound();
        GameController.instance.playerColor = "";
        GameController.instance.gameDifficulty = "";
        SceneManager.LoadScene(0);
    }

    public void OptionsMenu()
    {
        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(4);
    }

    public void HelpMenu()
    {
        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(5);
    }
}
