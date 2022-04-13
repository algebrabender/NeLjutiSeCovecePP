using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        Debug.Log(GameController.instance.playerColor);
        Debug.Log(GameController.instance.gameDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBack()
    {
        GameController.instance.playerColor = "";
        GameController.instance.gameDifficulty = "";
        SceneManager.LoadScene(0);
    }
}
