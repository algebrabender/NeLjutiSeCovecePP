using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    internal int lastScene = -1;
    internal string playerColor = "";
    internal string gameDifficulty = "";

    internal List<Pawn> controlledPawns = new List<Pawn>(4);
    internal List<Pawn> AIPawns = new List<Pawn>(12);

    internal int lastRolledValue = -1; //TODO: check if needed

    void Awake()
    {
        instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    internal void SetPawns()
    {
        int numOfLives = -1;
        switch (gameDifficulty)
        {
            case "easy":
                numOfLives = 3;
                break;
            case "medium":
                numOfLives = 2;
                break;
            case "hard":
                numOfLives = 1;
                break;
        }
        
        for (int i = 1; i < 5; i++)
        {
            Pawn p = new Pawn();
            p.Number = i;
            p.Eaten = false;
            p.NumOfLivesLeft = numOfLives;
            p.Color = playerColor;
            controlledPawns.Add(p);
        }
    }

    internal void SetAIPawns()
    {
        int numOfLives = -1;
        switch (gameDifficulty)
        {
            case "easy":
                numOfLives = 3;
                break;
            case "medium":
                numOfLives = 2;
                break;
            case "hard":
                numOfLives = 1;
                break;
        }

        List<string> colors = new List<string>();
        switch (playerColor)
        {
            case "red":
                colors.AddRange(new string[] { "blue", "green", "yellow" });
                break;
            case "blue":
                colors.AddRange(new string[] { "red", "green", "yellow" });
                break;
            case "green":
                colors.AddRange(new string[] { "red", "blue", "yellow" });
                break;
            case "yellow":
                colors.AddRange(new string[] { "red", "blue", "green"});
                break;
        }

        foreach (var color in colors)
        {
            for (int i = 1; i < 5; i++)
            {
                Pawn p = new Pawn();
                p.Number = i;
                p.Eaten = false;
                p.NumOfLivesLeft = numOfLives;
                p.Color = color;
                AIPawns.Add(p);
            }
        }
        
    }
}
