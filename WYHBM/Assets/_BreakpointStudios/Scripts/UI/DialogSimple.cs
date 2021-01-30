using System.Collections;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class DialogSimple : MonoBehaviour
{
    [SerializeField, ReadOnly] private DIALOG_STATE _dialogState = DIALOG_STATE.Ready;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private WorldConfig _worldConfig = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionCancel = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasUtility;
    [Space]
    [SerializeField, ConditionalHide] private Button _dialogPanelBtn = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _dialogTxt = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringEvent = null;
    [SerializeField, ConditionalHide] private GameObject _continueImg = null;

    // Dialogues
    private bool _isReading;
    private int _totalVisibleCharacters;
    private int _counter;
    private int _visibleCount;
    private string _objectName;

    private Coroutine _coroutineWrite;
    private WaitForSeconds _waitStart;
    private WaitForSeconds _waitSpeed;

    // Events
    private ChangeInputEvent _changeInputEvent;
    private EnableMovementEvent _enableMovementEvent;
    private QuestEvent _questEvent;
    private EventSystemEvent _eventSystemEvent;

    private IDialogueable _dialogueable;

    private void Start()
    {
        _canvasUtility = GetComponent<CanvasGroupUtility>();

        _waitStart = new WaitForSeconds(_worldConfig.textTimeStart);
        _waitSpeed = new WaitForSeconds(_worldConfig.textTimeSpeed);

        _changeInputEvent = new ChangeInputEvent();
        _changeInputEvent.enable = true;

        _enableMovementEvent = new EnableMovementEvent();
        _eventSystemEvent = new EventSystemEvent();

        _questEvent = new QuestEvent();

        _dialogPanelBtn.onClick.AddListener(Select);
    }

    private void OnEnable()
    {
        EventController.AddListener<DialogSimpleEvent>(OnEnableDialog);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<DialogSimpleEvent>(OnEnableDialog);
    }

    private void OnEnableDialog(DialogSimpleEvent evt)
    {
        if (evt.enable)
        {
            _questEvent.data = evt.questData;
            _questEvent.state = evt.questState;
            _objectName = evt.objectName;
            _localizeStringEvent.StringReference = evt.localizedString;
            _localizeStringEvent.OnUpdateString.Invoke(_dialogTxt.text);

            EventController.AddListener<InteractionEvent>(OnInteractionDialog);
            _actionCancel.action.performed += Select;

        }
        else
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);
            _actionCancel.action.performed -= Select;
        }
    }

    private void OnInteractionDialog(InteractionEvent evt)
    {
        if (!evt.isStart)return;

        Select();
    }

    private void Select(InputAction.CallbackContext context)
    {
        Select();
    }

    private void Select()
    {
        switch (_dialogState)
        {
            case DIALOG_STATE.Ready:
                Play();
                break;

            case DIALOG_STATE.InProgress:
                CompleteText();
                break;

            case DIALOG_STATE.Done:
                Stop();
                break;
        }
    }

    private void Play()
    {
        _dialogState = DIALOG_STATE.InProgress;

        _enableMovementEvent.canMove = false;
        EventController.TriggerEvent(_enableMovementEvent);

        _canvasUtility.Show(true);

        _dialogTxt.maxVisibleCharacters = 0;
        _counter = 0;
        _visibleCount = 0;

        _coroutineWrite = StartCoroutine(WriteText());
    }

    private void Stop()
    {
        _dialogState = DIALOG_STATE.Locked;

        _canvasUtility.Show(false);

        EventController.TriggerEvent(_changeInputEvent);

        _eventSystemEvent.objectSelected = null;
        EventController.TriggerEvent(_eventSystemEvent);

        _enableMovementEvent.canMove = true;
        EventController.TriggerEvent(_enableMovementEvent);

        TriggerQuest();
    }

    private void TriggerQuest()
    {
        if (_questEvent.data == null)return;

        if (GameData.Instance.CheckAndWriteID(string.Format(DDParameters.FormatQuadruple, _questEvent.data.name, _questEvent.state.ToString(), DDParameters.SimpleQuest, _objectName)))return;

        EventController.TriggerEvent(_questEvent);
    }

    private IEnumerator WriteText()
    {

        _isReading = true;

        yield return _waitStart;

        _isReading = false;

        _totalVisibleCharacters = _dialogTxt.textInfo.characterCount;

        while (_visibleCount < _totalVisibleCharacters)
        {
            _visibleCount = _counter % (_totalVisibleCharacters + 1);

            _dialogTxt.maxVisibleCharacters = _visibleCount;

            _counter++;

            yield return _waitSpeed;
        }

        CompleteText();
    }

    private void CompleteText()
    {
        if (_isReading)return;

        _dialogState = DIALOG_STATE.Done;

        StopCoroutine(_coroutineWrite);

        _dialogTxt.maxVisibleCharacters = _totalVisibleCharacters;

        _dialogPanelBtn.Select();

        _eventSystemEvent.objectSelected = _dialogPanelBtn.gameObject;
        EventController.TriggerEvent(_eventSystemEvent);

        _continueImg.SetActive(true);
    }

    public void EnableInteraction()
    {
        _dialogState = DIALOG_STATE.Ready;
    }

}