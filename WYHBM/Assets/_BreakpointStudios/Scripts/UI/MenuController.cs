using DG.Tweening;
using Events;
using FMOD.Studio;
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
    [SerializeField, ConditionalHide] private FMODConfig _FMODConfig = null;
    [SerializeField, ConditionalHide] private Image _logoImg = null;
    [SerializeField, ConditionalHide] private OptionsController _optionsController = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasLogo = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionAnyButton = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionBack = null;
    [SerializeField, ConditionalHide] private DeviceConfig _deviceConfig = null;
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
    [SerializeField, ConditionalHide] private GameObject _firstSelectQuit = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonYes = null;
    [SerializeField, ConditionalHide] private ButtonUI _buttonNo = null;

    private ChangeSceneEvent _changeSceneEvent;

    private FadeEvent _fadeEvent;
    private GameObject _lastPanel;

    private Vector2 _logoStartSize = new Vector2(600, 600);
    // private float _logoStartY = 50f;

    private Vector2 _logoEndSize = new Vector2(475, 475);
    private float _logoEndY = 150f;
    EventInstance menuMusic;
    EventInstance selectSound;
    EventInstance backSound;

    // private bool _isDataLoaded;
    // public bool IsDataLoaded { get { return _isDataLoaded; } }

    private void Awake()
    {
        SetVersion();
        _deviceConfig.UpdateDictionary();
    }

    private void Start()
    {
        menuMusic = FMODUnity.RuntimeManager.CreateInstance(_FMODConfig.music);
        selectSound = FMODUnity.RuntimeManager.CreateInstance(_FMODConfig.select);
        backSound = FMODUnity.RuntimeManager.CreateInstance(_FMODConfig.back);

        menuMusic.start();
        _changeSceneEvent = new ChangeSceneEvent();
        _changeSceneEvent.load = true;
        _changeSceneEvent.useEnableMovementEvent = false;
        _changeSceneEvent.isLoadAdditive = false;
        _changeSceneEvent.sceneData = sceneData;
        _changeSceneEvent.instantFade = false;

        AddListeners();

        if (GameData.Instance.HomeUsed)
        {
            _canvasHome.ShowInstant(false);
            _canvasMain.ShowInstant(true);

            _logoImg.rectTransform.DOLocalMoveY(_logoEndY, 0);
            _logoImg.rectTransform.DOSizeDelta(_logoEndSize, 0);

            EventSystemUtility.Instance.SetSelectedGameObject(_firstSelectMain);
            _lastGameObject = _firstSelectMain;

            EventSystemUtility.Instance.DisableInput(false);
        }
        else
        {
            GameData.Instance.HomeUsed = true;

            _actionAnyButton.action.performed += action => PressAnyButton(action);
            _actionAnyButton.action.Enable();
        }

        _actionBack.action.performed += action => ExecuteBackInput();
    }

    private void PressAnyButton(InputAction.CallbackContext action)
    {
        selectSound.start();

        GameData.Instance.DetectDevice(action.control.device);
        _actionAnyButton.action.Disable();

        _canvasHome.Show(false);
        _canvasMain.Show(true, 0.5f);
        _logoImg.rectTransform.DOLocalMoveY(_logoEndY, 1);
        _logoImg.rectTransform.DOSizeDelta(_logoEndSize, 1);

        EventSystemUtility.Instance.SetSelectedGameObject(_firstSelectMain);
        _lastGameObject = _firstSelectMain;
    }

    private void AddListeners()
    {
        _buttonPlay.AddListener(() => Execute(UI_TYPE.Play));
        _buttonOptions.AddListener(() => Execute(UI_TYPE.Options));
        _buttonQuit.AddListener(() => Execute(UI_TYPE.Quit));

        _buttonYes.AddListener(() => ExecuteQuit(true));
        _buttonNo.AddListener(() => ExecuteQuit(false));

        _optionsController.AddListeners(() => ExecuteBack(false), () => ExecuteBack(true));
    }

    private void Execute(UI_TYPE type)
    {
        _currentUIType = type;

        switch (type)
        {
            case UI_TYPE.Play:
                selectSound.start();

                EventSystemUtility.Instance.DisableInput(true);
                EventController.TriggerEvent(_changeSceneEvent);

                menuMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                break;

            case UI_TYPE.Options:
                selectSound.start();

                _canvasMain.Show(false);
                _canvasLogo.Show(false);
                _canvasOptions.Show(true, 0.5f);

                EventSystemUtility.Instance.SetSelectedGameObject(_optionsController.FirstSelectOptions);
                _lastGameObject = _buttonOptions.gameObject;
                break;

            case UI_TYPE.Quit:
                selectSound.start();

                _canvasMain.Show(false);
                _canvasLogo.Show(false);
                _canvasExit.Show(true, 0.5f);

                EventSystemUtility.Instance.SetSelectedGameObject(_firstSelectQuit);
                _lastGameObject = _buttonQuit.gameObject;
                break;

            case UI_TYPE.None:
            default:
                break;
        }
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
            backSound.start();

            _canvasOptions.Show(false);
            _canvasMain.Show(true, 0.5f);
            _canvasLogo.Show(true, 0.5f);

            EventSystemUtility.Instance.SetSelectedGameObject(_lastGameObject);
            _lastGameObject = _optionsController.FirstSelectOptions;

            _currentUIType = UI_TYPE.None;

            _optionsController.SaveSettings();
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
            selectSound.start();

            EventSystemUtility.Instance.DisableInput(true);
            Application.Quit();
        }
        else
        {
            backSound.start();
            _canvasExit.Show(false);
            _canvasMain.Show(true, 0.5f);
            _canvasLogo.Show(true, 0.5f);

            EventSystemUtility.Instance.SetSelectedGameObject(_lastGameObject);
            _lastGameObject = _firstSelectMain;

            _currentUIType = UI_TYPE.None;
        }
    }

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