using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Audio;

public class SettingsConfiguration : MonoBehaviour
{
    public Toggle togFullscreen;
    public Toggle togVsync;
    public Toggle togMotionBlur;
    public Toggle togOcclusion;
    public Toggle togDepthOfField;

    public TMP_Text resolutionText;
    public int[] availableRes;
    public int resIndex = 1;

    public float maxSliderValue;
    public TMP_Text brightnessText;

    public PostProcessProfile profile;
    public AutoExposure exposure;
    public MotionBlur blur;
    public AmbientOcclusion occlusion;
    public DepthOfField depthOfField;

    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider SFXSlider;

    private void Start()
    {
        profile.TryGetSettings(out exposure);
        profile.TryGetSettings(out blur);
        profile.TryGetSettings(out occlusion);
        profile.TryGetSettings(out depthOfField);

        brightnessText.text = ((int)((100 * exposure.keyValue.value) / maxSliderValue)).ToString();

        togFullscreen.isOn = Screen.fullScreen; //togFullscreen.isOn ?
        togMotionBlur.isOn = blur.enabled.value;
        togVsync.isOn = QualitySettings.vSyncCount == 0 ? false : true;
        togOcclusion.isOn = occlusion.enabled.value;
        togDepthOfField.isOn = depthOfField.enabled.value;

        if(PlayerPrefs.HasKey("MusicVol")) // or PlayerPrefs.HasKeey("SFXVol")??
        {
            LoadVolume();
        }
        else
        {
            SetMasterVol();
            SetMusicVol();
            SetSFXVol();
        }
    }

    public void ApplyChanges()
    {
        Screen.SetResolution(availableRes[resIndex], availableRes[resIndex] - (resIndex + 2) * 280,togFullscreen.isOn);

        QualitySettings.vSyncCount = togVsync.isOn ? 1 : 0; //vSyncCount = 0, unlocks the frame rate. vSyncCount = 1, locks the frame rate to 144 or 60 fps (depending on the pc refresh rate)

        blur.enabled.value = togMotionBlur.isOn;
        occlusion.enabled.value = togOcclusion.isOn;
        depthOfField.enabled.value = togDepthOfField.isOn;
    }

    public void DecreaseResolution()
    {
        resIndex--;
        if(resIndex < 0 )
        {
            resIndex++;
        }
        UpdateResolutionText();
    }

    public void IncreaseResolution()
    {
        resIndex++;
        if(resIndex > availableRes.Length-1)
        {
            resIndex--;
        }
        UpdateResolutionText();
    }

    public void UpdateResolutionText()
    {
        resolutionText.text = availableRes[resIndex].ToString() + "x" + (availableRes[resIndex] - (resIndex + 2) * 280).ToString();
    }

    public void AdjustBrightness(float value) //To get the slider value into the float value variable change from the inspector to AdjustBrightess under dynamic float
    {
        //Screen.brightness = value; //Doesn't work
        exposure.keyValue.value = value;

        brightnessText.text = ((int)((100*value)/maxSliderValue)).ToString(); //100*value/maxSliderValue represents the percentage the value has reached of the maxSliderValue
    }

    public void SetMasterVol()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20); // "Master" was exposed in the audio mixer
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    public void SetMusicVol()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(musicSlider.value)*20); // "Music" was exposed in the audio mixer
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void SetSFXVol()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(SFXSlider.value) * 20); // "SFX" was exposed in the audio mixer
        PlayerPrefs.SetFloat("SFXVol", SFXSlider.value);
    }

    public void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");

        SetMasterVol();
        SetMusicVol();
        SetSFXVol();
    }
}
