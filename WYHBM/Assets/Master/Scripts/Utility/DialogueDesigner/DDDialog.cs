using System.Collections;
using DD;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class DDDialog : MonoBehaviour
{
    [SerializeField, ReadOnly] private NPCController _currentNPC = null;
    [SerializeField, ReadOnly] private DIALOG_STATE _dialogState = DIALOG_STATE.Done;
    [SerializeField, ReadOnly] private bool _legacyMode = false;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private WorldConfig _worldConfig = null;
    [SerializeField, ConditionalHide] private LocalizedStringTable _tableDD;
    [SerializeField, ConditionalHide] private InputActionReference _actionCancel = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasUtility;
    [Space]
    [SerializeField, ConditionalHide] private Button _dialogPanelBtn = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _dialogTxt = null;
    [SerializeField, ConditionalHide] private DDButton[] _ddButtons = null;
    [Space]
    [SerializeField, ConditionalHide] private Image _npcImg = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _titleTxt = null;
    [SerializeField, ConditionalHide] private GameObject _continueImg = null;

    // Dialogues
    private bool _isReading;
    private string _currentDialog;
    private bool _canInteract = true;
    private int _lastIndexButton;
    private int _totalVisibleCharacters;
    private int _counter;
    private int _visibleCount;
    private bool _ddError;
    private string _legacyLanguage = "ENG";
    // private PlayerSO _currentPlayer;
    // private string _currentName = "DDUtility";
    // private string _lastName;

    private Coroutine _coroutineWrite;
    private WaitForSeconds _waitStart;
    private WaitForSeconds _waitSpeed;

    private StringTable _stringTableDD;
    private long _nodeTitleParsed;
    private long _nodeConditionParsed;
    private Locale _currentLocale;

    // Events
    private ChangeInputEvent _changeInputEvent;
    private EnableMovementEvent _enableMovementEvent;
    // private ShowInteractionHintEvent _showInteractionHintEvent;
    private EventSystemEvent _eventSystemEvent;

    #region  Dialogue Designer

    private ShowMessageNode _showMessageNode;
    private ShowMessageNodeChoice _choiceNode;

    public enum DDExecuteScript
    {
        NewQuest,
        UpdateQuest,
        CompleteQuest,
        Finish,
    }

    public enum DDEvaluateCondition
    {
        FirstTime,
        Finished,
        CheckQuest,
        HaveQuest,
    }

    private Dialogue _loadedDialogue;
    private DialoguePlayer _dialoguePlayer;
    private IDialogueable _dialogueable;

    #endregion

    private void Start()
    {
        _waitStart = new WaitForSeconds(_worldConfig.textTimeStart);
        _waitSpeed = new WaitForSeconds(_worldConfig.textTimeSpeed);

        _changeInputEvent = new ChangeInputEvent();
        _changeInputEvent.enable = true;

        _enableMovementEvent = new EnableMovementEvent();
        _enableMovementEvent.canMove = true;

        // _showInteractionHintEvent = new ShowInteractionHintEvent();;
        _eventSystemEvent = new EventSystemEvent();

        for (int i = 0; i < _ddButtons.Length; i++)
        {
            int index = i;
            _ddButtons[i].AddListener(() => SelectNode(index));
        }

        _dialogPanelBtn.onClick.AddListener(Select);

        // // if you want to handle a particular dialogue differently, you can use these instead
        // //m_dialoguePlayer.OverrideOnShowMessage += OnShowMessageSpecial;
        // //m_dialoguePlayer.OverrideOnEvaluateCondition += OnEvaluateConditionSpecial;
        // //m_dialoguePlayer.OverrideOnExecuteScript += OnExecuteScriptSpecial;
    }

    private void OnEnable()
    {
        _tableDD.TableChanged += LoadStrings;

        EventController.AddListener<DialogDesignerEvent>(OnEnableDialog);
        EventController.AddListener<UpdateLanguageEvent>(OnUpdateLanguage);

        DialoguePlayer.GlobalOnShowMessage += OnShowMessage;
        DialoguePlayer.GlobalOnEvaluateCondition += OnEvaluateCondition;
        DialoguePlayer.GlobalOnExecuteScript += OnExecuteScript;
    }

    private void OnDisable()
    {
        _tableDD.TableChanged -= LoadStrings;

        EventController.RemoveListener<DialogDesignerEvent>(OnEnableDialog);
        EventController.RemoveListener<UpdateLanguageEvent>(OnUpdateLanguage);

        DialoguePlayer.GlobalOnShowMessage -= OnShowMessage;
        DialoguePlayer.GlobalOnEvaluateCondition -= OnEvaluateCondition;
        DialoguePlayer.GlobalOnExecuteScript -= OnExecuteScript;
    }

    public void SetInteraction(bool canInteract)
    {
        _canInteract = canInteract;
    }

    private void LoadStrings(StringTable stringTable)
    {
        _stringTableDD = stringTable;
    }

    private void OnEnableDialog(DialogDesignerEvent evt)
    {
        _legacyMode = GameData.Instance.DevDDLegacyMode;

        if (evt.enable)
        {
            _currentNPC = evt.npc;
            _loadedDialogue = Dialogue.FromAsset(evt.dialogue);
            _dialogueable = evt.dialogueable;
            // _currentPlayer = evt.playerData;
            EventController.AddListener<InteractionEvent>(OnInteractionDialog);
            _actionCancel.action.performed += Back;
        }
        else
        {
            _currentNPC = null;
            _loadedDialogue = null;
            _dialogueable = null;
            // _currentPlayer = null;
            EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);
            _actionCancel.action.performed -= Back;
        }
    }

    private void OnInteractionDialog(InteractionEvent evt)
    {
        if (!evt.isStart && !_canInteract)return;

        _dialoguePlayer = new DialoguePlayer(_loadedDialogue);
        _dialoguePlayer.OnDialogueEnded += OnDialogueEnded;

        UpdateNode(_dialoguePlayer.CurrentNode as ShowMessageNode);

        _npcImg.sprite = _currentNPC.Data.GetIcon(EMOTION.None);
        _titleTxt.text = _currentNPC.Data.Name;

        _currentDialog = "";
        _dialogTxt.text = "";

        _dialogState = DIALOG_STATE.Ready;

        _canvasUtility.Show(true);

        _dialoguePlayer.Play();

        // _showInteractionHintEvent.show = false;
        // EventController.TriggerEvent(_showInteractionHintEvent);
    }

    private void UpdateNode(ShowMessageNode showMessageNode)
    {
        _showMessageNode = showMessageNode;

        if (_showMessageNode != null)
        {
            _choiceNode = _showMessageNode as ShowMessageNodeChoice;
        }

    }

    private void OnDialogueEnded(DialoguePlayer sender)
    {
        _dialoguePlayer.OnDialogueEnded -= OnDialogueEnded;
        _dialoguePlayer = null;

        _showMessageNode = null;
        _choiceNode = null;

        _currentDialog = "";
        _dialogTxt.text = "";

        _canvasUtility.Show(false);

        _eventSystemEvent.objectSelected = null;
        EventController.TriggerEvent(_eventSystemEvent);

        // _showInteractionHintEvent.show = true;
        // EventController.TriggerEvent(_showInteractionHintEvent);
        EventController.TriggerEvent(_changeInputEvent);
        EventController.TriggerEvent(_enableMovementEvent);
    }

    private void OnShowMessage(DialoguePlayer sender, ShowMessageNode node)
    {
        if (_ddError)
        {
            OnDialogueEnded(sender);
            _ddError = false;
            return;
        }

        UpdateString(node);

        _dialogTxt.text = _currentDialog;

        UpdateNode(node);

        _lastIndexButton = 0;
        _continueImg.SetActive(false);

        for (int i = 0; i < _ddButtons.Length; i++)_ddButtons[i].Hide();

        _dialogState = DIALOG_STATE.Ready;

        Play();
    }

    private void UpdateString(ShowMessageNode node)
    {
        if (_legacyMode)
        {
            _currentDialog = node.GetText(_legacyLanguage);
        }
        else
        {
            if (!long.TryParse(node.Title, out _nodeTitleParsed))
            {
                _currentDialog = string.Format("ERROR: {0}", node.Title);
                Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse \"{node.Title}\"", gameObject);
                _ddError = true;
            }
            else
            {
                // LocalizationSettings.StringDatabase.GetLocalizedStringAsync(_tableDDNew, _currentLocale).Completed += (AsyncOperationHandle<string> op) =>
                // {
                //     _currentDialog = op.Result;
                // };

                _currentDialog = _stringTableDD.GetEntry(_nodeTitleParsed).GetLocalizedString();
            }

        }
    }

    private void Play()
    {
        switch (_dialogState)
        {
            case DIALOG_STATE.Ready:
                _dialogTxt.maxVisibleCharacters = 0;
                _counter = 0;
                _visibleCount = 0;

                _coroutineWrite = StartCoroutine(WriteText());
                break;

            case DIALOG_STATE.InProgress:
                CompleteText();
                break;

            case DIALOG_STATE.Done:
                break;
        }
    }

    private IEnumerator WriteText()
    {
        _dialogState = DIALOG_STATE.InProgress;

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

        if (_choiceNode != null)
        {
            if (_legacyMode)
            {
                for (int i = 0; i < _choiceNode.Choices.Length; i++)
                {
                    _ddButtons[i].Show(_choiceNode.GetChoiceText(i, _legacyLanguage));
                    _lastIndexButton = i;
                }
            }
            else
            {
                for (int i = 0; i < _choiceNode.Choices.Length; i++)
                {
                    _nodeConditionParsed = 0;

                    if (!long.TryParse(_choiceNode.Choices[i].Condition, out _nodeConditionParsed))
                    {
                        _currentDialog = string.Format("ERROR: {0}", _choiceNode.Choices[i].Condition);
                        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse \"{_choiceNode.Choices[i].Condition}\"", gameObject);
                        _ddError = true;
                        break;

                    }
                    else
                    {
                        _ddButtons[i].Show(_stringTableDD.GetEntry(_nodeConditionParsed).GetLocalizedString());
                        _lastIndexButton = i;
                    }
                }

            }

            _ddButtons[_lastIndexButton].SetInput();
            _ddButtons[0].Select();

            _eventSystemEvent.objectSelected = _ddButtons[0].gameObject;
        }
        else
        {
            _dialogPanelBtn.Select();

            _eventSystemEvent.objectSelected = _dialogPanelBtn.gameObject;

            _continueImg.SetActive(true);
        }

        EventController.TriggerEvent(_eventSystemEvent);
    }

    private void SelectNode(int index)
    {
        switch (_dialogState)
        {
            case DIALOG_STATE.Ready:
                break;

            case DIALOG_STATE.InProgress:
                CompleteText();
                break;

            case DIALOG_STATE.Done:
                _dialoguePlayer.AdvanceMessage(index);
                break;
        }

    }

    private void Select()
    {
        if (_dialoguePlayer == null || _showMessageNode == null)return;

        if (_choiceNode != null)
        {
            Play();
        }
        else
        {
            SelectNode(0);
        }
    }

    private void Back(InputAction.CallbackContext context)
    {
        Back();
    }

    private void Back()
    {
        if (_dialoguePlayer == null || _showMessageNode == null || _choiceNode == null)return;

        SelectNode(_lastIndexButton);
    }

    private void OnUpdateLanguage(UpdateLanguageEvent evt)
    {
        _currentLocale = evt.locale;

        if (_dialoguePlayer == null || _showMessageNode == null)return;

        StartCoroutine(UpdateLanguage());
    }

    private IEnumerator UpdateLanguage()
    {
        UpdateString(_showMessageNode);

        // _dialogTxt.alpha = 0;

        _dialogTxt.text = _currentDialog;

        yield return new WaitForSeconds(0.1f);

        // _dialogTxt.alpha = 1;

        _totalVisibleCharacters = _dialogTxt.textInfo.characterCount;

        _dialogTxt.maxVisibleCharacters = _totalVisibleCharacters;

        if (_choiceNode != null)
        {
            if (_legacyMode)
            {
                for (int i = 0; i < _choiceNode.Choices.Length; i++)
                {
                    _ddButtons[i].UpdateText(_choiceNode.GetChoiceText(i, _legacyLanguage));
                }
            }
            else
            {
                for (int i = 0; i < _choiceNode.Choices.Length; i++)
                {
                    _nodeConditionParsed = 0;

                    if (!long.TryParse(_choiceNode.Choices[i].Condition, out _nodeConditionParsed))
                    {
                        _currentDialog = string.Format("ERROR: {0}", _choiceNode.Choices[i].Condition);
                        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse \"{_choiceNode.Choices[i].Condition}\"", gameObject);
                        _ddError = true;
                        break;
                    }
                    else
                    {
                        _ddButtons[i].UpdateText(_stringTableDD.GetEntry(_nodeConditionParsed).GetLocalizedString());
                    }
                }

            }
        }

    }

    private bool OnEvaluateCondition(DialoguePlayer sender, string script)
    {
        if (System.Enum.TryParse<DDEvaluateCondition>(script, out DDEvaluateCondition scriptParsed))
        {
            switch (scriptParsed)
            {
                case DDEvaluateCondition.FirstTime:
                    return _dialogueable.DDFirstTime();

                case DDEvaluateCondition.Finished:
                    return _dialogueable.DDFinished();

                case DDEvaluateCondition.CheckQuest:
                    return _dialogueable.DDCheckQuest();

                case DDEvaluateCondition.HaveQuest:
                    return _dialogueable.DDHaveQuest();

                default:
                    Debug.LogError($"<color=red><b>[ERROR]</b></color> No DDEvaluateCondition", gameObject);
                    return false;
            }
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse DDEvaluateCondition \"{script}\"", gameObject);
        return false;
    }

    private void OnExecuteScript(DialoguePlayer sender, string script)
    {
        if (script.Contains(DDParameters.Emotion))
        {
            string[] emotionArray = script.Split('=');

            if (int.TryParse(emotionArray[1], out int emotionId))
            {
                _npcImg.sprite = _currentNPC.Data.GetIcon((EMOTION)emotionId);
                // _npcImg.sprite = _currentCharacter.GetIcon((EMOTION)emotionId);
            }
            else
            {
                Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse DDExecuteScript \"{script}\"", gameObject);
            }

            return;
        }

        if (System.Enum.TryParse<DDExecuteScript>(script, out DDExecuteScript scriptParsed))
        {
            switch (scriptParsed)
            {
                case DDExecuteScript.NewQuest:
                    _dialogueable.DDQuest(QUEST_STATE.New);
                    break;

                case DDExecuteScript.UpdateQuest:
                    _dialogueable.DDQuest(QUEST_STATE.Update);
                    break;

                case DDExecuteScript.CompleteQuest:
                    _dialogueable.DDQuest(QUEST_STATE.Complete);
                    break;

                case DDExecuteScript.Finish:
                    _dialogueable.DDFinish();
                    break;

                default:
                    Debug.LogError($"<color=red><b>[ERROR]</b></color> No DDExecuteScript", gameObject);
                    break;
            }

            return;
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse DDExecuteScript \"{script}\"", gameObject);
        }
    }

}