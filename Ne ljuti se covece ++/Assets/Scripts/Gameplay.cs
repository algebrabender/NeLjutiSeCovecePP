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
    /*
     * Board image from: https://pngtree.com/so/Adventure
     */

    public Button rollDiceButton;
    public Button pawnOneButton;
    public Button pawnTwoButton;
    public Button pawnThreeButton;
    public Button pawnFourButton;

    public Text historyText;

    public AudioSource diceRollAudioSource;

    public Image boardImage;
    public Sprite redBoardSprite;
    public Sprite blueBoardSprite;
    public Sprite greenBoardSprite;
    public Sprite yellowBoardSprite;

    private bool allowPlay = false;

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
            if (!GameController.instance.AIPawns[pawnNumber + (i * 4)].Eaten)
            {
                diceRollAudioSource.Play();

                //TODO: make turn

                historyText.text += "Pawn number: " + pawnNumber + " i: " + i + " index: " + (pawnNumber + (i * 4)) + "\n";
                pawnNumber = Random.Range(0, 4);
                rolledNumber = Random.Range(1, 7);

                yield return new WaitForSeconds(3);
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
        diceRollAudioSource.Play();

        int value = this.GetNumber();

        historyText.text += "Rolled number: " + value + "\n";

        GameController.instance.lastRolledValue = value;

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
        if (!GameController.instance.controlledPawns[0].Eaten)
        {
            GameController.instance.controlledPawns[0].UpdateLives();
            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnTwo()
    {
        if (!GameController.instance.controlledPawns[1].Eaten)
        {
            GameController.instance.controlledPawns[1].UpdateLives();
            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnThree()
    {
        if (!GameController.instance.controlledPawns[2].Eaten)
        {
            GameController.instance.controlledPawns[2].UpdateLives();
            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
    }

    public void PawnFour()
    {
        if (!GameController.instance.controlledPawns[3].Eaten)
        {
            GameController.instance.controlledPawns[3].UpdateLives();
            this.DisableButtons();
            StartCoroutine(this.AITurn());
        }
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
