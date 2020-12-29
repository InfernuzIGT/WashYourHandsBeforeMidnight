using Events;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct InteractionData
{
    public TextAsset dialogDD;
    [Space]
    public QuestSO quest;
}

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour, IDialogueable
{
    [System.Serializable]
    public class InteractionUnityEvent : UnityEvent<Collider> { }

    [Header("Interaction")]
    [SerializeField] private InteractionData[] data;
    [Space]
    [SerializeField] private QUEST_STATE[] questState;
    [Space]
    [SerializeField] private InteractionUnityEvent onEnter;
    [SerializeField] private InteractionUnityEvent onExit;

    private SpriteRenderer _hintSprite;
    private PlayerSO _playerData;

    protected bool _showHint = true;

    private QuestEvent _questEvent;
    private ShowInteractionHintEvent _showInteractionHintEvent;

    public virtual void Awake()
    {
        _hintSprite = transform.GetComponentInChildren<SpriteRenderer>();

        _hintSprite.enabled = false;

        _questEvent = new QuestEvent();
        _showInteractionHintEvent = new ShowInteractionHintEvent();;
    }

    #region Interaction

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && _playerData == null)_playerData = other.gameObject.GetComponent<PlayerController>().PlayerData;

        onEnter.Invoke(other);
        ShowHint(true);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke(other);
        ShowHint(false);
    }

    private void ShowHint(bool show)
    {
        if (!_showHint)return;

        _hintSprite.enabled = show;

        _showInteractionHintEvent.show = show;
        EventController.TriggerEvent(_showInteractionHintEvent);
    }

    public TextAsset GetDialogData()
    {
        return data[_playerData.ID].dialogDD;
    }

    public QuestSO GetQuestData()
    {
        return data[_playerData.ID].quest;
    }

    public QUEST_STATE GetQuestState()
    {
        return questState[_playerData.ID];
    }

    #endregion

    #region Execution

    public virtual void Execute() { }
    public virtual void Execute(bool enable) { }
    public virtual void Execute(bool enable, NPCController currentNPC) { }

    #endregion

    #region Dialogue Designer

    public void DDQuest(QUEST_STATE state)
    {
        _questEvent.data = GetQuestData();
        _questEvent.state = state;
        EventController.TriggerEvent(_questEvent);
    }

    public bool DDFirstTime()
    {
        return !GameData.Instance.CheckAndWriteID(string.Format(DDParameters.Format, gameObject.name, DDParameters.FirstTime));
    }

    public bool DDFinished()
    {
        return GameData.Instance.CheckID(string.Format(DDParameters.Format, gameObject.name, DDParameters.Finished));
    }

    public bool DDCheckQuest()
    {
        return GameData.Instance.CheckQuest(GetQuestData());
    }

    public bool DDHaveQuest()
    {
        return GameData.Instance.HaveQuest(GetQuestData());
    }

    public void DDFinish()
    {
        GameData.Instance.WriteID(string.Format(DDParameters.Format, gameObject.name, DDParameters.Finished));
    }

    #endregion
}