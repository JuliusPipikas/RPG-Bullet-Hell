using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject options;

    [SerializeField]
    private GameObject soundManager;

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Dropdown qualityDropdown;

    public void Awake()
    {
        QualitySettings.SetQualityLevel(3);
        qualityDropdown.value = 3;
        musicSlider.value = 1;
        sfxSlider.value = 1;
    }
    public void onStartGame()
    {
        Object.DontDestroyOnLoad(soundManager);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void onExit()
    {
        Application.Quit();
        Debug.Log("Quit Application");
    }

    public void onOptions()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void onOptionsBack()
    {
        menu.SetActive(true);
        options.SetActive(false);
    }

    public void onChangeQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    public void setVolume()
    {
        music.volume = musicSlider.value;
        sfx.volume = sfxSlider.value;
    }
}
