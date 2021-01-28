using DG.Tweening;
using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField, ReadOnly] private UI_TYPE _currentUIType = UI_TYPE.None;
    [SerializeField, ReadOnly] private GameObject _lastGameObject = null;
    [SerializeField, ReadOnly] private SessionSettings _sessionSettings = null;
    [SerializeField] private SceneSO sceneData = null;

    [Header("Version")]
    [SerializeField] private bool _isDemo = false;
    [SerializeField] private VERSION version = VERSION.Alpha;
    [SerializeField, Range(0, 10)] private int versionReview = 0;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private TextMeshProUGUI versionTxt = null;
    [SerializeField, ConditionalHide] private Image _logoImg = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasLogo = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionAnyButton = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionBack = null;
    [Space]
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasHome = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasMain = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasOptions = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasExit = null;
    [Space]
    [SerializeField, ConditionalHide] private GameObject _firstSelectMain = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonPlay = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonOptions = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonQuit = null;
    [Space]
    [SerializeField, ConditionalHide] private GameObject _firstSelectOptions = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonLanguage = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonResolution = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonQuality = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonFullscreen = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonVsync = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonMasterVolume = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonSoundEffects = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonMusic = null;
    [SerializeField, ConditionalHide] private ButtonDoubleUI _buttonVibration = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonApply = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonBack = null;
    [Space]
    [SerializeField, ConditionalHide] private GameObject _firstSelectQuit = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonYes = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonNo = null;
    [Space]
    [SerializeField, ConditionalHide] private DeviceConfig _deviceConfig = null;
    [SerializeField, ConditionalHide] private DeviceUtility _deviceUtility = null;
    [SerializeField, ConditionalHide] private EventSystemUtility _eventSystemUtility = null;

    private ChangeSceneEvent _changeSceneEvent;

    // Settings
    private int _indexResolution;
    private int _indexQuality;
    private int _indexMasterVolume = 10;
    private int _indexSoundEffects = 10;
    private int _indexMusic = 10;

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
        _deviceConfig.UpdateDictionary();
    }

    private void Start()
    {
        // sliderSound.onValueChanged.AddListener(VolumeSound);
        // sliderMusic.onValueChanged.AddListener(VolumeMusic);

        _changeSceneEvent = new ChangeSceneEvent();
        _changeSceneEvent.load = true;
        _changeSceneEvent.isLoadAdditive = false;
        _changeSceneEvent.sceneData = sceneData;
        _changeSceneEvent.instantFade = false;

        // _menuMusic.Play();

        CreateInput();
        AddListeners();
        LoadSettings();
    }

    private void CreateInput()
    {
        _actionBack.action.performed += action => ExecuteBackInput();

        _actionAnyButton.action.performed += action => PressAnyButton(action);
        _actionAnyButton.action.Enable();
    }

    private void PressAnyButton(InputAction.CallbackContext action)
    {
        _deviceUtility.DetectDevice(action.control.device);
        _actionAnyButton.action.Disable();

        _canvasHome.Show(false);
        _canvasMain.Show(true, 0.5f);
        _logoImg.rectTransform.DOLocalMoveY(_logoEndY, 1);
        _logoImg.rectTransform.DOSizeDelta(_logoEndSize, 1);

        _eventSystemUtility.SetSelectedGameObject(_firstSelectMain);
        _lastGameObject = _firstSelectMain;
    }

    private void AddListeners()
    {
        _buttonPlay.AddListener(() => Execute(UI_TYPE.Play));
        _buttonOptions.AddListener(() => Execute(UI_TYPE.Options));
        _buttonQuit.AddListener(() => Execute(UI_TYPE.Quit));

        _buttonLanguage.AddListenerHorizontal(() => OptionsLanguage(true), () => OptionsLanguage(false));
        _buttonResolution.AddListenerHorizontal(() => OptionsResolution(true), () => OptionsResolution(false));
        _buttonQuality.AddListenerHorizontal(() => OptionsQuality(true), () => OptionsQuality(false));
        _buttonFullscreen.AddListenerHorizontal(() => OptionsFullscreen(true), () => OptionsFullscreen(false));
        _buttonVsync.AddListenerHorizontal(() => OptionsVsync(true), () => OptionsVsync(false));
        _buttonMasterVolume.AddListenerHorizontal(() => OptionsMasterVolume(true), () => OptionsMasterVolume(false));
        _buttonSoundEffects.AddListenerHorizontal(() => OptionsSoundEffects(true), () => OptionsSoundEffects(false));
        _buttonMusic.AddListenerHorizontal(() => OptionsMusic(true), () => OptionsMusic(false));
        //  _buttonVibration.AddListener();
        _buttonApply.AddListener(() => ExecuteBack(false));
        _buttonBack.AddListener(() => ExecuteBack(true));

        _buttonYes.AddListener(() => ExecuteQuit(true));
        _buttonNo.AddListener(() => ExecuteQuit(false));
    }

    private void LoadSettings()
    {
        _buttonLanguage.SetText(GameData.Instance.GetCurrentLanguage());

        _indexResolution = System.Array.IndexOf(Screen.resolutions, Screen.currentResolution);
        _buttonResolution.SetText(Screen.currentResolution.ToString());

        _indexQuality = QualitySettings.GetQualityLevel();
        _buttonQuality.SetText(QualitySettings.names[_indexQuality]);

        _buttonFullscreen.SetText(Screen.fullScreen.ToString());
        _buttonVsync.SetText(QualitySettings.vSyncCount == 0 ? "false" : "true");

        _buttonMasterVolume.SetText(_indexMasterVolume.ToString());
        _buttonSoundEffects.SetText(_indexSoundEffects.ToString());
        _buttonMusic.SetText(_indexMusic.ToString());
    }

    private void OnEnable()
    {
        EventController.AddListener<UpdateLanguageEvent>(OnUpdateLanguage);
        // EventController.AddListener<MainMenuEvent>(MainMenu);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<UpdateLanguageEvent>(OnUpdateLanguage);
        // EventController.RemoveListener<MainMenuEvent>(MainMenu);
    }

    private void Execute(UI_TYPE type)
    {
        _currentUIType = type;

        switch (type)
        {
            case UI_TYPE.Play:
                _eventSystemUtility.DisableInput(true);
                EventController.TriggerEvent(_changeSceneEvent);
                break;

            case UI_TYPE.Options:
                _canvasMain.Show(false);
                _canvasLogo.Show(false);
                _canvasOptions.Show(true, 0.5f);

                _eventSystemUtility.SetSelectedGameObject(_firstSelectOptions);
                _lastGameObject = _buttonOptions.gameObject;
                break;

            case UI_TYPE.Quit:
                _canvasMain.Show(false);
                _canvasLogo.Show(false);
                _canvasExit.Show(true, 0.5f);

                _eventSystemUtility.SetSelectedGameObject(_firstSelectQuit);
                _lastGameObject = _buttonQuit.gameObject;
                break;

            case UI_TYPE.None:
            default:
                break;
        }
    }

    private void OptionsLanguage(bool isLeft)
    {
        GameData.Instance.SelectNextLanguage(!isLeft);
    }

    private void OptionsResolution(bool isLeft)
    {
        _indexResolution = Utils.GetNextIndex(!isLeft, _indexResolution, Screen.resolutions.Length - 1, false);
        Screen.SetResolution(Screen.resolutions[_indexResolution].width, Screen.resolutions[_indexResolution].height, Screen.fullScreen);

        _buttonResolution.SetText(Screen.resolutions[_indexResolution].ToString());
    }

    private void OptionsQuality(bool isLeft)
    {
        _indexQuality = Utils.GetNextIndex(!isLeft, _indexQuality, QualitySettings.names.Length - 1, false);
        QualitySettings.SetQualityLevel(_indexQuality, true);

        _buttonQuality.SetText(QualitySettings.names[_indexQuality]);
    }

    private void OptionsFullscreen(bool isLeft)
    {
        Screen.fullScreen = isLeft;

        _buttonFullscreen.SetText(isLeft.ToString());
    }

    private void OptionsVsync(bool isLeft)
    {
        QualitySettings.vSyncCount = isLeft ? 1 : 0;

        _buttonVsync.SetText(isLeft.ToString());
    }

    private void OptionsMasterVolume(bool isLeft)
    {
        _indexMasterVolume = Utils.GetNextIndex(!isLeft, _indexMasterVolume, 10, false);
        // TODO Matias: FMOD 

        _buttonMasterVolume.SetText(_indexMasterVolume.ToString());
    }

    private void OptionsSoundEffects(bool isLeft)
    {
        _indexSoundEffects = Utils.GetNextIndex(!isLeft, _indexSoundEffects, 10, false);
        // TODO Matias: FMOD 

        _buttonSoundEffects.SetText(_indexSoundEffects.ToString());
    }

    private void OptionsMusic(bool isLeft)
    {
        _indexMusic = Utils.GetNextIndex(!isLeft, _indexMusic, 10, false);
        // TODO Matias: FMOD 

        _buttonMusic.SetText(_indexMusic.ToString());
    }

    private void ExecuteBackInput()
    {
        switch (_currentUIType)
        {
            case UI_TYPE.Options:
                ExecuteBack(true);
                break;

            case UI_TYPE.Quit:
                ExecuteQuit(false);
                break;

            case UI_TYPE.None:
            default:
                break;
        }

    }

    private void ExecuteBack(bool isBack)
    {
        if (isBack)
        {
            _canvasOptions.Show(false);
            _canvasMain.Show(true, 0.5f);
            _canvasLogo.Show(true, 0.5f);

            _eventSystemUtility.SetSelectedGameObject(_lastGameObject);
            _lastGameObject = _firstSelectOptions;

            _currentUIType = UI_TYPE.None;
        }
        // else
        // {
        //     // TODO Mariano: Apply Settings
        // }
    }

    private void ExecuteQuit(bool isYes)
    {
        if (isYes)
        {
            _eventSystemUtility.DisableInput(true);
            Application.Quit();
        }
        else
        {
            _canvasExit.Show(false);
            _canvasMain.Show(true, 0.5f);
            _canvasLogo.Show(true, 0.5f);

            _eventSystemUtility.SetSelectedGameObject(_lastGameObject);
            _lastGameObject = _firstSelectMain;

            _currentUIType = UI_TYPE.None;
        }
    }

    private void OnUpdateLanguage(UpdateLanguageEvent evt)
    {
        string[] splitLanguage = evt.locale.name.Split('(');

        _buttonLanguage.SetText(splitLanguage[0]);
    }

    private void PlayButtonSound(float parameter)
    {
        _buttonSounds.Play();
        _buttonSounds.EventInstance.setParameterByName(FMODParameters.UI, parameter);
    }

    // public void MainMenu(MainMenuEvent evt)
    // {
    //     switch (evt.menuType)
    //     {
    //         case UI_TYPE.Continue:
    //             Fade();

    //             PlayButtonSound(3);
    //             break;

    //         case UI_TYPE.NewGame:
    //             Fade();

    //             // GameData.Data.isDataLoaded = true;

    //             PlayButtonSound(3);
    //             break;

    //         case UI_TYPE.Options:
    //             creditsPanel.SetActive(false);
    //             optionsPanel.SetActive(true);

    //             PlayButtonSound(1);
    //             break;

    //         case UI_TYPE.Credits:
    //             optionsPanel.SetActive(false);
    //             creditsPanel.SetActive(true);

    //             _menuMusic.EventInstance.setParameterByName(FMODParameters.Credits, 1f);
    //             PlayButtonSound(1);
    //             break;

    //         case UI_TYPE.Back:

    //             DesactivateAll();

    //             PlayButtonSound(0);
    //             break;

    //         case UI_TYPE.Exit:
    //             DesactivateAll();

    //             ShowPopup(true);
    //             PlayButtonSound(3);
    //             break;

    //         case UI_TYPE.YesPopup:
    //             Application.Quit();
    //             PlayButtonSound(3);
    //             break;
    //     }
    // }

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