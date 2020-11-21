using System.Collections;
using DD;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DDUtility : MonoBehaviour
{
    public enum DIALOG_STATE
    {
        Ready = 0,
        InProgress = 1,
        Done = 2
    }

    [SerializeField, ReadOnly] private NPCSO _currentNPC = null;
    [SerializeField, ReadOnly] private string _currentLanguage = "";
    [SerializeField, ReadOnly] private DIALOG_STATE _dialogState = DIALOG_STATE.Done;

    [Header("Settings")]
    [SerializeField] private float messageLifetime = 3f;
    [SerializeField] private float timeStart = 0.5f;
    [SerializeField] private float timeSpace = 1f;

    [Header("References")]
    [SerializeField] private Button _dialogPanelBtn = null;
    [SerializeField] private DDButton[] _ddButtons = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _dialogTxt = null;
    [SerializeField] private Image _npcImg = null;
    [SerializeField] private TextMeshProUGUI _npcTxt = null;
    [SerializeField] private GameObject _continueImg = null;

    // Dialogues
    private char _charSpace = '*';
    private bool _isReading;
    private string _currentDialog;
    private string _textSkip;
    private Coroutine _coroutineWrite;
    private bool _canInteract = true;
    private int _lastIndexButton;

    private WaitForSeconds _waitStart;
    private WaitForSeconds _waitSpace;
    private WaitForSeconds _waitDeactivateUI;

    private CanvasGroupUtility _canvasUtility;

    // Events
    private ChangeInputEvent _changeInputEvent;
    private EnableMovementEvent _enableMovementEvent;
    private ShowInteractionHintEvent _showInteractionHintEvent;
    private EventSystemEvent _eventSystemEvent;

    #region  Dialogue Designer

    private ShowMessageNode _showMessageNode;
    private ShowMessageNodeChoice _choiceNode;

    private const string emotion = "emotion";

    public enum DDExecuteScript
    {
        GiveReward
    }

    public enum DDEvaluateCondition
    {
        HaveAmount
    }

    private Dialogue _loadedDialogue;
    private DialoguePlayer _dialoguePlayer;

    #endregion

    private void Start()
    {
        _canvasUtility = GetComponent<CanvasGroupUtility>();

        _waitStart = new WaitForSeconds(timeStart);
        _waitSpace = new WaitForSeconds(timeSpace);
        _waitDeactivateUI = new WaitForSeconds(messageLifetime);

        _changeInputEvent = new ChangeInputEvent();
        _changeInputEvent.enable = true;

        _enableMovementEvent = new EnableMovementEvent();
        _enableMovementEvent.canMove = true;

        _showInteractionHintEvent = new ShowInteractionHintEvent();;
        _eventSystemEvent = new EventSystemEvent();

        _currentLanguage = GameData.Instance.GetLanguage();

        for (int i = 0; i < _ddButtons.Length; i++)
        {
            int index = i;
            _ddButtons[i].AddListener(() => SelectNode(index));
        }

        // // if you want to handle a particular dialogue differently, you can use these instead
        // //m_dialoguePlayer.OverrideOnShowMessage += OnShowMessageSpecial;
        // //m_dialoguePlayer.OverrideOnEvaluateCondition += OnEvaluateConditionSpecial;
        // //m_dialoguePlayer.OverrideOnExecuteScript += OnExecuteScriptSpecial;
    }

    private void OnEnable()
    {
        EventController.AddListener<EnableDialogEvent>(OnEnableDialog);

        DialoguePlayer.GlobalOnShowMessage += OnShowMessage;
        DialoguePlayer.GlobalOnEvaluateCondition += OnEvaluateCondition;
        DialoguePlayer.GlobalOnExecuteScript += OnExecuteScript;
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);

        DialoguePlayer.GlobalOnShowMessage -= OnShowMessage;
        DialoguePlayer.GlobalOnEvaluateCondition -= OnEvaluateCondition;
        DialoguePlayer.GlobalOnExecuteScript -= OnExecuteScript;
    }

    public void SetInteraction(bool canInteract)
    {
        _canInteract = canInteract;
    }

    private void OnEnableDialog(EnableDialogEvent evt)
    {
        if (evt.enable)
        {
            _currentNPC = evt.npc;
            _loadedDialogue = Dialogue.FromAsset(evt.npc.dialogDD);
            EventController.AddListener<InteractionEvent>(OnInteractionDialog);
        }
        else
        {
            _currentNPC = null;
            _loadedDialogue = null;
            EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);
        }
    }

    private void OnInteractionDialog(InteractionEvent evt)
    {
        if (!evt.isStart && !_canInteract)return;

        _dialoguePlayer = new DialoguePlayer(_loadedDialogue);
        _dialoguePlayer.OnDialogueEnded += OnDialogueEnded;

        UpdateNode(_dialoguePlayer.CurrentNode as ShowMessageNode);

        _npcImg.sprite = _currentNPC.GetIcon(EMOTION.None);
        _npcTxt.text = _currentNPC.name;

        _currentDialog = "";
        _dialogTxt.text = "";

        _canvasUtility.Show(true);

        _dialoguePlayer.Play();

        _showInteractionHintEvent.show = false;
        EventController.TriggerEvent(_showInteractionHintEvent);
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

        _showInteractionHintEvent.show = true;
        EventController.TriggerEvent(_showInteractionHintEvent);
        EventController.TriggerEvent(_changeInputEvent);
        EventController.TriggerEvent(_enableMovementEvent);
    }

    private void OnShowMessage(DialoguePlayer sender, ShowMessageNode node)
    {
        _currentDialog = node.GetText(_currentLanguage);

        UpdateNode(node);

        _lastIndexButton = 0;
        _continueImg.SetActive(false);

        for (int i = 0; i < _ddButtons.Length; i++)_ddButtons[i].Hide();

        _dialogState = DIALOG_STATE.Ready;

        Play();
    }

    private void Play()
    {
        switch (_dialogState)
        {
            case DIALOG_STATE.Ready:
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

        _textSkip = "";

        foreach (char c in _currentDialog)
        {
            _textSkip += c == _charSpace ? ' ' : c;
        }

        _dialogTxt.text = "";

        _isReading = false;

        yield return _waitStart;

        foreach (char c in _currentDialog)
        {
            if (c == _charSpace)
            {
                _dialogTxt.text += ' ';
                yield return _waitSpace;
            }
            else
            {
                _dialogTxt.text += c;
            }

            yield return null;
        }

        CompleteText();
    }

    private void CompleteText()
    {
        if (_isReading)return;

        _dialogState = DIALOG_STATE.Done;

        StopCoroutine(_coroutineWrite);

        _dialogTxt.text = _textSkip;

        if (_choiceNode != null)
        {
            for (int i = 0; i < _choiceNode.Choices.Length; i++)
            {
                _ddButtons[i].Show(_choiceNode.GetChoiceText(i, _currentLanguage));
                _lastIndexButton = i;
            }

            _ddButtons[_lastIndexButton].SetInput();
            _ddButtons[0].Select();

            _eventSystemEvent.objectSelected = _ddButtons[0].gameObject;
        }
        else
        {
            _dialogPanelBtn.Select();

            _eventSystemEvent.objectSelected = _dialogPanelBtn.gameObject;
        }

        EventController.TriggerEvent(_eventSystemEvent);
        _continueImg.SetActive(true);
    }

    private void SelectNode(int index)
    {
        _dialoguePlayer.AdvanceMessage(index);
    }

    public void Select()
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

    private void Back()
    {
        SelectNode(_lastIndexButton);
    }

    public void UpdateLanguage(string language)
    {
        _currentLanguage = language;

        _currentDialog = _showMessageNode.GetText(_currentLanguage);

        if (_choiceNode != null)
        {
            for (int i = 0; i < _choiceNode.Choices.Length; i++)
            {
                _ddButtons[i].UpdateText(_choiceNode.GetChoiceText(i, _currentLanguage));
            }
        }
    }

    private bool OnEvaluateCondition(DialoguePlayer sender, string script)
    {
        if (System.Enum.TryParse<DDEvaluateCondition>(script, out DDEvaluateCondition scriptParsed))
        {
            switch (scriptParsed)
            {
                case DDEvaluateCondition.HaveAmount:
                    // TODO Mariano: Review
                    // return _currentNPC.DDHaveAmount(); 
                    return true;

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
        if (script.Contains(emotion))
        {
            string[] emotionArray = script.Split('=');

            if (int.TryParse(emotionArray[1], out int emotionId))
            {
                _npcImg.sprite = _currentNPC.GetIcon((EMOTION)emotionId);
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
                case DDExecuteScript.GiveReward:
                    // _currentNPC.DDGiveReward();
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