using DG.Tweening;
using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public enum VERSION
    {
        Alpha,
        Beta,
        Release
    }

    [Header("General")]
    [SerializeField] private Image logoImg = null;

    [Header("Version")]
    [SerializeField] private bool _isDemo = false;
    [SerializeField] private VERSION version = VERSION.Alpha;
    [SerializeField, Range(0, 10)] private int versionReview = 0;
    [SerializeField] private TextMeshProUGUI versionTxt = null;

    [Header("DEPRECATED")]
    public Image _fadeImg;
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject Popup;
    [Space]
    public Slider sliderSound;
    public Slider sliderMusic;
    public StudioEventEmitter _buttonSounds;
    public StudioEventEmitter _menuMusic;

    private FadeEvent _fadeEvent;
    private GameObject _lastPanel;

    private Vector2 _logoStartSize = new Vector2(600, 600);
    private float _logoStartY = 50f;

    private Vector2 _logoEndSize = new Vector2(475, 475);
    private float _logoEndY = 150f;

    // private bool _isDataLoaded;
    // public bool IsDataLoaded { get { return _isDataLoaded; } }

    private void Awake()
    {
        SetVersion();
    }

    private void Start()
    {
        sliderSound.onValueChanged.AddListener(VolumeSound);
        sliderMusic.onValueChanged.AddListener(VolumeMusic);

        _fadeEvent = new FadeEvent();
        _fadeEvent.instant = true;
        // _fadeEvent.callbackMid = ChangeScene;

        // if (GameData.Data.isDataLoaded)
        // {
        //     NewGameButton.SetActive(false);
        //     ContinueButton.SetActive(true);
        // }

        mainPanel.SetActive(true);

        _menuMusic.Play();
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

                PlayButtonSound(3);
                break;

            case MENU_TYPE.NewGame:
                Fade();

                // GameData.Data.isDataLoaded = true;

                PlayButtonSound(3);
                break;

            case MENU_TYPE.Options:
                creditsPanel.SetActive(false);
                optionsPanel.SetActive(true);

                PlayButtonSound(1);
                break;

            case MENU_TYPE.Credits:
                optionsPanel.SetActive(false);
                creditsPanel.SetActive(true);

                _menuMusic.EventInstance.setParameterByName(FMODParameters.Credits, 1f);
                PlayButtonSound(1);
                break;

            case MENU_TYPE.Back:

                DesactivateAll();

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

    public void SetVersion()
    {
        versionTxt.text = string.Format("{0}{1}{2}{3}", _isDemo ? "Demo " : "", Application.version, GetVersion(), versionReview == 0 ? "" : versionReview.ToString());
    }

    private string GetVersion()
    {
        switch (version)
        {
            case VERSION.Alpha:
                return "a";
            case VERSION.Beta:
                return "b";
            case VERSION.Release:
                return "-RC";
        }

        return ".";
    }
}