using Events;
using UnityEngine;
using UnityEngine.Localization;

public class InteractionDialog : Interaction
{
    [Header("Dialog")]
    [SerializeField] private LocalizedString _localizedDialog;

    private DialogSimpleEvent _interactionDialogEvent;
    [SerializeField] private Animator anim;
    private bool _isPlaying;
    

    private void Start()
    {
        _interactionDialogEvent = new DialogSimpleEvent();
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

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.localizedString = _localizedDialog;
        _interactionDialogEvent.questData = GetData();
        _interactionDialogEvent.questState = GetQuestState();

        if (anim != null && !_isPlaying)
        {
            _isPlaying = true;

            anim.SetBool("startAnim", true);

            FMODUnity.RuntimeManager.PlayOneShot("event:/Interactables/Doors/Lever", GetComponent<Transform>().position);

        }

        EventController.TriggerEvent(_interactionDialogEvent);

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