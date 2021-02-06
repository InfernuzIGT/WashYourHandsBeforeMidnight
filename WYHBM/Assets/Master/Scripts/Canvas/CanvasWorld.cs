using System.Collections.Generic;
using DG.Tweening;
using Events;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupUtility))]
public class CanvasWorld : MonoBehaviour
{
    [Header("General")]
    [SerializeField, ReadOnly] private UI_TYPE _currentUIType = UI_TYPE.Play;
    [SerializeField, ReadOnly] private GameObject _lastGameObject = null;
    [SerializeField] private SceneSO sceneData = null;

    // [Header("FMOD")]
    // [SerializeField] private StudioEventEmitter _buttonSounds;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private CanvasGroupUtility _panelPause;
    [SerializeField, ConditionalHide] private OptionsController _optionsController = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionBack = null;
    [Space]
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

    [Header("DEPRECATED")]
    public Popup popup;
    public Image progressImg;
    [Space]
    public GameObject system;
    public GameObject systemOptions;
    [Space]
    public GameObject diary;
    public Transform diaryTitleContainer;
    public Transform diaryDescriptionContainer;
    [Space]
    public GameObject inventory;
    public ItemDescription itemDescription;
    public Transform itemParents;
    public Button buttonLeft;
    public Button buttonRight;

    // Inventory
    // private int _lastSlot = 0;

    // Diary
    private Dictionary<QuestSO, QuestTitle> dicQuestTitle;
    private Dictionary<QuestSO, QuestDescription> dicQuestDescription;
    private GameObject _currentQuestSelected;
    private bool _system;
    private bool _inventory;
    private bool _diary;
    private bool _isComplete;
    private bool _canInteract;

    private Tween _txtAnimation;
    private Canvas _canvas;
    private WaitForSeconds _waitStart;
    private WaitForSeconds _waitSpace;
    private WaitForSeconds _waitDeactivateUI;

    private ChangeSceneEvent _changeSceneEvent;
    private PauseEvent _pauseEvent;

    private CanvasGroupUtility _canvasUtility;

    private void Awake()
    {
        _canvasUtility = GetComponent<CanvasGroupUtility>();
    }

    private void Start()
    {
        dicQuestTitle = new Dictionary<QuestSO, QuestTitle>();
        dicQuestDescription = new Dictionary<QuestSO, QuestDescription>();

        _pauseEvent = new PauseEvent();

        _changeSceneEvent = new ChangeSceneEvent();
        _changeSceneEvent.load = true;
        _changeSceneEvent.useEnableMovementEvent = false;
        _changeSceneEvent.isLoadAdditive = false;
        _changeSceneEvent.sceneData = sceneData;
        _changeSceneEvent.instantFade = false;

        AddListeners();
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

    private void OnEnable()
    {
        EventController.AddListener<PauseEvent>(OnPause);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<PauseEvent>(OnPause);
    }

    private void Execute(UI_TYPE type)
    {
        if (!_canInteract)return;

        CanInteract(false);

        _currentUIType = type;

        switch (type)
        {
            case UI_TYPE.Play:
                // _buttonSounds.EventInstance.setParameterByName("UI", 3f);
                _pauseEvent.pauseType = PAUSE_TYPE.PauseMenu;
                EventController.TriggerEvent(_pauseEvent);
                break;

            case UI_TYPE.Options:
                _canvasMain.Show(false);
                _canvasOptions.Show(true, 0.5f);

                EventSystemUtility.Instance.SetSelectedGameObject(_optionsController.FirstSelectOptions);
                _lastGameObject = _buttonOptions.gameObject;
                break;

            case UI_TYPE.Quit:
                _canvasMain.Show(false);
                _canvasExit.Show(true, 0.5f);

                EventSystemUtility.Instance.SetSelectedGameObject(_firstSelectQuit);
                _lastGameObject = _buttonQuit.gameObject;
                break;

            case UI_TYPE.None:
            default:
                break;
        }
    }

    private void ExecuteBackInput(InputAction.CallbackContext context)
    {
        if (!_canInteract)return;

        switch (_currentUIType)
        {
            case UI_TYPE.Play:
                _pauseEvent.pauseType = PAUSE_TYPE.PauseMenu;
                EventController.TriggerEvent(_pauseEvent);
                break;

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
        CanInteract(false);

        if (isBack)
        {
            _currentUIType = UI_TYPE.Play;

            _canvasOptions.Show(false);
            _canvasMain.Show(true, 0.5f);

            EventSystemUtility.Instance.SetSelectedGameObject(_lastGameObject);
            _lastGameObject = _optionsController.FirstSelectOptions;

            _optionsController.SaveSettings();
        }
        // else
        // {
        //     // TODO Mariano: Apply Settings
        // }
    }

    public void CanInteract(bool canInteract)
    {
        _canInteract = canInteract;
        _optionsController.CanInteract = canInteract;
    }

    private void ExecuteQuit(bool isYes)
    {
        CanInteract(false);

        if (isYes)
        {
            // _buttonSounds.EventInstance.setParameterByName("UI", 0f);

            EventSystemUtility.Instance.DisableInput(true);
            EventController.TriggerEvent(_changeSceneEvent);
        }
        else
        {
            _canvasExit.Show(false);
            _canvasMain.Show(true, 0.5f);

            EventSystemUtility.Instance.SetSelectedGameObject(_lastGameObject);
            _lastGameObject = _firstSelectMain;

            _currentUIType = UI_TYPE.Play;
        }
    }

    private void OnPause(PauseEvent evt)
    {
        _panelPause.ShowInstant(evt.isPaused);

        EventSystemUtility.Instance.SetSelectedGameObject(_firstSelectMain);
        _lastGameObject = _firstSelectMain;

        _canInteract = evt.isPaused;

        if (evt.isPaused)
        {
            _actionBack.action.performed += ExecuteBackInput;
        }
        else
        {
            _actionBack.action.performed -= ExecuteBackInput;
        }

    }

    public void Show(bool isShowing)
    {
        _canvasUtility.ShowInstant(isShowing);
    }

    #region 

    #endregion

    #region Quest

    public void SetQuest(QuestSO data)
    {
        if (data == null)
        {
            return;
        }

        // // ShowPopup(string.Format(GameData.Instance.textConfig.popupNewQuest, data.title));

        // // Create Quest Title
        // QuestTitle questTitle = Instantiate(GameData.Instance.worldConfig.questTitlePrefab, diaryTitleContainer);
        // questTitle.Init(data);
        // dicQuestTitle.Add(data, questTitle);

        // // Create Quest Description
        // QuestDescription questDescription = Instantiate(GameData.Instance.worldConfig.questDescriptionPrefab, diaryDescriptionContainer);
        // questDescription.Init(data);
        // dicQuestDescription.Add(data, questDescription);
        // SelectQuest(data);

        // GameManager.Instance.CurrentQuestData.state = QUEST_STATE.InProgress;
    }

    public void ReloadQuest(QuestSO data)
    {
        // // Create Quest Title
        // QuestTitle questTitle = Instantiate(GameData.Instance.worldConfig.questTitlePrefab, diaryTitleContainer);
        // questTitle.Init(data);
        // dicQuestTitle.Add(data, questTitle);

        // // Create Quest Description
        // QuestDescription questDescription = Instantiate(GameData.Instance.worldConfig.questDescriptionPrefab, diaryDescriptionContainer);
        // questDescription.Init(data);
        // dicQuestDescription.Add(data, questDescription);

        // Debug.Log($"<b> Quest: {data.title} </b>");
    }

    public void SelectQuest(QuestSO data)
    {
        Debug.Log($"<b> Selected </b>");
        _currentQuestSelected.gameObject.SetActive(false);
        _currentQuestSelected = dicQuestDescription[data].gameObject;
        _currentQuestSelected.gameObject.SetActive(true);
    }

    public void UpdateQuest(QuestSO data, int progress)
    {
        dicQuestDescription[data].UpdateObjetives();

        // if (progress == data.objetives.Length)
        // {
        //     ShowPopup(string.Format(GameData.Instance.textConfig.popupCompleted, data.title));
        //     dicQuestTitle[data].Complete();

        //     GameManager.Instance.CurrentQuestData.state = QUEST_STATE.Completed;
        // }
        // else
        // {
        //     ShowPopup(string.Format(GameData.Instance.textConfig.popupNewObjetive, data.objetives[progress]));
        // }
    }

    #endregion

    public void ShowPopup(string text, bool canRepeat = true)
    {
        if (!canRepeat && popup.gameObject.activeSelf)
        {
            return;
        }

        popup.gameObject.SetActive(false);
        popup.SetTitle(text);
        popup.gameObject.SetActive(true);
    }

}