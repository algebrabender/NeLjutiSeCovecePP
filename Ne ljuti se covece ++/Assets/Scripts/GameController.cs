using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts;
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
    internal string history = "";

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

    internal void SerializeGameState()
    {
        SaveData saveData = new SaveData();
        for (int i = 0; i < 4; i++)
        {
            saveData.controlledPawnsNumOfLives.Add(controlledPawns[i].NumOfLivesLeft);
            saveData.controlledPawnsSpot.Add(controlledPawns[i].Spot);
            saveData.controlledPawnsPositions.Add(controlledPawns[i].Position);
            saveData.controlledPawnsStartPositions.Add(controlledPawns[i].StartPosition);
            saveData.controlledPawnsOut.Add(controlledPawns[i].Out);
        }

        for (int i = 0; i < 12; i++)
        {
            saveData.AIPawnsNumOfLives.Add(AIPawns[i].NumOfLivesLeft);
            saveData.AIPawnsSpot.Add(AIPawns[i].Spot);
            saveData.AIPawnsSpotIfFromPlayer.Add(AIPawns[i].SpotIfFromPlayer);
            saveData.AIPawnsPositions.Add(AIPawns[i].Position);
            saveData.AIPawnsStartPositions.Add(AIPawns[i].StartPosition);
            saveData.AIPawnsOut.Add(AIPawns[i].Out);
        }
        string jsonData = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("SaveData", jsonData);
        PlayerPrefs.Save();
    }

    internal void DeserializeGameState()
    {
        string jsonData = PlayerPrefs.GetString("SaveData");
        SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonData);

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
                sprite = diamondSprite;
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            controlledPawns[i].Spot = loadedData.controlledPawnsSpot[i];
            controlledPawns[i].SpotIfFromPlayer = -1;
            controlledPawns[i].NumOfLivesLeft = loadedData.controlledPawnsNumOfLives[i];
            controlledPawns[i].Position = loadedData.controlledPawnsPositions[i];
            controlledPawns[i].StartPosition = loadedData.controlledPawnsStartPositions[i];
            controlledPawns[i].Number = i + 1;
            controlledPawns[i].Out = loadedData.controlledPawnsOut[i];
            controlledPawns[i].Color = playerColor;
            controlledPawns[i].typeSprite = sprite;
            controlledPawns[i].StartNumOfLives = numOfLives;
        }

        for (int i = 0; i < 12; i++)
        {
            AIPawns[i].Spot = loadedData.AIPawnsSpot[i];
            AIPawns[i].SpotIfFromPlayer = loadedData.AIPawnsSpotIfFromPlayer[i];
            AIPawns[i].NumOfLivesLeft = loadedData.AIPawnsNumOfLives[i];
            AIPawns[i].Position = loadedData.AIPawnsPositions[i];
            AIPawns[i].StartPosition = loadedData.AIPawnsStartPositions[i];
            AIPawns[i].Number = i % 4 + 1;
            AIPawns[i].Out = loadedData.AIPawnsOut[i];
            AIPawns[i].StartNumOfLives = numOfLives;

            if (i < 4)
            {
                switch (upperLeftColor)
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
                AIPawns[i].Color = upperLeftColor;
                AIPawns[i].typeSprite = sprite;
            }
            else if (i < 8)
            {
                switch (upperRightColor)
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
                AIPawns[i].Color = upperRightColor;
                AIPawns[i].typeSprite = sprite;
            }
            else
            {
                switch (lowerRightColor)
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
                AIPawns[i].Color = lowerRightColor;
                AIPawns[i].typeSprite = sprite;
            }
        }
    }
}
