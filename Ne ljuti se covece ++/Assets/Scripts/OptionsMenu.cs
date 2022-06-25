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

    //private Dictionary<string, KeyCode> keyCodes = new Dictionary<string, KeyCode>();
    public Text pawnOne, pawnTwo, pawnThree, pawnFour, rollDice, goBack, newGame, optionMenu, helpMenu;
    private GameObject currentKey;

    void Start()
    {
        allSoundsVolumeSlider.value = AudioManager.instance.allVolumeMultiplier;
        backgroundVolumeSlider.value = AudioManager.instance.backgroundVolume;
        effectsVolumeSlider.value = AudioManager.instance.effectsVolume;

        pawnOne.text = GameController.instance.keyCodes["pawnOne"].ToString();
        pawnTwo.text = GameController.instance.keyCodes["pawnTwo"].ToString();
        pawnThree.text = GameController.instance.keyCodes["pawnThree"].ToString();
        pawnFour.text = GameController.instance.keyCodes["pawnFour"].ToString();
        rollDice.text = GameController.instance.keyCodes["rollDice"].ToString();
        goBack.text = GameController.instance.keyCodes["goBack"].ToString();
        newGame.text = GameController.instance.keyCodes["newGame"].ToString();
        optionMenu.text = GameController.instance.keyCodes["optionMenu"].ToString();
        helpMenu.text = GameController.instance.keyCodes["helpMenu"].ToString();
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
                GameController.instance.keyCodes[currentKey.name] = e.keyCode;
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
            AudioManager.instance.SetBackgroundVolume(backgroundVolumeSlider.value);
            AudioManager.instance.SetEffectsVolume(effectsVolumeSlider.value);
        }    
    }

    public void SetBackgroundVolume()
    {
        if (AudioManager.instance.backgroundVolume != backgroundVolumeSlider.value)
        {
            AudioManager.instance.SetBackgroundVolume(backgroundVolumeSlider.value);          
        }
    }

    public void SetEffectsVolume()
    {
        if (AudioManager.instance.effectsVolume != effectsVolumeSlider.value)
        {
            AudioManager.instance.SetEffectsVolume(effectsVolumeSlider.value);        
        }
    }

    public void SaveValues()
    {
        foreach (var entry in GameController.instance.keyCodes)
        {
            PlayerPrefs.SetString(entry.Key, entry.Value.ToString());
        }

        PlayerPrefs.SetFloat("allSoundsVolume", allSoundsVolumeSlider.value);
        PlayerPrefs.SetFloat("backgroundVolume", backgroundVolumeSlider.value);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolumeSlider.value);

        PlayerPrefs.Save();
    }

    public void GoBack()
    {
        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(GameController.instance.lastScene);     
    }
}
