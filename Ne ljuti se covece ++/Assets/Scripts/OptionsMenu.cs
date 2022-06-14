using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider allSoundsVolumeSlider;
    public Slider backgroundVolumeSlider;
    public Slider effectsVolumeSlider;

    private Dictionary<string, KeyCode> keyCodes = new Dictionary<string, KeyCode>();
    public Text pawnOne, pawnTwo, pawnThree, pawnFour, rollDice, goBack, newGame, optionMenu, helpMenu;
    private GameObject currentKey;

    void Start()
    {
        allSoundsVolumeSlider.value = PlayerPrefs.GetFloat("allSoundsVolume");
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat("backgroundVolume");
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("effectsVolume");

        keyCodes.Add("pawnOne", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnOne", "Alpha1")));
        keyCodes.Add("pawnTwo", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnTwo", "Alpha2")));
        keyCodes.Add("pawnThree", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnThree", "Alpha3")));
        keyCodes.Add("pawnFour", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("pawnFour", "Alpha4")));
        keyCodes.Add("rollDice", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rollDice", "D")));
        keyCodes.Add("goBack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("goBack", "Escape")));
        keyCodes.Add("newGame", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("newGame", "Space")));
        keyCodes.Add("optionMenu", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("optionMenu", "O")));
        keyCodes.Add("helpMenu", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("helpMenu", "H")));

        pawnOne.text = keyCodes["pawnOne"].ToString();
        pawnTwo.text = keyCodes["pawnTwo"].ToString();
        pawnThree.text = keyCodes["pawnThree"].ToString();
        pawnFour.text = keyCodes["pawnFour"].ToString();
        rollDice.text = keyCodes["rollDice"].ToString();
        goBack.text = keyCodes["goBack"].ToString();
        newGame.text = keyCodes["newGame"].ToString();
        optionMenu.text = keyCodes["optionMenu"].ToString();
        helpMenu.text = keyCodes["helpMenu"].ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("goBack", "Escape"))))
        {
            this.GoBack();
        }
    }

    void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keyCodes[currentKey.name] = e.keyCode;
                currentKey.GetComponentInChildren<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        if (currentKey != null)
        {

        }

        currentKey = clicked;
    }

    public void SetAllSoundsVolume()
    {
        if (AudioManager.instance.allVolumeMultiplier != allSoundsVolumeSlider.value)
        {
            AudioManager.instance.SetAllSoundsVolume(allSoundsVolumeSlider.value);
            PlayerPrefs.SetFloat("allSoundsVolume", allSoundsVolumeSlider.value);
        }
    }

    public void SetBackgroundVolume()
    {
        if (AudioManager.instance.backgroundVolume != backgroundVolumeSlider.value)
        {
            AudioManager.instance.SetBackgroundVolume(backgroundVolumeSlider.value);
            PlayerPrefs.SetFloat("backgroundVolume", backgroundVolumeSlider.value);
        }
    }

    public void SetEffectsVolume()
    {
        if (AudioManager.instance.effectsVolume != effectsVolumeSlider.value)
        {
            AudioManager.instance.SetEffectsVolume(effectsVolumeSlider.value);
            PlayerPrefs.SetFloat("effectsVolume", effectsVolumeSlider.value);
        }
    }

    public void SaveValues()
    {
        foreach (var entry in keyCodes)
        {
            PlayerPrefs.SetString(entry.Key, entry.Value.ToString());
        }

        PlayerPrefs.Save();
    }

    public void GoBack()
    {
        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(GameController.instance.lastScene);
    }
}
