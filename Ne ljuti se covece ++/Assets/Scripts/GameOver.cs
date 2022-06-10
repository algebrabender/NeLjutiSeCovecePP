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
        newGameButton.onClick.RemoveAllListeners();
        newGameButton.onClick.AddListener(NewGame);
        newGameButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(Exit);
        exitButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);

        gameOverText.text = "<b>" + GameController.instance.winnerText + "!</b>";    
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
