using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;     // Untuk musik background
    public AudioSource sfxSource;       // Untuk sound effect

    [Header("UI Elements")]
    public Slider musicSlider;          // Slider untuk volume musik
    public Slider sfxSlider;            // Slider untuk volume sfx

    void Start()
    {
        // Load saved volume settings jika ada
        LoadSettings();

        // Setup listeners untuk slider
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void LoadSettings()
    {
        // Load volume settings atau gunakan default 0.5 jika belum ada
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        // Set nilai ke slider dan audio source
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
}
