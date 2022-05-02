using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Return))
        {
            this.NewGame();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            this.OptionsMenu();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            this.HelpMenu();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Exit();
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene(4);
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene(5);
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
