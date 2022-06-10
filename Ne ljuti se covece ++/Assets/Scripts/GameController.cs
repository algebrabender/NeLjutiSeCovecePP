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
    internal string playerColorTranslation = "";
    internal string gameDifficultyTranslation = "";
    internal string winnerText = "";

    internal List<Pawn> controlledPawns = new List<Pawn>(4);
    internal List<Pawn> AIPawns = new List<Pawn>(12);
    internal int controlledHousesLeft = 4;
    internal int upperLeftHousesLeft = 4;
    internal string upperLeftColor = "";
    internal int upperRightHousesLeft = 4;
    internal string upperRightColor = "";
    internal int lowerRightHousesLeft = 4;
    internal string lowerRightColor = "";

    internal int lastRolledValue = -1;

    public Sprite heartSprite;
    public Sprite cloverSprite;
    public Sprite leafSprite;
    public Sprite diamondSprite;

    void Awake()
    {
        instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

    internal void SetPawns(Vector2 positionOne, Vector2 positionTwo, Vector2 positionThree, Vector2 positionFour)
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

        Sprite sprite = null;
        switch (playerColor)
        {
            case "red":
                sprite = heartSprite;
                break;
            case "blue":
                sprite = cloverSprite;
                break;
            case "green":
                sprite = leafSprite;
                break;
            case "yellow":
                sprite= diamondSprite;
                break;
        }
        
        for (int i = 1; i < 5; i++)
        {
            Pawn p = new Pawn();
            p.typeSprite = sprite;
            p.Number = i;
            p.Out = false;
            p.StartNumOfLives = numOfLives;
            p.NumOfLivesLeft = numOfLives;
            p.Color = playerColor;
            p.Spot = 0;
            p.SpotIfFromPlayer = 0;
            controlledPawns.Add(p);
        }

        controlledPawns[0].StartPosition = positionOne;
        controlledPawns[1].StartPosition = positionTwo;
        controlledPawns[2].StartPosition = positionThree;
        controlledPawns[3].StartPosition = positionFour;
        controlledPawns[0].Position = positionOne;
        controlledPawns[1].Position = positionTwo;
        controlledPawns[2].Position = positionThree;
        controlledPawns[3].Position = positionFour;
    }

    internal void SetAIPawns(Vector2[] positions)
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
                upperLeftColor = "blue";
                upperRightColor = "green";
                lowerRightColor = "yellow";
                break;
            case "blue":
                colors.AddRange(new string[] { "green", "yellow", "red" });
                upperLeftColor = "green";
                upperRightColor = "yellow";
                lowerRightColor = "red";
                break;
            case "green":
                colors.AddRange(new string[] { "yellow", "red", "blue" });
                upperLeftColor = "yellow";
                upperRightColor = "red";
                lowerRightColor = "blue";
                break;
            case "yellow":
                colors.AddRange(new string[] { "red", "blue", "green"});
                upperLeftColor = "red";
                upperRightColor = "blue";
                lowerRightColor = "green";
                break;
        }

        Sprite sprite = null;
        int j = 0;
        foreach (var color in colors)
        {
            for (int i = 1; i < 5; i++)
            {
                switch (color)
                {
                    case "red":
                        sprite = heartSprite;
                        break;
                    case "blue":
                        sprite = cloverSprite;
                        break;
                    case "green":
                        sprite = leafSprite;
                        break;
                    case "yellow":
                        sprite = diamondSprite;
                        break;
                }
                Pawn p = new Pawn();
                p.typeSprite = sprite;
                p.Number = i;
                p.StartNumOfLives = numOfLives;
                p.NumOfLivesLeft = numOfLives;
                p.StartPosition = positions[j + (i - 1)];
                p.Position = positions[j + (i - 1)];
                p.Color = color;
                p.Spot = 0;
                //p.SpotIfFromPlayer = 14 * ((j+4) / 4);
                p.SpotIfFromPlayer = -1;
                AIPawns.Add(p);
            }
            j+=4;
        }
        
    }
}
