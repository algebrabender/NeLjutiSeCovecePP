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

    void Start()
    {
        using (StreamReader sr = new StreamReader("optionValues.txt"))
        {
            string line = sr.ReadLine();
            allSoundsVolumeSlider.value = float.Parse(line.Split(':')[1]);
            line = sr.ReadLine();
            backgroundVolumeSlider.value = float.Parse(line.Split(':')[1]);
            line = sr.ReadLine();
            effectsVolumeSlider.value = float.Parse(line.Split(':')[1]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.GoBack();
        }
    }

    public void SetAllSoundsVolume()
    {
        if (AudioManager.instance.allVolumeMultiplier != allSoundsVolumeSlider.value)
            AudioManager.instance.SetAllSoundsVolume(allSoundsVolumeSlider.value);
    }

    public void SetBackgroundVolume()
    {
        if (AudioManager.instance.backgroundVolume != backgroundVolumeSlider.value)
            AudioManager.instance.SetBackgroundVolume(backgroundVolumeSlider.value);
    }

    public void SetEffectsVolume()
    {
        if (AudioManager.instance.effectsVolume != effectsVolumeSlider.value)
            AudioManager.instance.SetEffectsVolume(effectsVolumeSlider.value);
    }

    public void GoBack()
    {
        using (StreamWriter sw = new StreamWriter("optionValues.txt"))
        {
            sw.WriteLine("All Sounds Volume:" + allSoundsVolumeSlider.value);
            sw.WriteLine("Background Volume:" + backgroundVolumeSlider.value);
            sw.WriteLine("Effects Volume:" + effectsVolumeSlider.value);
        }

        AudioManager.instance.PlayButtonPressedSound();
        SceneManager.LoadScene(GameController.instance.lastScene);
    }
}
