using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void OnEnable()
    {
        LoadVolume();
    }

    // �����
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 15);
        SaveVolume(volume);
    }

    // ���������� ���������
    private void SaveVolume(float volume)
    {
        slider.value = volume;
        PlayerPrefs.SetFloat("VolumePreference", volume);
    }

    // �������� ��������
    private void LoadVolume()
    {
        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumePreference");
            SetVolume(savedVolume);
        }
        else
            SetVolume(0.5f);
    }

    // ��� ����������
    public TMP_Dropdown resolutionDropdown;

    public Slider slider;

    Resolution[] resolutions;

    public void StartSetSettings()
    {
        // �������������� ����������� ����������
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
        gameObject.SetActive(false);
    }

    // ���������������
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // ����������
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // ���������� ��������
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        SaveVolume(slider.value);
    }

    // �������� ���������� ��������
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
