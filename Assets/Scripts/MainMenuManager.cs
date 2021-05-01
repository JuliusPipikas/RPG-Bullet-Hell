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
    private GameObject title;
    [SerializeField]
    private GameObject controls;

    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Image loadingSprite;
    [SerializeField]
    private List<Sprite> loadingSprites;

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

    [SerializeField]
    private AudioClip start;
    [SerializeField]
    private AudioClip buttonPress;
    [SerializeField]
    private AudioClip Interact;
    [SerializeField]
    private AudioClip menuMusic;

    [SerializeField]
    private Texture2D cursor;

    public void Awake()
    {
        Cursor.SetCursor(cursor, new Vector2(cursor.width/2, cursor.height/2), CursorMode.Auto);
        QualitySettings.SetQualityLevel(3);
        GameObject preservedValues = GameObject.Find("preservedValues");
        if (preservedValues)
        {
            qualityDropdown.value = preservedValues.GetComponent<PreserveValues>().quality_level;
            music.volume = preservedValues.GetComponent<PreserveValues>().music_volume;
            sfx.volume = preservedValues.GetComponent<PreserveValues>().sfx_volume;
            Object.Destroy(preservedValues);
        }
        else
        {
            qualityDropdown.value = 3;
        }

        musicSlider.value = music.volume;
        sfxSlider.value = sfx.volume;
        music.clip = menuMusic;
        music.loop = true;
        music.Play();

    }
    public void onStartGame()
    {
        if (sfx)
        {
            sfx.PlayOneShot(buttonPress);
            Object.DontDestroyOnLoad(soundManager);
        }
        menu.SetActive(false);
        loadingScreen.SetActive(true);
        title.SetActive(false);
        if (sfx)
        {
            StartCoroutine(LoadAsynchronously());
        }
    }

    public void mockWindows()
    {
        menu = new GameObject();
        title = new GameObject();
        controls = new GameObject();
        controls.SetActive(false);
        loadingScreen = new GameObject();
        loadingScreen.SetActive(false);
        options = new GameObject();
        options.SetActive(false);
    }

    public bool menuActive()
    {
        return menu.activeSelf;
    }

    public bool optionsActive()
    {
        return options.activeSelf;
    }

    public bool titleActive()
    {
        return title.activeSelf;
    }

    public bool controlsActive()
    {
        return controls.activeSelf;
    }
    public bool loadingActive()
    {
        return loadingScreen.activeSelf;
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        while (!operation.isDone)
        {
            loadingSprite.sprite = loadingSprites[Mathf.RoundToInt(operation.progress * 10)];
            yield return null;
        }
    }

    public void onExit()
    {
        sfx.PlayOneShot(buttonPress);
        Application.Quit();
        Debug.Log("Quit Application");
    }

    public void onOptions()
    {
        if (sfx)
        {
            sfx.PlayOneShot(buttonPress);
        }
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void onControls()
    {
        if (sfx)
        {
            sfx.PlayOneShot(buttonPress);
        }
        menu.SetActive(false);
        controls.SetActive(true);
    }
    public void onControlsBack()
    {
        if (sfx)
        {
            sfx.PlayOneShot(buttonPress);
        }
        menu.SetActive(true);
        controls.SetActive(false);
    }

    public void onOptionsBack()
    {
        if (sfx)
        {
            sfx.PlayOneShot(buttonPress);
        }
        menu.SetActive(true);
        options.SetActive(false);
    }

    bool t = true;

    public void onChangeQuality()
    {
        if (!t)
        {
            sfx.PlayOneShot(Interact);
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
        else
        {
            music.volume = musicSlider.value;
            sfx.volume = sfxSlider.value;
        }
    }
}
