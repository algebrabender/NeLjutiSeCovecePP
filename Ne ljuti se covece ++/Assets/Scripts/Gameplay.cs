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
    #region UI Elements
    public Button rollDiceButton;
    public Button pawnOneButton;
    public Button pawnTwoButton;
    public Button pawnThreeButton;
    public Button pawnFourButton;

    public Text historyText;
    public Text rolledNumberText;

    public Image boardImage;
    public Sprite redBoardSprite;
    public Sprite blueBoardSprite;
    public Sprite greenBoardSprite;
    public Sprite yellowBoardSprite;

    public GameObject pawnOne;
    public GameObject pawnTwo;
    public GameObject pawnThree;
    public GameObject pawnFour;

    public List<GameObject> AIPawns;
    #endregion

    private int lastRolledValue = -1;
    private bool allowPlay = false;

    #region Private Methodes
    private void SetColors()
    {
        ColorBlock colorBlock = pawnOneButton.colors;

        switch (GameController.instance.playerColor)
        {
            case "red":
                colorBlock.normalColor = new Color(241f / 255f, 87f / 255f, 87f / 255f);
                boardImage.sprite = redBoardSprite;               
                break;
            case "blue":
                colorBlock.normalColor = new Color(93f / 255f, 102f / 255f, 255f / 255f);
                boardImage.sprite = blueBoardSprite;
                break;
            case "green":
                colorBlock.normalColor = new Color(53f / 255f, 204f / 255f, 97f / 255f);
                boardImage.sprite = greenBoardSprite;
                break;
            case "yellow":
                colorBlock.normalColor = new Color(255f / 255f, 238f / 255f, 78f / 255f);
                boardImage.sprite = yellowBoardSprite;
                break;
        }

        pawnOneButton.colors = colorBlock;
        pawnTwoButton.colors = colorBlock;
        pawnThreeButton.colors = colorBlock;
        pawnFourButton.colors = colorBlock;
    }

    private void SetButtonListeners()
    {
        rollDiceButton.onClick.RemoveAllListeners();

        rollDiceButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        rollDiceButton.onClick.AddListener(AudioManager.instance.PlayDiceRollSound);
        rollDiceButton.onClick.AddListener(RollDice);

        pawnOneButton.onClick.RemoveAllListeners();
        pawnTwoButton.onClick.RemoveAllListeners();
        pawnThreeButton.onClick.RemoveAllListeners();
        pawnFourButton.onClick.RemoveAllListeners();

        pawnOneButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        pawnTwoButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        pawnThreeButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        pawnFourButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
    }

    private int GetNumber()
    {
        int rolledNumber = Random.Range(1, 7);

        return rolledNumber;
    }

    private void DisableButtons()
    {
        //rollDiceButton.enabled = false;
        
        pawnOneButton.enabled = false;
        pawnTwoButton.enabled = false;
        pawnThreeButton.enabled = false;
        pawnFourButton.enabled = false;
    }

    private IEnumerator AITurn()
    {
        rollDiceButton.enabled = false;

        yield return new WaitForSeconds(1.0f);

        allowPlay = false;

        int rolledNumber = Random.Range(1, 7);
        int pawnNumber = Random.Range(0, 4);
        int outedPawn = -1;

        for (int i = 0; i < 3; i++)
        {
            if (!GameController.instance.AIPawns[pawnNumber + (i * 4)].Out && rolledNumber == 6)
            {
                float newX;
                float newY;
                if (i == 0) //upper left
                {
                    newX = 100.38f;
                    newY = 366.72f;
                }
                else if (i == 1) //upper right
                {
                    newX = 384.25f;
                    newY = 569.27f;
                }
                else //lower right
                {
                    newX = 587.21f;
                    newY = 285.94f;
                }

                bool thereIsOuted = false;
                for (int j = i * 4; j < i * 4 + 4; j++)
                {
                    if (j == pawnNumber + (i * 4))
                        continue;
                    if ((float)System.Math.Round(GameController.instance.AIPawns[j].Position.x, 2) == newX
                        && (float)System.Math.Round(GameController.instance.AIPawns[j].Position.y, 2) == newY)
                    {
                        thereIsOuted = true;
                        outedPawn = j % 4;
                        break;
                    }
                }

                if (!thereIsOuted)
                {
                    AIPawns[pawnNumber + (i * 4)].transform.position = new Vector3(newX, newY, AIPawns[pawnNumber + (i * 4)].transform.position.z);
                    GameController.instance.AIPawns[pawnNumber + (i * 4)].OutedPawn(newX, newY);

                    if (pawnNumber + (i * 4) < 4)
                        GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer = 14;
                    else if (pawnNumber + (i * 4) < 8)
                        GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer = 28;
                    else
                        GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer = 42;

                    this.CheckIfAIEating(GameController.instance.AIPawns[pawnNumber + (i * 4)], GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer, true);

                    historyText.text += "Kockica je pala na: " + rolledNumber + "\n";
                    historyText.text += "Pawn number: " + pawnNumber + " i: " + i + " index: " + (pawnNumber + (i * 4)) + "\n";
                    historyText.text += "----------------------------------------\n";
                    GameController.instance.history = historyText.text;

                    pawnNumber = Random.Range(0, 4);
                    rolledNumber = Random.Range(1, 7);
                }
                else
                {
                    i--;
                    //pawnNumber = Random.Range(0, 4);
                    pawnNumber = outedPawn;
                    //rolledNumber = Random.Range(1, 7);
                    continue;
                }

                yield return new WaitForSeconds(1.5f);
            }
            else if (GameController.instance.AIPawns[pawnNumber + (i * 4)].Out && GameController.instance.AIPawns[pawnNumber + (i * 4)].Spot != 59)
            {
                AudioManager.instance.PlayDiceRollSound();

                float deltaX, deltaY;
                int newSpot;
                this.CalculateDeltas(GameController.instance.AIPawns[pawnNumber + (i * 4)].Spot, rolledNumber, out deltaX, out deltaY, out newSpot, i);

                if (newSpot != 59)
                {
                    bool sameSpot = false;
                    for (int j = i * 4; j < i * 4 + 4; j++)
                    {
                        if (pawnNumber + i * 4 != j)
                            if (GameController.instance.AIPawns[j].Spot == newSpot)
                            {
                                sameSpot = true;
                                break;
                            }
                    }

                    if (sameSpot)
                    {
                        i--;
                        pawnNumber = Random.Range(0, 4);
                        rolledNumber = Random.Range(1, 7);
                        continue;
                    }

                    if (newSpot < 55)
                    {
                        if (GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer + rolledNumber < 56)
                        {
                            if (this.CheckIfAIEating(GameController.instance.AIPawns[pawnNumber + (i * 4)], GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer + rolledNumber))
                            {
                                this.CalculateDeltas(GameController.instance.AIPawns[pawnNumber + (i * 4)].Spot, rolledNumber - 1, out deltaX, out deltaY, out newSpot, i);
                                rolledNumber--;
                            }
                        }
                        else
                        {
                            if (this.CheckIfAIEating(GameController.instance.AIPawns[pawnNumber + (i * 4)], GameController.instance.AIPawns[pawnNumber + (i * 4)].SpotIfFromPlayer + rolledNumber - 56))
                            {
                                this.CalculateDeltas(GameController.instance.AIPawns[pawnNumber + (i * 4)].Spot, rolledNumber - 1, out deltaX, out deltaY, out newSpot, i);
                                rolledNumber--;
                            }
                        }
                    }
                }
                else
                {
                    AudioManager.instance.PlayInHouseSound();  
                }

                float temp;
                if (i == 0)
                {
                    temp = -deltaX;
                    deltaX = deltaY;
                    deltaY = temp;
                }
                else if (i == 1)
                {
                    deltaX *= -1;
                    deltaY *= -1;
                }
                else
                {
                    temp = deltaX;
                    deltaX = -deltaY;
                    deltaY = temp;
                }

                if (deltaX == 0 && deltaY == 0)
                    rolledNumber = 0;

                AIPawns[pawnNumber + (i * 4)].transform.position = new Vector3(AIPawns[pawnNumber + (i * 4)].transform.position.x + deltaX, AIPawns[pawnNumber + (i * 4)].transform.position.y + deltaY, AIPawns[pawnNumber + (i * 4)].transform.position.z);
                GameController.instance.AIPawns[pawnNumber + (i * 4)].UpdatePosition(deltaX, deltaY, newSpot, rolledNumber);

                historyText.text += "Kockica je pala na: " + rolledNumber + "\n";
                historyText.text += "Pawn number: " + pawnNumber + " i: " + i + " index: " + (pawnNumber + (i * 4)) + "\n";
                historyText.text += "----------------------------------------\n";
                GameController.instance.history = historyText.text;

                pawnNumber = Random.Range(0, 4);
                rolledNumber = Random.Range(1, 7);

                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                //historyText.text += "Turn skipped\n";
                //historyText.text += "----------------------------------------\n";
                i--;
                int prevPawnNumber = pawnNumber;
                pawnNumber = Random.Range(0, 4);
                while (pawnNumber == prevPawnNumber)
                    pawnNumber = Random.Range(0, 4);
                rolledNumber = Random.Range(1, 7);
            }
        }

        rollDiceButton.enabled = true;
    }

    private void SetPawnNumbers()
    {
        pawnOne.GetComponentInChildren<Image>().sprite = GameController.instance.controlledPawns[0].typeSprite;
        pawnOne.GetComponentInChildren<Text>().text = GameController.instance.controlledPawns[0].Number.ToString();

        pawnTwo.GetComponentInChildren<Image>().sprite = GameController.instance.controlledPawns[1].typeSprite;
        pawnTwo.GetComponentInChildren<Text>().text = GameController.instance.controlledPawns[1].Number.ToString();

        pawnThree.GetComponentInChildren<Image>().sprite = GameController.instance.controlledPawns[2].typeSprite;
        pawnThree.GetComponentInChildren<Text>().text = GameController.instance.controlledPawns[2].Number.ToString();

        pawnFour.GetComponentInChildren<Image>().sprite = GameController.instance.controlledPawns[3].typeSprite;
        pawnFour.GetComponentInChildren<Text>().text = GameController.instance.controlledPawns[3].Number.ToString();

        for (int i = 0; i < 12; i++)
        {
            AIPawns[i].GetComponentInChildren<Image>().sprite = GameController.instance.AIPawns[i].typeSprite;
            AIPawns[i].GetComponentInChildren<Text>().text = GameController.instance.AIPawns[i].Number.ToString();
        }    
    }
    
    private void CalculateDeltas(int currentSpot, int rolledValue, out float deltaX, out float deltaY, out int newSpot, int ai = 3)
    {
        deltaX = 40.27f;
        deltaY = 40.63f;

        newSpot = currentSpot + rolledValue;
        
        if (newSpot <= 5)
        {
            deltaX = 0;
            deltaY *= rolledValue;
            return;
        }

        if (newSpot <= 11)
        {
            if (currentSpot >= 5)
            {
                deltaX *= -rolledValue;
                deltaY = 0;
            }
            else
            {
                deltaX *= -(newSpot - 5);
                deltaY *= (5 - currentSpot);
            }
            return;
        }

        if (newSpot <= 13)
        {
            if (currentSpot >= 11)
            {
                deltaX = 0;
                deltaY *= rolledValue;
            }
            else
            {
                deltaX *= -(11 - currentSpot);
                deltaY *= (newSpot - 11);
            }
            return;
        }

        if (newSpot <= 19)
        {
            if (currentSpot >= 13)
            {
                deltaX *= rolledValue;
                deltaY = 0;
            }
            else if (currentSpot >= 11)
            {
                deltaX *= (newSpot - 13);
                deltaY *= (13 - currentSpot);
            }
            else
            {
                deltaX *= ((newSpot - 13) - (11 - currentSpot));
                deltaY *= 2;
            }
            return;
        }

        if (newSpot <= 25)
        {
            if (currentSpot >= 19)
            {
                deltaX = 0;
                deltaY *= rolledValue;
            }
            else
            {
                deltaX *= (19 - currentSpot);
                deltaY *= (newSpot - 19);
            }
            return;
        }

        if (newSpot <= 27)
        {
            if (currentSpot >= 25)
            {
                deltaX *= rolledValue;
                deltaY = 0;
            }
            else
            {
                deltaX *= (newSpot - 25); 
                deltaY *= (25 - currentSpot);
            }
            return;
        }

        if (newSpot <= 33)
        {
            if (currentSpot >= 27)
            {
                deltaX = 0;
                deltaY *= -rolledValue;
            }
            else if (currentSpot >= 25)
            {
                deltaX *= (27 - currentSpot);
                deltaY *= -(newSpot - 27);
            }
            else
            {
                deltaX *= 2;
                deltaY *= (-(newSpot - 27) + (25 - currentSpot));
            }
            return;
        }
        
        if (newSpot <= 39)
        {
            if (currentSpot >= 33)
            {
                deltaX *= rolledValue;
                deltaY = 0;
            }
            else
            {
                deltaX *= (newSpot - 33);
                deltaY *= -(33 - currentSpot);
            }
            return;
        }

        if (newSpot <= 41)
        {
            if (currentSpot >= 39)
            {
                deltaX = 0;
                deltaY *= -rolledValue;
            }    
            else
            {
                deltaX *= (39 - currentSpot);
                deltaY *= -(newSpot - 39);
            }
            return;
        }

        if (newSpot <= 47)
        {
            if (currentSpot >= 41)
            { 
                deltaX *= -rolledValue;
                deltaY = 0;
            }
            else if (currentSpot >= 39)
            {
                deltaX *= -(newSpot - 41);
                deltaY *= -(41 - currentSpot);
            }
            else
            {
                deltaX *= (-(newSpot - 41) + (39 - currentSpot));
                deltaY *= -2;
            }
            return;
        }

        if (newSpot <= 53)
        {
            if (currentSpot >= 47)
            {
                deltaX = 0;
                deltaY *= -rolledValue;
            }
            else
            {
                deltaX *= -(47 - currentSpot);
                deltaY *= -(newSpot - 47);
            }
            return;
        }

        if (newSpot == 54)
        {
            deltaX *= -1;
            if (currentSpot == 53)
                deltaY = 0;
            else
                deltaY *= -(53 - currentSpot);
        }

        if (newSpot >= 59)
        {
            newSpot = currentSpot;
            deltaX = 0;
            deltaY = 0;
        }

        if (newSpot >= 55 && newSpot < 59)
        {
            bool allowed = true;
            switch(ai)
            {
                case 3: //controlled pawn
                    if (GameController.instance.controlledHousesLeft == newSpot - 54)
                        GameController.instance.controlledHousesLeft--;
                    else
                        allowed = false;
                    break;
                case 2:
                    if (GameController.instance.lowerRightHousesLeft == newSpot - 54)
                        GameController.instance.lowerRightHousesLeft--;
                    else
                        allowed = false;
                    break;
                case 1:
                    if (GameController.instance.upperRightHousesLeft == newSpot - 54)
                        GameController.instance.upperRightHousesLeft--;
                    else
                        allowed = false;
                    break;
                case 0:
                    if (GameController.instance.upperLeftHousesLeft == newSpot - 54)
                         GameController.instance.upperLeftHousesLeft--;
                    else
                        allowed = false;
                    break;
            }

            if (allowed)
            {
                if (currentSpot == 54)
                {
                    deltaX = 0;
                    deltaY *= newSpot - 54;
                }
                else
                {
                    deltaX *= -1;
                    deltaY *= (-(53 - currentSpot) + (newSpot - 54));
                }
                newSpot = 59;
            }
            else
            {
                newSpot = currentSpot;
                deltaX = 0;
                deltaY = 0;
            }
        }
    }
    
    private bool CheckIfEating(int newSpot, bool outing = false)
    {
        if (newSpot >= 55)
            return false;

        foreach (Pawn p in GameController.instance.AIPawns)
        {
            if (p.SpotIfFromPlayer == newSpot)
            {
                AudioManager.instance.PlayEatingSound();

                if (!outing)
                {
                    if (p.UpdateLives()) //eaten
                    {
                        int index = GameController.instance.AIPawns.IndexOf(p);
                        AIPawns[index].transform.position = new Vector3(p.Position.x, p.Position.y, 0.0f);
                        //if (index / 4 == 0)
                        //    p.SpotIfFromPlayer = 14;
                        //else if (index / 4 == 1)
                        //    p.SpotIfFromPlayer = 28;
                        //else
                        //    p.SpotIfFromPlayer = 42;
                        p.SpotIfFromPlayer = -1;
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    while (p.NumOfLivesLeft > 0)
                        if (p.UpdateLives())
                            break;
                    int index = GameController.instance.AIPawns.IndexOf(p);
                    AIPawns[index].transform.position = new Vector3(p.Position.x, p.Position.y, 0.0f);
                    //if (index / 4 == 0)
                    //    p.SpotIfFromPlayer = 14;
                    //else if (index / 4 == 1)
                    //    p.SpotIfFromPlayer = 28;
                    //else
                    //    p.SpotIfFromPlayer = 42;
                    p.SpotIfFromPlayer = -1;
                    return false;
                }
            }
        }
        return false;
    }
    
    private bool CheckIfAIEating(Pawn currentPawn, int newSpot, bool outing = false)
    {
        //if (newSpot >= 55)
        //    return false;

        List<Pawn> pawnsToCheck= new List<Pawn>();
        pawnsToCheck.AddRange(GameController.instance.AIPawns);

        int index = pawnsToCheck.IndexOf(currentPawn);

        if (index / 4 == 0)
        {
            pawnsToCheck.RemoveAt(3);
            pawnsToCheck.RemoveAt(2);
            pawnsToCheck.RemoveAt(1);
            pawnsToCheck.RemoveAt(0);
        }
        else if (index / 4 == 1)
        {
            pawnsToCheck.RemoveAt(7);
            pawnsToCheck.RemoveAt(6);
            pawnsToCheck.RemoveAt(5);
            pawnsToCheck.RemoveAt(4);
        }
        else if (index / 4 == 2)
        {
            pawnsToCheck.RemoveAt(11);
            pawnsToCheck.RemoveAt(10);
            pawnsToCheck.RemoveAt(9);
            pawnsToCheck.RemoveAt(8);
        }

        foreach (Pawn p in pawnsToCheck)
        {
            if (p.SpotIfFromPlayer == newSpot && p.Spot != 59)
            {
                AudioManager.instance.PlayEatingSound();

                if (!outing)
                {
                    if (p.UpdateLives()) //eaten
                    {
                        index = GameController.instance.AIPawns.IndexOf(p);
                        AIPawns[index].transform.position = new Vector3(p.Position.x, p.Position.y, 0.0f);

                        //if (index / 4 == 0)
                        //    p.SpotIfFromPlayer = 14;
                        //else if (index / 4 == 1)
                        //    p.SpotIfFromPlayer = 28;
                        //else
                        //    p.SpotIfFromPlayer = 42;
                        p.SpotIfFromPlayer = -1;
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    while (p.NumOfLivesLeft > 0)
                        if (p.UpdateLives())
                            break;
                    index = GameController.instance.AIPawns.IndexOf(p);
                    AIPawns[index].transform.position = new Vector3(p.Position.x, p.Position.y, 0.0f);

                    //if (index / 4 == 0)
                    //    p.SpotIfFromPlayer = 14;
                    //else if (index / 4 == 1)
                    //    p.SpotIfFromPlayer = 28;
                    //else
                    //    p.SpotIfFromPlayer = 42;
                    p.SpotIfFromPlayer = -1;
                    return false;
                }
            }
        }
        
        if (GameController.instance.controlledPawns[0].Spot == newSpot)
        {
            AudioManager.instance.PlayEatingSound();

            if (!outing)
            {
                if (GameController.instance.controlledPawns[0].UpdateLives())
                {
                    pawnOne.transform.position = new Vector3(GameController.instance.controlledPawns[0].Position.x, GameController.instance.controlledPawns[0].Position.y, 0.0f);
                    return false;
                }
                else
                    return true;
            }
            else
            {
                while (GameController.instance.controlledPawns[0].NumOfLivesLeft > 0)
                    if (GameController.instance.controlledPawns[0].UpdateLives())
                        break;
                pawnOne.transform.position = new Vector3(GameController.instance.controlledPawns[0].Position.x, GameController.instance.controlledPawns[0].Position.y, 0.0f);
                return false;
            }
        }

        if (GameController.instance.controlledPawns[1].Spot == newSpot)
        {
            AudioManager.instance.PlayEatingSound();

            if (!outing)
            {
                if (GameController.instance.controlledPawns[1].UpdateLives())
                {
                    pawnTwo.transform.position = new Vector3(GameController.instance.controlledPawns[1].Position.x, GameController.instance.controlledPawns[1].Position.y, 0.0f);
                    return false;
                }
                else
                    return true;
            }
            else
            {
                while (GameController.instance.controlledPawns[1].NumOfLivesLeft > 0)
                    if (GameController.instance.controlledPawns[1].UpdateLives())
                        break;
                pawnTwo.transform.position = new Vector3(GameController.instance.controlledPawns[1].Position.x, GameController.instance.controlledPawns[1].Position.y, 0.0f);
                return false;
            }
        }

        if (GameController.instance.controlledPawns[2].Spot == newSpot)
        {
            AudioManager.instance.PlayEatingSound();

            if (!outing)
            {
                if (GameController.instance.controlledPawns[2].UpdateLives())
                {
                    pawnThree.transform.position = new Vector3(GameController.instance.controlledPawns[2].Position.x, GameController.instance.controlledPawns[2].Position.y, 0.0f);
                    return false;
                }
                else
                    return true;
            }
            else
            {
                while (GameController.instance.controlledPawns[2].NumOfLivesLeft > 0)
                    if (GameController.instance.controlledPawns[2].UpdateLives())
                        break;
                pawnThree.transform.position = new Vector3(GameController.instance.controlledPawns[2].Position.x, GameController.instance.controlledPawns[2].Position.y, 0.0f);
                return false;
            }
        }

        if (GameController.instance.controlledPawns[3].Spot == newSpot)
        {
            AudioManager.instance.PlayEatingSound();

            if (!outing)
            {
                if (GameController.instance.controlledPawns[3].UpdateLives())
                {
                    pawnFour.transform.position = new Vector3(GameController.instance.controlledPawns[3].Position.x, GameController.instance.controlledPawns[3].Position.y, 0.0f);
                    return false;
                }
                else
                    return true;
            }
            else
            {
                while (GameController.instance.controlledPawns[3].NumOfLivesLeft > 0)
                    if (GameController.instance.controlledPawns[3].UpdateLives())
                        break;
                pawnFour.transform.position = new Vector3(GameController.instance.controlledPawns[3].Position.x, GameController.instance.controlledPawns[3].Position.y, 0.0f);
                return false;
            }
        }

        return false;
    }
    
    private string GameOverTextTranslation(string color)
    {
        switch (color)
        {
            case "red":
                return "crveni";
            case "blue":
                return "plavi";
            case "green":
                return "zeleni";
            case "yellow":
                return "�uti";
            default:
                return "";
        }
    }
    #endregion

    #region Methodes
    void Start()
    {
        if (GameController.instance.lastScene != 1)
        {
            historyText.text = GameController.instance.history;

            GameController.instance.DeserializeGameState();

            pawnOne.transform.position = GameController.instance.controlledPawns[0].Position;
            pawnTwo.transform.position = GameController.instance.controlledPawns[1].Position;
            pawnThree.transform.position = GameController.instance.controlledPawns[2].Position;
            pawnFour.transform.position = GameController.instance.controlledPawns[3].Position;

            for (int i = 0; i < 12; i++)
            {
                AIPawns[i].transform.position = GameController.instance.AIPawns[i].Position;
            }
        }
        else
        {
            historyText.text = "Izabrana boja: " + GameController.instance.playerColorTranslation +
                                "\nIzabrana te�ina: " + GameController.instance.gameDifficultyTranslation + "\n";
            GameController.instance.history = historyText.text;

            

            GameController.instance.SetPawns((Vector2)pawnOne.transform.position, (Vector2)pawnTwo.transform.position,
                                             (Vector2)pawnThree.transform.position, (Vector2)pawnFour.transform.position);

            Vector2[] positions = new Vector2[12];

            for (int i = 0; i < 4; i++)
            {
                positions[i] = (Vector2)AIPawns[i].transform.position;
                positions[i + 4] = (Vector2)AIPawns[i + 4].transform.position;
                positions[i + 8] = (Vector2)AIPawns[i + 8].transform.position;
            }

            GameController.instance.SetAIPawns(positions);
        }

        this.SetButtonListeners();

        this.SetColors();

        this.SetPawnNumbers();

        pawnOneButton.enabled = false;
        pawnTwoButton.enabled = false;
        pawnThreeButton.enabled = false;
        pawnFourButton.enabled = false;

        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

    }

    void Update()
    {
        if (GameController.instance.controlledHousesLeft == 0)
        {
            GameController.instance.winnerText = "POBEDA";
            SceneManager.LoadScene(3);
        }
            
        if (GameController.instance.upperLeftHousesLeft == 0)
        {
            string color = GameOverTextTranslation(GameController.instance.upperLeftColor);
            GameController.instance.winnerText = "POBEDIO JE " + color.ToUpper();
            SceneManager.LoadScene(3);
        }

        if (GameController.instance.upperRightHousesLeft == 0)
        {
            string color = GameOverTextTranslation(GameController.instance.upperRightColor);
            GameController.instance.winnerText = "POBEDIO JE " + color.ToUpper();
            SceneManager.LoadScene(3);
        }

        if (GameController.instance.lowerRightHousesLeft == 0)
        {
            string color = GameOverTextTranslation(GameController.instance.lowerRightColor);
            GameController.instance.winnerText = "POBEDIO JE " + color.ToUpper();
            SceneManager.LoadScene(3);
        }

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
        else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rollDice", "D"))))
        {
            this.RollDice();
        }
        if (allowPlay)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnOne", "Alpha1"))))
            {
                this.PawnOne();
            }
            else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnTwo", "Alpha2"))))
            {
                this.PawnTwo();
            }
            else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnThree", "Alpha3"))))
            {
                this.PawnThree();
            }
            else if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnFour", "Alpha4"))))
            {
                this.PawnFour();
            }
        }
    }

    public void RollDice()
    {
        AudioManager.instance.PlayDiceRollSound();

        int value = this.GetNumber();

        rolledNumberText.text = "Kockica je pala na: " + value + "\n";

        lastRolledValue = value;

        List<Button> buttons = new List<Button>();
        buttons.AddRange(new Button[] { pawnOneButton, pawnTwoButton, pawnThreeButton, pawnFourButton });

        for (int i = 0; i < buttons.Count; i++)
        {
            if (GameController.instance.controlledPawns[i].Out)
                buttons[i].enabled = true;
            else if (lastRolledValue != 6)
                buttons[i].enabled = false;
        }

        if (lastRolledValue == 6)
        {
            pawnOneButton.enabled = true;
            pawnTwoButton.enabled = true;
            pawnThreeButton.enabled = true;
            pawnFourButton.enabled = true;

            if (GameController.instance.controlledPawns[0].Out && GameController.instance.controlledPawns[0].Spot == 0)
            {
                if (!GameController.instance.controlledPawns[1].Out)
                    pawnTwoButton.enabled = false;
                if (!GameController.instance.controlledPawns[2].Out)
                    pawnThreeButton.enabled = false;
                if (!GameController.instance.controlledPawns[3].Out)
                    pawnFourButton.enabled = false;

                pawnOneButton.enabled = true;
            }

            if (GameController.instance.controlledPawns[1].Out && GameController.instance.controlledPawns[1].Spot == 0)
            {
                if (!GameController.instance.controlledPawns[0].Out)
                    pawnOneButton.enabled = false;
                if (!GameController.instance.controlledPawns[2].Out)
                    pawnThreeButton.enabled = false;
                if (!GameController.instance.controlledPawns[3].Out)
                    pawnFourButton.enabled = false;

                pawnTwoButton.enabled = true;
            }

            if (GameController.instance.controlledPawns[2].Out && GameController.instance.controlledPawns[2].Spot == 0)
            {
                if (!GameController.instance.controlledPawns[0].Out)
                    pawnOneButton.enabled = false;
                if (!GameController.instance.controlledPawns[1].Out)
                    pawnTwoButton.enabled = false;
                if (!GameController.instance.controlledPawns[3].Out)
                    pawnFourButton.enabled = false;

                pawnThreeButton.enabled = true;
            }

            if (GameController.instance.controlledPawns[3].Out && GameController.instance.controlledPawns[3].Spot == 0)
            {
                if (!GameController.instance.controlledPawns[0].Out)
                    pawnOneButton.enabled = false;
                if (!GameController.instance.controlledPawns[1].Out)
                    pawnTwoButton.enabled = false;
                if (!GameController.instance.controlledPawns[2].Out)
                    pawnThreeButton.enabled = false;

                pawnFourButton.enabled = true;
            }
        }

        if (GameController.instance.controlledPawns[0].Out &&
           (GameController.instance.controlledPawns[0].Spot + lastRolledValue == GameController.instance.controlledPawns[1].Spot ||
            GameController.instance.controlledPawns[0].Spot + lastRolledValue == GameController.instance.controlledPawns[2].Spot ||
            GameController.instance.controlledPawns[0].Spot + lastRolledValue == GameController.instance.controlledPawns[3].Spot))
            pawnOneButton.enabled = false;

        if (GameController.instance.controlledPawns[1].Out &&
           (GameController.instance.controlledPawns[1].Spot + lastRolledValue == GameController.instance.controlledPawns[0].Spot ||
            GameController.instance.controlledPawns[1].Spot + lastRolledValue == GameController.instance.controlledPawns[2].Spot ||
            GameController.instance.controlledPawns[1].Spot + lastRolledValue == GameController.instance.controlledPawns[3].Spot))
            pawnTwoButton.enabled = false;

        if (GameController.instance.controlledPawns[2].Out &&
           (GameController.instance.controlledPawns[2].Spot + lastRolledValue == GameController.instance.controlledPawns[0].Spot ||
            GameController.instance.controlledPawns[2].Spot + lastRolledValue == GameController.instance.controlledPawns[1].Spot ||
            GameController.instance.controlledPawns[2].Spot + lastRolledValue == GameController.instance.controlledPawns[3].Spot))
            pawnThreeButton.enabled = false;

        if (GameController.instance.controlledPawns[3].Out &&
           (GameController.instance.controlledPawns[3].Spot + lastRolledValue == GameController.instance.controlledPawns[0].Spot ||
            GameController.instance.controlledPawns[3].Spot + lastRolledValue == GameController.instance.controlledPawns[1].Spot ||
            GameController.instance.controlledPawns[3].Spot + lastRolledValue == GameController.instance.controlledPawns[2].Spot))
            pawnFourButton.enabled = false;

        for (int i = 0; i < 4; i++)
        {
            if (GameController.instance.controlledPawns[i].Spot == 59)
                buttons[i].enabled = false;
        }

        foreach (Button b in buttons)
        {
            if (b.enabled)
            {
                allowPlay = true;
                return;
            }
        }

        ////no avaiable move
        allowPlay = false;
        StartCoroutine(this.AITurn());
    }

    public void PawnOne()
    {
        if (!GameController.instance.controlledPawns[0].Out && lastRolledValue == 6)
        {
            pawnOne.transform.position = new Vector3(pawnOne.transform.position.x + 102.6f, pawnOne.transform.position.y - 19.54f, pawnOne.transform.position.z);
            GameController.instance.controlledPawns[0].OutedPawn(pawnOne.transform.position.x, pawnOne.transform.position.y);

            this.CheckIfEating(0, true);

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
        else if (GameController.instance.controlledPawns[0].Out)
        {
            float deltaX, deltaY;
            int newSpot;

            this.CalculateDeltas(GameController.instance.controlledPawns[0].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);
            if (this.CheckIfEating(newSpot))
                this.CalculateDeltas(GameController.instance.controlledPawns[0].Spot, lastRolledValue - 1, out deltaX, out deltaY, out newSpot);

            pawnOne.transform.position = new Vector3(pawnOne.transform.position.x + deltaX, pawnOne.transform.position.y + deltaY, pawnOne.transform.position.z);
            GameController.instance.controlledPawns[0].UpdatePosition(deltaX, deltaY, newSpot);

            if (newSpot > 55)
            {
                GameController.instance.controlledPawns[0].Spot = 59; //in house so there is no "eating"
                AudioManager.instance.PlayInHouseSound();
            }
            
            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnTwo()
    {
        if (!GameController.instance.controlledPawns[1].Out && lastRolledValue == 6)
        {
            pawnTwo.transform.position = new Vector3(pawnTwo.transform.position.x + 174.73f, pawnTwo.transform.position.y - 19.54f, pawnTwo.transform.position.z);
            GameController.instance.controlledPawns[1].OutedPawn(pawnTwo.transform.position.x, pawnTwo.transform.position.y);

            this.CheckIfEating(0, true);

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
        else if (GameController.instance.controlledPawns[1].Out)
        {
            float deltaX, deltaY;
            int newSpot;
            
            this.CalculateDeltas(GameController.instance.controlledPawns[1].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);
            if (this.CheckIfEating(newSpot))
                this.CalculateDeltas(GameController.instance.controlledPawns[1].Spot, lastRolledValue - 1, out deltaX, out deltaY, out newSpot);

            pawnTwo.transform.position = new Vector3(pawnTwo.transform.position.x + deltaX, pawnTwo.transform.position.y + deltaY, pawnTwo.transform.position.z);
            GameController.instance.controlledPawns[1].UpdatePosition(deltaX, deltaY, newSpot);

            if (newSpot > 55)
            {
                GameController.instance.controlledPawns[1].Spot = 59; //in house so there is no "eating"
                AudioManager.instance.PlayInHouseSound();
            }

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnThree()
    {
        if (!GameController.instance.controlledPawns[2].Out && lastRolledValue == 6)
        {
            pawnThree.transform.position = new Vector3(pawnThree.transform.position.x + 174.73f, pawnThree.transform.position.y - 95.87f, pawnThree.transform.position.z);
            GameController.instance.controlledPawns[2].OutedPawn(pawnThree.transform.position.x, pawnThree.transform.position.y);

            this.CheckIfEating(0, true);

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
        else if (GameController.instance.controlledPawns[2].Out)
        {
            float deltaX, deltaY;
            int newSpot;
            
            this.CalculateDeltas(GameController.instance.controlledPawns[2].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);
            if (this.CheckIfEating(newSpot))
                this.CalculateDeltas(GameController.instance.controlledPawns[2].Spot, lastRolledValue - 1, out deltaX, out deltaY, out newSpot);

            pawnThree.transform.position = new Vector3(pawnThree.transform.position.x + deltaX, pawnThree.transform.position.y + deltaY, pawnThree.transform.position.z);
            GameController.instance.controlledPawns[2].UpdatePosition(deltaX, deltaY, newSpot);

            if (newSpot > 55)
            {
                GameController.instance.controlledPawns[2].Spot = 59; //in house so there is no "eating"
                AudioManager.instance.PlayInHouseSound();
            }

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnFour()
    {
        if (!GameController.instance.controlledPawns[3].Out && lastRolledValue == 6)
        {
            pawnFour.transform.position = new Vector3(pawnFour.transform.position.x + 102.6f, pawnFour.transform.position.y - 95.87f, pawnFour.transform.position.z);
            GameController.instance.controlledPawns[3].OutedPawn(pawnFour.transform.position.x, pawnFour.transform.position.y);

            this.CheckIfEating(0, true);

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
        else if (GameController.instance.controlledPawns[3].Out)
        {
            float deltaX, deltaY;
            int newSpot;

            this.CalculateDeltas(GameController.instance.controlledPawns[3].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);
            if (this.CheckIfEating(newSpot))
                this.CalculateDeltas(GameController.instance.controlledPawns[3].Spot, lastRolledValue - 1, out deltaX, out deltaY, out newSpot);

            pawnFour.transform.position = new Vector3(pawnFour.transform.position.x + deltaX, pawnFour.transform.position.y + deltaY, pawnFour.transform.position.z);
            GameController.instance.controlledPawns[3].UpdatePosition(deltaX, deltaY, newSpot);

            if (newSpot > 55)
            {
                GameController.instance.controlledPawns[0].Spot = 59; //in house so there is no "eating"
                AudioManager.instance.PlayInHouseSound();
            }

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
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
        GameController.instance.SerializeGameState();

        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(4);
    }

    public void HelpMenu()
    {
        GameController.instance.SerializeGameState();

        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(5);
    }

    #endregion
}
