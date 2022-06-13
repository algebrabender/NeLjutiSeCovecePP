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
    public Button quickGameButton;

    private void SetButtonListeners()
    {
        redColorButton.onClick.RemoveAllListeners();
        blueColorButton.onClick.RemoveAllListeners();
        greenColorButton.onClick.RemoveAllListeners();
        yellowColorButton.onClick.RemoveAllListeners();

        redColorButton.onClick.AddListener(delegate { SetColor("red", "crvena"); });
        redColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        blueColorButton.onClick.AddListener(delegate { SetColor("blue", "plava"); });
        blueColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        greenColorButton.onClick.AddListener(delegate { SetColor("green", "zelena"); });
        greenColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        yellowColorButton.onClick.AddListener(delegate { SetColor("yellow", "žuta"); });
        yellowColorButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        easyDifficultyButton.onClick.RemoveAllListeners();
        mediumDifficultyButton.onClick.RemoveAllListeners();
        hardDifficultyButton.onClick.RemoveAllListeners();

        easyDifficultyButton.onClick.AddListener(delegate { SetDifficulty("easy", "lako"); });
        easyDifficultyButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        mediumDifficultyButton.onClick.AddListener(delegate { SetDifficulty("medium", "srednje"); });
        mediumDifficultyButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        hardDifficultyButton.onClick.AddListener(delegate { SetDifficulty("hard", "teško"); });
        hardDifficultyButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        startGameButton.interactable = false;
        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(StartGame);
        startGameButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        quickGameButton.onClick.RemoveAllListeners();
        quickGameButton.onClick.AddListener(QuickGame);
        quickGameButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
    }

    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        this.SetButtonListeners();
    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("goBack", "Escape"))))
        {
            this.GoBack();
        }
        else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("optionMenu", "O"))))
        {
            this.OptionsMenu();
        }
        else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("helpMenu", "H"))))
        {
            this.HelpMenu();
        }
        else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("newGame", "Space"))))
        {
            if (GameController.instance.playerColor != "" && GameController.instance.gameDifficulty != "")
                this.StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            this.SetColor("red", "crveno");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            this.SetColor("blue", "plavo");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            this.SetColor("green", "zeleno");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            this.SetColor("yellow", "žuto");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.SetDifficulty("easy", "lako");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.SetDifficulty("medium", "srednje");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.SetDifficulty("hard", "teško");
        }
    }

    internal void SetColor(string color, string colorTranslation)
    {
        GameController.instance.playerColor = color;
        GameController.instance.playerColorTranslation = colorTranslation;
        
        if (GameController.instance.gameDifficulty != "")
            startGameButton.interactable = true;
    }

    internal void SetDifficulty(string difficulty, string difficultyTranslation)
    {
        GameController.instance.gameDifficulty = difficulty;
        GameController.instance.gameDifficultyTranslation = difficultyTranslation;
    
        if (GameController.instance.playerColor != "")
            startGameButton.interactable = true;
    }

    internal void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    internal void QuickGame()
    {
        int num = Random.Range(0, 3);
        switch(num)
        {
            case 0:
                SetDifficulty("easy", "lako");
                break;
            case 1:
                SetDifficulty("medium", "srednje");
                break;
            case 2:
                SetDifficulty("hard", "teško");
                break;
        }

        num = Random.Range(0, 4);
        switch (num)
        {
            case 0:
                SetColor("red", "crvena");
                break;
            case 1:
                SetColor("blue", "plava");
                break;
            case 2:
                SetColor("green", "zelena");
                break;
            case 3:
                SetColor("yellow", "žuta");
                break;
        }

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
