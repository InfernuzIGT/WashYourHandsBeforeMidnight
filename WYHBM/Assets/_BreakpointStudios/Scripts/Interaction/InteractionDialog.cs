using Events;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;

public class InteractionDialog : Interaction
{
    [Header("Dialog")]
    [SerializeField] private LocalizedString _localizedDialog;
    [SerializeField, EventRef] private string _sound = "";
    [Space]
    [SerializeField] private bool _useOnlyOnce;
    [SerializeField] private LocalizedString _localizedUsedDialog;

    private DialogSimpleEvent _interactionDialogEvent;
    private bool _used;
    private string _usedId;

    private void Start()
    {
        _interactionDialogEvent = new DialogSimpleEvent();

        _usedId = string.Format(DDParameters.Format, DDParameters.SimpleDialog, gameObject.name);

        if (_useOnlyOnce)
        {
            _used = GameData.Instance.CheckID(_usedId);

            if (_used)ExecuteAnimation(true);
        }

    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(true);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(false);
        }
    }

    public override void Execute(bool enable)
    {
        base.Execute();

        CanInteractEvent(enable);

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.localizedString = _used ? _localizedUsedDialog : _localizedDialog;
        _interactionDialogEvent.questData = GetData();
        _interactionDialogEvent.questState = GetQuestState();

        EventController.TriggerEvent(_interactionDialogEvent);
    }

    public override void OnInteractEvent()
    {
        base.OnInteractEvent();

        if (_sound != "")RuntimeManager.PlayOneShot(_sound);

        ExecuteAnimation();

        if (_useOnlyOnce && !_used)
        {
            _used = true;
            Execute(false);

            GameData.Instance.WriteID(_usedId);
        }
    }

    private QuestSO GetData()
    {
        QuestSO data = GetQuestData();

        if (data == null)return null;

        QUEST_STATE state = GetQuestState();
        string id = string.Format(DDParameters.FormatQuadruple, data.name, state.ToString(), DDParameters.SimpleQuest, gameObject.name);

        switch (state)
        {
            case QUEST_STATE.New:
                if (!GameData.Instance.CheckAndWriteID(id))
                {
                    return data;
                }
                break;

            case QUEST_STATE.Update:
                if (GameData.Instance.CheckQuestRequiredStep(data, GetQuestRequiredStep()) && !GameData.Instance.CheckID(id))
                {
                    GameData.Instance.WriteID(id);
                    return data;
                }
                break;

            case QUEST_STATE.Complete:
                if (GameData.Instance.HaveQuest(data) && !GameData.Instance.CheckID(id))
                {
                    GameData.Instance.WriteID(id);
                    return data;
                }
                break;
        }

        return null;
    }
}