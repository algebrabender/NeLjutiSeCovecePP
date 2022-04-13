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

    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        redColorButton.onClick.RemoveAllListeners();
        blueColorButton.onClick.RemoveAllListeners();
        greenColorButton.onClick.RemoveAllListeners();
        yellowColorButton.onClick.RemoveAllListeners();

        redColorButton.onClick.AddListener(delegate { SetColor("red"); });
        blueColorButton.onClick.AddListener(delegate { SetColor("blue"); });
        greenColorButton.onClick.AddListener(delegate { SetColor("green"); });
        yellowColorButton.onClick.AddListener(delegate { SetColor("yellow"); });

        easyDifficultyButton.onClick.RemoveAllListeners();
        mediumDifficultyButton.onClick.RemoveAllListeners();
        hardDifficultyButton.onClick.RemoveAllListeners();

        easyDifficultyButton.onClick.AddListener(delegate { SetDifficulty("easy"); });
        mediumDifficultyButton.onClick.AddListener(delegate { SetDifficulty("medium"); });
        hardDifficultyButton.onClick.AddListener(delegate { SetDifficulty("hard"); });

        startGameButton.interactable = false;
        startGameButton.onClick.RemoveAllListeners();
        startGameButton.onClick.AddListener(StartGame);
    }

    void Update()
    {
        
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
        GameController.instance.playerColor = "";
        GameController.instance.gameDifficulty = "";
        SceneManager.LoadScene(0);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene(4);
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene(5);
    }
}
