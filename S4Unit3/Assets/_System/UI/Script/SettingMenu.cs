using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider sliderBGM, sliderSFX;

    //Resolution[] resolutions;

    //public Dropdown resolutionDropdown;

    private void Start()
    {
        //All For Windows Size Fixed Used
        //resolutions = Screen.resolutions;
        //resolutionDropdown.ClearOptions();
        //List<string> options = new List<string>();

        //int currentResolutionIndex = 0;
        //for(int i = 0;i < resolutions.Length ; i++)
        //{
        //    string option = resolutions[i].width + " x " + resolutions[i].height;
        //    options.Add(option);

        //    if (resolutions[i].width == Screen.currentResolution.width &&
        //        resolutions[i].height == Screen.currentResolution.height)
        //    {
        //        currentResolutionIndex = i;
        //    }
        //}

        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();
    }

    //public void SetResolution(int resolutionIndex)
    //{
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //}

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        float valueBGM;
        audioMixer.GetFloat("MusicVolume", out valueBGM);
        sliderBGM.value = valueBGM;

        float valueSFX;
        audioMixer.GetFloat("SFXVolume", out valueSFX);
        sliderSFX.value = valueSFX;
    }

    public void SetMainVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
