using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            LoadMasterVolume();
        }
        else
        {
            SetMasterVolume();
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicVolume();
        }
        else
        {
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume();
        }
    }

    private void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SetMasterVolume();
    }

    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
    }

    private void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume();
    }

    public void SetMasterVolume()
    {
        float vol = masterSlider.value;
        mixer.SetFloat("Master", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("MasterVolume", vol);
    }

    public void SetMusicVolume()
    {
        float vol = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("MusicVolume", vol);
    }

    public void SetSFXVolume()
    {
        float vol = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(vol) * 20);
        PlayerPrefs.SetFloat("SFXVolume", vol);
    }
}
