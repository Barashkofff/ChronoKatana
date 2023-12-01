using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    // Аудио
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        SaveVolume(volume);
    }

    // Сохранение громкости
    private void SaveVolume(float volume)
    {
        slider.value = volume;
        PlayerPrefs.SetFloat("VolumePreference", volume);
    }

    // Загрузка настроек
    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumePreference");
            SetVolume(savedVolume);
        }
        else
        {
            SetVolume(0.3f); 
        }
    }

    // Для сохранения
    public TMP_Dropdown resolutionDropdown;

    public Slider slider;

    Resolution[] resolutions;

    void Start()
    {
        // Автоматическое определение разрешения
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);

        LoadVolume();
    }

    // Полноэкранность
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Разрешение
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Сохранение настроек
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        float value;
        bool flag = audioMixer.GetFloat("Volume", out value);
        if (flag) 
            SaveVolume(value);
        else
            SaveVolume(0.3f);
    }

    // Загрузка сохранённых настроек
    public void LoadSettings(int currentResolutionIndex)
    {
        
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;

        LoadVolume();
    }
}
