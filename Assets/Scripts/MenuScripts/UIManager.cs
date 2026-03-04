using UnityEngine;
using UnityEngine.UI; // Required for slider references

public class UIManager : MonoBehaviour
{
    [Header("Audio Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // 1. Initialize sliders with current SoundManager volume
        if (SoundManager.instance != null)
        {
            // Set slider to match current volume
            musicSlider.value = SoundManager.instance.musicSource.volume;
            sfxSlider.value = SoundManager.instance.sfxSource.volume;
        }

        // 2. Add listeners to the sliders so they call the functions in SoundManager
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetMusicVolume(value);
        }
    }

    public void SetSFXVolume(float value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetSFXVolume(value);
        }
    }
}