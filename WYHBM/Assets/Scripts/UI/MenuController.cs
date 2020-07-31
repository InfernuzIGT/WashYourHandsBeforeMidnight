using System.Collections;
using DG.Tweening;
using Events;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private FadeEvent _fadeEvent;
    public Image _fadeImg;

    [Header("Cameras")]
    public GameObject[] cams;

    [Header("Generals")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    [Space]
    public GameObject Popup;

    [Header("FMOD")]
    public Slider sliderSound;
    public Slider sliderMusic;
    // ADD SFX SLIDER

    [Space]

    public StudioEventEmitter _buttonSounds;
    public StudioEventEmitter _menuMusic;

    private GameObject _lastCam;
    private GameObject _lastPanel;

    // private bool _isDataLoaded;
    // public bool IsDataLoaded { get { return _isDataLoaded; } }

    private void Start()
    {
        sliderSound.onValueChanged.AddListener(VolumeSound);
        sliderMusic.onValueChanged.AddListener(VolumeMusic);

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = true;
        _fadeEvent.callbackMid = ChangeScene;

        // if (GameData.Data.isDataLoaded)
        // {
        //     NewGameButton.SetActive(false);
        //     ContinueButton.SetActive(true);
        // }

        mainPanel.SetActive(true);

        _menuMusic.Play();
    }

    private void ChangeScene()
    {
        GameData.Instance.LoadScene(SCENE_INDEX.Master);
    }

    private void OnEnable()
    {
        EventController.AddListener<MainMenuEvent>(MainMenu);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<MainMenuEvent>(MainMenu);
    }

    private void PlayButtonSound(float parameter)
    {
        _buttonSounds.Play();
        _buttonSounds.EventInstance.setParameterByName(FMODParameters.UI, parameter);
    }

    public void MainMenu(MainMenuEvent evt)
    {
        switch (evt.menuType)
        {
            case MENU_TYPE.Continue:
                Fade();
                cams[3].SetActive(true);

                _lastCam = cams[0];

                PlayButtonSound(3);
                break;

            case MENU_TYPE.NewGame:
                Fade();
                cams[3].SetActive(true);

                _lastCam = cams[0];

                // GameData.Data.isDataLoaded = true;

                PlayButtonSound(3);
                break;

            case MENU_TYPE.Options:
                creditsPanel.SetActive(false);
                optionsPanel.SetActive(true);

                cams[1].SetActive(true);
                _lastCam = cams[0];

                PlayButtonSound(1);
                break;

            case MENU_TYPE.Credits:
                optionsPanel.SetActive(false);
                creditsPanel.SetActive(true);

                cams[2].SetActive(true);
                _lastCam = cams[0];

                _menuMusic.EventInstance.setParameterByName(FMODParameters.Credits, 1f);
                PlayButtonSound(1);
                break;

            case MENU_TYPE.Back:

                DesactivateAll();

                for (int i = 0; i < cams.Length; i++)
                {
                    cams[i].SetActive(false);
                }

                _lastCam.SetActive(true);

                PlayButtonSound(0);
                break;

            case MENU_TYPE.Exit:
                DesactivateAll();

                ShowPopup(true);
                PlayButtonSound(3);
                break;

            case MENU_TYPE.YesPopup:
                Application.Quit();
                PlayButtonSound(3);
                break;
        }
    }

    private void Fade()
    {
        // Change Fade when world is end
        _fadeImg.DOFade(1, 2f).OnKill(GoToMaster);

    }

    private void GoToMaster()
    {
        EventController.TriggerEvent(_fadeEvent);
    }

    public void ShowPopup(bool _isOpen)
    {
        Popup.SetActive(_isOpen);
    }

    public void DesactivateAll()
    {
        ShowPopup(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        PlayButtonSound(4);
        _menuMusic.EventInstance.setParameterByName(FMODParameters.Credits, 0f);
    }

    public void VolumeSound(float vol)
    {
        RuntimeManager.StudioSystem.setParameterByName("SoundsSlider", vol);
    }

    public void VolumeMusic(float vol)
    {
        RuntimeManager.StudioSystem.setParameterByName("MusicSlider", vol);
    }

    //     public void OnYesButton()
    //     {
    //         if (_isQuitting)
    //         {
    //             Application.Quit();

    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 0f);

    //         }
    //         if (_isCreatingNew)
    //         {
    //             Debug.Log($"<b> New Game is created </b>");

    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 3f);
    //             menuMusic.Stop();
    //             // Save new data in GAMEDATA

    //             LoadScene(SCENE_INDEX.Master);
    //         }

    //         if (_isContinuing)
    //         {
    //             Debug.Log($"<b> Loading Game </b>");

    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 3f);
    //             menuMusic.Stop();
    //             // Load demo scene
    //         }
    //     }

    //     public void OnNoButton()
    //     {
    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 2f);
    //     }
}