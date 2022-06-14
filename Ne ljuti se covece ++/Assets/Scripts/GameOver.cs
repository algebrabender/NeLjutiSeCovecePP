using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text gameOverText = null;

    public Button newGameButton = null;
    public Button exitButton = null;

    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        newGameButton.onClick.RemoveAllListeners();
        newGameButton.onClick.AddListener(NewGame);
        newGameButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(Exit);
        exitButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        gameOverText.text = "<b>" + GameController.instance.winnerText + "!</b>";

        AudioManager.instance.PlayGameoverSound();
    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("goBack", "Escape"))))
        {
            this.Exit();
        }
        else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("newGame", "Space"))))
        {
            this.NewGame();
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            //for editor
            EditorApplication.isPlaying = false;
        #else
            //for build
            Application.Quit();
        #endif
    }
}
