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
    public Button rollDiceButton;
    public Button pawnOneButton;
    public Button pawnTwoButton;
    public Button pawnThreeButton;
    public Button pawnFourButton;

    public Text historyText;

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
                break;
        }
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

    private void AITurn()
    {
        int rolledNumber = Random.Range(1, 7);

        int pawnNumber = Random.Range(0, 4);

        for (int i = 0; i < 3; i++)
        {
            if (!GameController.instance.AIPawns[pawnNumber + (i * 4)].Eaten)
            {
                //TODO: make turn
                Debug.Log("Pawn number: " + pawnNumber + " i: " + i + " index: " + (pawnNumber + (i * 4)));
                pawnNumber = Random.Range(0, 4);
                rolledNumber = Random.Range(1, 7);
                continue;
            }
            else
            {
                pawnNumber = Random.Range(0, 3);
                i--;
            }
        }
    }

    void Start()
    {
        GameController.instance.lastScene = SceneManager.GetActiveScene().buildIndex;

        historyText.text += "\nColor: " + GameController.instance.playerColor + " Difficulty: " + GameController.instance.gameDifficulty + "\n";

        this.SetColors();

        GameController.instance.SetPawns();
        GameController.instance.SetAIPawns();

        pawnOneButton.enabled = false;
        pawnTwoButton.enabled = false;
        pawnThreeButton.enabled = false;
        pawnFourButton.enabled = false;
    }

    void Update()
    {
        
    }

    public void RollDice()
    {
        int value = this.GetNumber();

        historyText.text += "\nRolled number: " + value + "\n";

        GameController.instance.lastRolledValue = value;

        List<Button> buttons = new List<Button>();
        buttons.AddRange(new Button[] { pawnOneButton, pawnTwoButton, pawnThreeButton, pawnFourButton });

        for (int i = 0; i < 4; i++)
        {
            if (!GameController.instance.controlledPawns[i].Eaten)
                buttons[i].enabled = true;
        }
    }

    public void PawnOne()
    {
        GameController.instance.controlledPawns[0].UpdateLives();
        this.DisableButtons();
        this.AITurn();
    }

    public void PawnTwo()
    {
        GameController.instance.controlledPawns[1].UpdateLives();
        this.DisableButtons();
        this.AITurn();
    }

    public void PawnThree()
    {
        GameController.instance.controlledPawns[2].UpdateLives();
        this.DisableButtons();
        this.AITurn();
    }

    public void PawnFour()
    {
        GameController.instance.controlledPawns[3].UpdateLives();
        this.DisableButtons();
        this.AITurn();
    }

    public void GoBack()
    {
        GameController.instance.playerColor = "";
        GameController.instance.gameDifficulty = "";
        SceneManager.LoadScene(0);
    }

    //TODO: make sure game state is saved
    public void OptionsMenu()
    {
        SceneManager.LoadScene(4);
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene(5);
    }
}
