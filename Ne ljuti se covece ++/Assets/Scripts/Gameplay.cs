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
        ColorBlock colorBlockOne = pawnOneButton.colors;
        ColorBlock colorBlockTwo = pawnTwoButton.colors;
        ColorBlock colorBlockThree = pawnThreeButton.colors;
        ColorBlock colorBlockFour = pawnFourButton.colors;

        switch (GameController.instance.playerColor)
        {
            case "red":
                colorBlockOne.normalColor = new Color(241f / 255f, 87f / 255f, 87f / 255f);
                pawnOneButton.colors = colorBlockOne;
                colorBlockTwo.normalColor = new Color(241f / 255f, 87f / 255f, 87f / 255f);
                pawnTwoButton.colors = colorBlockTwo;
                colorBlockThree.normalColor = new Color(241f / 255f, 87f / 255f, 87f / 255f);
                pawnThreeButton.colors = colorBlockThree;
                colorBlockFour.normalColor = new Color(241f / 255f, 87f / 255f, 87f / 255f);
                pawnFourButton.colors = colorBlockFour;
                boardImage.sprite = redBoardSprite;
                break;
            case "blue":
                colorBlockOne.normalColor = new Color(93f / 255f, 102f / 255f, 255f / 255f);
                pawnOneButton.colors = colorBlockOne;
                colorBlockTwo.normalColor = new Color(93f / 255f, 102f / 255f, 255f / 255f);
                pawnTwoButton.colors = colorBlockTwo;
                colorBlockThree.normalColor = new Color(93f / 255f, 102f / 255f, 255f / 255f);
                pawnThreeButton.colors = colorBlockThree;
                colorBlockFour.normalColor = new Color(93f / 255f, 102f / 255f, 255f / 255f);
                pawnFourButton.colors = colorBlockFour;
                boardImage.sprite = blueBoardSprite;
                break;
            case "green":
                colorBlockOne.normalColor = new Color(53f / 255f, 204f / 255f, 97f / 255f);
                pawnOneButton.colors = colorBlockOne;
                colorBlockTwo.normalColor = new Color(53f / 255f, 204f / 255f, 97f / 255f);
                pawnTwoButton.colors = colorBlockTwo;
                colorBlockThree.normalColor = new Color(53f / 255f, 204f / 255f, 97f / 255f);
                pawnThreeButton.colors = colorBlockThree;
                colorBlockFour.normalColor = new Color(53f / 255f, 204f / 255f, 97f / 255f);
                pawnFourButton.colors = colorBlockFour;
                boardImage.sprite = greenBoardSprite;
                break;
            case "yellow":
                colorBlockOne.normalColor = new Color(255f / 255f, 238f / 255f, 78f / 255f);
                pawnOneButton.colors = colorBlockOne;
                colorBlockTwo.normalColor = new Color(255f / 255f, 238f / 255f, 78f / 255f);
                pawnTwoButton.colors = colorBlockTwo;
                colorBlockThree.normalColor = new Color(255f / 255f, 238f / 255f, 78f / 255f);
                pawnThreeButton.colors = colorBlockThree;
                colorBlockFour.normalColor = new Color(255f / 255f, 238f / 255f, 78f / 255f);
                pawnFourButton.colors = colorBlockFour;
                boardImage.sprite = yellowBoardSprite;
                break;
        }
    }

    private void SetButtonListeners()
    {
        rollDiceButton.onClick.RemoveAllListeners();

        rollDiceButton.onClick.AddListener(AudioManager.instance.PlayButtonPressedSound);
        rollDiceButton.onClick.AddListener(AudioManager.instance.PlayDiceRollSound);

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
        allowPlay = false;

        int rolledNumber = Random.Range(1, 7);
        int pawnNumber = Random.Range(0, 4);

        for (int i = 0; i < 3; i++)
        {
            if (!GameController.instance.AIPawns[pawnNumber + (i * 4)].Out && rolledNumber == 6)
            {

                historyText.text += "Kockica je pala na: " + rolledNumber + "\n";
                historyText.text += "Pawn number: " + pawnNumber + " i: " + i + " index: " + (pawnNumber + (i * 4)) + "\n";
                historyText.text += "----------------------------------------\n";

                float newX;
                float newY;
                if (i == 0) //upper left
                {
                    newX = 103.38f;
                    newY = 366.72f;
                }
                else if (i == 1) //upper right
                {
                    newX = 384.25f;
                    newY = 569.27f;
                }
                else //lower right
                {
                    newX = 584.21f;
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
                        break;
                    }
                }

                if (!thereIsOuted)
                {
                    AIPawns[pawnNumber + (i * 4)].transform.position = new Vector3(newX, newY, AIPawns[pawnNumber + (i * 4)].transform.position.z);
                    GameController.instance.AIPawns[pawnNumber + (i * 4)].OutedPawn(newX, newY);

                    pawnNumber = Random.Range(0, 4);
                    rolledNumber = Random.Range(1, 7);
                    //rolledNumber = 6;
                }
                else
                {
                    pawnNumber = Random.Range(0, 4);
                    rolledNumber = Random.Range(1, 7);
                    //rolledNumber = 6;
                }

                yield return new WaitForSeconds(2);
            }
            else if (GameController.instance.AIPawns[pawnNumber + (i * 4)].Out && !GameController.instance.AIPawns[pawnNumber + (i * 4)].Eaten)
            {
                AudioManager.instance.PlayDiceRollSound();

                historyText.text += "Kockica je pala na: " + rolledNumber + "\n";
                historyText.text += "Pawn number: " + pawnNumber + " i: " + i + " index: " + (pawnNumber + (i * 4)) + "\n";
                historyText.text += "----------------------------------------\n";

                float deltaX, deltaY;
                int newSpot;
                this.CalculateDeltas(GameController.instance.AIPawns[pawnNumber + (i * 4)].Spot, rolledNumber, out deltaX, out deltaY, out newSpot);

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

                AIPawns[pawnNumber + (i * 4)].transform.position = new Vector3(AIPawns[pawnNumber + (i * 4)].transform.position.x + deltaX, AIPawns[pawnNumber + (i * 4)].transform.position.y + deltaY, AIPawns[pawnNumber + (i * 4)].transform.position.z);
                GameController.instance.AIPawns[pawnNumber + (i * 4)].UpdateLives();
                GameController.instance.AIPawns[pawnNumber + (i * 4)].UpdatePosition(deltaX, deltaY, newSpot);

                pawnNumber = Random.Range(0, 4);
                rolledNumber = Random.Range(1, 7);

                yield return new WaitForSeconds(2);
            }
            else
            {
                historyText.text += "Turn skipped\n";
                historyText.text += "----------------------------------------\n";
                //pawnNumber = Random.Range(0, 3);
                //rolledNumber = Random.Range(1, 7);
                //i--;
            }
        }

        //yield return new WaitForSeconds(2);
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
    
    private void CalculateDeltas(int currentSpot, int rolledValue, out float deltaX, out float deltaY, out int newSpot)
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

        //TODO: kucice
    }
    #endregion

    #region Methodes
    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        historyText.text = "";

        this.SetColors();

        this.SetButtonListeners();

        GameController.instance.SetPawns((Vector2)pawnOne.transform.position, (Vector2)pawnTwo.transform.position, 
                                         (Vector2)pawnThree.transform.position, (Vector2)pawnFour.transform.position);

        Vector2[] positions = new Vector2[12];

        for (int i = 0; i < 4; i++)
        {
            positions[i] =(Vector2)AIPawns[i].transform.position;
            positions[i + 4] = (Vector2)AIPawns[i + 4].transform.position;
            positions[i + 8] = (Vector2)AIPawns[i + 8].transform.position;
        }

        GameController.instance.SetAIPawns(positions);

        this.SetPawnNumbers();

        pawnOneButton.enabled = false;
        pawnTwoButton.enabled = false;
        pawnThreeButton.enabled = false;
        pawnFourButton.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GoBack();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            this.RollDice();
        }
        if (allowPlay)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.PawnOne();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.PawnTwo();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.PawnThree();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
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

        for (int i = 0; i < 4; i++)
        {
            if (!GameController.instance.controlledPawns[i].Eaten)
                buttons[i].enabled = true;
        }

        allowPlay = true;
    }

    public void PawnOne()
    {
        if (!GameController.instance.controlledPawns[0].Out && lastRolledValue == 6
            && (float)System.Math.Round(GameController.instance.controlledPawns[1].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[1].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[2].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[2].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[3].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[3].Position.y, 2) != 83.87f)
        {
            pawnOne.transform.position = new Vector3(pawnOne.transform.position.x + 102.6f, pawnOne.transform.position.y - 19.54f, pawnOne.transform.position.z);
            GameController.instance.controlledPawns[0].OutedPawn(pawnOne.transform.position.x, pawnOne.transform.position.y);

            this.DisableButtons();
        }
        else if (GameController.instance.controlledPawns[0].Out && !GameController.instance.controlledPawns[0].Eaten)
        {
            float deltaX, deltaY;
            int newSpot;
            this.CalculateDeltas(GameController.instance.controlledPawns[0].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);
            
            pawnOne.transform.position = new Vector3(pawnOne.transform.position.x + deltaX, pawnOne.transform.position.y + deltaY, pawnOne.transform.position.z);
            GameController.instance.controlledPawns[0].UpdatePosition(deltaX, deltaY, newSpot);
              
            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnTwo()
    {
        if (!GameController.instance.controlledPawns[1].Out && lastRolledValue == 6 
            && (float)System.Math.Round(GameController.instance.controlledPawns[0].Position.x, 2) != 303.35f 
            && (float)System.Math.Round(GameController.instance.controlledPawns[0].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[2].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[2].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[3].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[3].Position.y, 2) != 83.87f)
        {
            pawnTwo.transform.position = new Vector3(pawnTwo.transform.position.x + 174.73f, pawnTwo.transform.position.y - 19.54f, pawnTwo.transform.position.z);
            GameController.instance.controlledPawns[1].OutedPawn(pawnTwo.transform.position.x, pawnTwo.transform.position.y);
        }
        else if (GameController.instance.controlledPawns[1].Out && !GameController.instance.controlledPawns[1].Eaten)
        {
            float deltaX, deltaY;
            int newSpot;
            this.CalculateDeltas(GameController.instance.controlledPawns[1].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);

            pawnTwo.transform.position = new Vector3(pawnTwo.transform.position.x + deltaX, pawnTwo.transform.position.y + deltaY, pawnTwo.transform.position.z);
            GameController.instance.controlledPawns[1].UpdatePosition(deltaX, deltaY, newSpot);

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnThree()
    {
        if (!GameController.instance.controlledPawns[2].Out && lastRolledValue == 6
            && (float)System.Math.Round(GameController.instance.controlledPawns[0].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[0].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[1].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[1].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[3].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[3].Position.y, 2) != 83.87f)
        {
            pawnThree.transform.position = new Vector3(pawnThree.transform.position.x + 174.73f, pawnThree.transform.position.y - 95.87f, pawnThree.transform.position.z);
            GameController.instance.controlledPawns[2].OutedPawn(pawnThree.transform.position.x, pawnThree.transform.position.y);
        }
        else if (GameController.instance.controlledPawns[2].Out && !GameController.instance.controlledPawns[2].Eaten)
        {
            float deltaX, deltaY;
            int newSpot;
            this.CalculateDeltas(GameController.instance.controlledPawns[2].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);

            pawnThree.transform.position = new Vector3(pawnThree.transform.position.x + deltaX, pawnThree.transform.position.y + deltaY, pawnThree.transform.position.z);
            GameController.instance.controlledPawns[2].UpdatePosition(deltaX, deltaY, newSpot);

            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnFour()
    {
        if (!GameController.instance.controlledPawns[3].Out && lastRolledValue == 6
            && (float)System.Math.Round(GameController.instance.controlledPawns[0].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[0].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[1].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[1].Position.y, 2) != 83.87f
            && (float)System.Math.Round(GameController.instance.controlledPawns[2].Position.x, 2) != 303.35f
            && (float)System.Math.Round(GameController.instance.controlledPawns[2].Position.y, 2) != 83.87f)
        {
            pawnFour.transform.position = new Vector3(pawnFour.transform.position.x + 102.6f, pawnFour.transform.position.y - 95.87f, pawnFour.transform.position.z);
            GameController.instance.controlledPawns[3].OutedPawn(pawnFour.transform.position.x, pawnFour.transform.position.y);
        }
        else if (GameController.instance.controlledPawns[3].Out && !GameController.instance.controlledPawns[3].Eaten)
        {
            float deltaX, deltaY;
            int newSpot;
            this.CalculateDeltas(GameController.instance.controlledPawns[3].Spot, lastRolledValue, out deltaX, out deltaY, out newSpot);

            pawnFour.transform.position = new Vector3(pawnFour.transform.position.x + deltaX, pawnFour.transform.position.y + deltaY, pawnFour.transform.position.z);
            GameController.instance.controlledPawns[3].UpdatePosition(deltaX, deltaY, newSpot);

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

    //TODO: make sure game state is saved
    public void OptionsMenu()
    {
        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(4);
    }

    public void HelpMenu()
    {
        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(5);
    }

    #endregion
}
