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
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionMenu()
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
