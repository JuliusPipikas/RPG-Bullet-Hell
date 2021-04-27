using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject options;

    [SerializeField]
    private GameObject deathmenu;

    [SerializeField]
    private GameObject victorymenu;

    [SerializeField]
    private GameObject soundManager;

    [SerializeField]
    private GameObject subtitles;

    [SerializeField]
    private AudioSource music;
    [SerializeField]
    private AudioSource sfx;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private AudioSource separatesfx;

    [SerializeField]
    private Dropdown qualityDropdown;
    [SerializeField]
    private AudioClip buttonPress;
    [SerializeField]
    private AudioClip Interact;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip deathVoice;
    public AudioClip victorySound;
    public AudioClip victoryVoice;

    [SerializeField]
    private GameObject preserveValues;

    [SerializeField]
    private GameObject DarkScreen;
    
    public string deathText = "Your adventure has come to an end...";
    public string victoryText;

    [SerializeField]
    private PlayerController Player;

    public void Awake()
    {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        soundManager = GameObject.Find("SoundManager");
        music = soundManager.transform.Find("MusicManager").GetComponent<AudioSource>();
        sfx = soundManager.transform.Find("SFXManager").GetComponent<AudioSource>();
        separatesfx.volume = sfx.volume;
        musicSlider.value = music.volume;
        sfxSlider.value = sfx.volume;
    }
    public void onResume()
    {
        Player.paused = false;
        Time.timeScale = 1;
        separatesfx.PlayOneShot(buttonPress);
        DarkScreen.SetActive(false);
        music.UnPause();
        sfx.UnPause();
        menu.SetActive(false);
    }

    public void onExit()
    {
        separatesfx.PlayOneShot(buttonPress);
        Object.Destroy(soundManager);
        preserveValues.GetComponent<PreserveValues>().sfx_volume = sfx.volume;
        preserveValues.GetComponent<PreserveValues>().music_volume = music.volume;
        preserveValues.GetComponent<PreserveValues>().quality_level = qualityDropdown.value;
        DontDestroyOnLoad(preserveValues);
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public void onOptions()
    {
        separatesfx.PlayOneShot(buttonPress);
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void onOptionsBack()
    {
        separatesfx.PlayOneShot(buttonPress);
        menu.SetActive(true);
        options.SetActive(false);
    }

    bool t = true;

    public void onChangeQuality()
    {
        if (!t)
        {
            separatesfx.PlayOneShot(Interact);
        }
        t = false;
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    bool tt = true;

    public void setVolume()
    {
        if (tt)
        {
            tt = false;
        }
        else {
            music.volume = musicSlider.value;
            sfx.volume = sfxSlider.value;
            separatesfx.volume = sfxSlider.value;
        }
    }

    public void deathCall()
    {
        DarkScreen.SetActive(true);
        StartCoroutine(onDeath());
    }

    public void victoryCall()
    {
        DarkScreen.SetActive(true);
        StartCoroutine(onVictory());
    }

    public IEnumerator onDeath()
    {
        deathmenu.SetActive(true);
        Time.timeScale = 0;
        music.volume = music.volume / 8;
        yield return new WaitForSecondsRealtime(0.5f);
        
        separatesfx.PlayOneShot(deathSound);
        yield return new WaitForSecondsRealtime(deathSound.length);
        Text t = subtitles.GetComponent<Text>();
        t.text = deathText;
        subtitles.SetActive(true);
        separatesfx.PlayOneShot(deathVoice);
        yield return new WaitForSecondsRealtime(deathVoice.length);
        music.volume = music.volume * 8;
    }

    public IEnumerator onVictory()
    {
        victorymenu.SetActive(true);
        Time.timeScale = 0;
        music.volume = music.volume / 8;
        yield return new WaitForSecondsRealtime(0.5f);

        separatesfx.PlayOneShot(victorySound);
        yield return new WaitForSecondsRealtime(victorySound.length);
        Text t = subtitles.GetComponent<Text>();
        t.text = victoryText;
        subtitles.SetActive(true);
        separatesfx.PlayOneShot(victoryVoice);
        yield return new WaitForSecondsRealtime(victoryVoice.length);
        music.volume = music.volume * 8;
    }
}
