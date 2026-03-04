using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown resolutionDropdown;
    public AudioMixer mainMixer;
    public Toggle fullscreenToggle;

    [Header("Navigation")]
    public GameObject settingsPanel;     // The Settings UI itself
    public GameObject previousMenuPanel; // The Main Menu or Pause Menu UI

    private Resolution[] resolutions;

    void Start()
    {
        // Setup resolution options immediately on game start
        SetupResolutions();

        // Sync the toggle with the actual game state
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = Screen.fullScreen;
    }

    void Update()
    {
        // Support for the Escape key to go back
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                GoBack();
            }
        }
    }

    private void SetupResolutions()
    {
        if (resolutionDropdown == null) return;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio.value.ToString("0") + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        
        // This is the CRASH FIX: Set value without triggering the SetResolution function
        resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        // Safety guard: Prevents NullReferenceException
        if (resolutions == null || resolutions.Length == 0) return;

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        if (mainMixer == null) return;

        // Linear (0-1) to Logarithmic (-80 to 0)
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        mainMixer.SetFloat("MasterVolume", dB);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void GoBack()
    {
        // Hide settings
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // Show the menu we came from
        if (previousMenuPanel != null) previousMenuPanel.SetActive(true);
    }
}