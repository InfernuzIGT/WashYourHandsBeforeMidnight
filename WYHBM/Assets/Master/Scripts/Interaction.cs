using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

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
    public InteractionData[] data;
    [Space]
    public QUEST_STATE[] questState;
    [Space]
    public PlayableAsset cutscene;
    [Space]
    public InteractionUnityEvent onEnter;
    public InteractionUnityEvent onExit;

    private SpriteRenderer _hintSprite;
    private PlayerSO _playerData;

    private CutsceneEvent _cutsceneEvent;
    private QuestEvent _questEvent;
    private ShowInteractionHintEvent _showInteractionHintEvent;

    public virtual void Awake()
    {
        _hintSprite = transform.GetComponentInChildren<SpriteRenderer>();

        _hintSprite.enabled = false;

        _cutsceneEvent = new CutsceneEvent();
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
        _hintSprite.enabled = show;

        _showInteractionHintEvent.show = show;
        EventController.TriggerEvent(_showInteractionHintEvent);
    }

    protected void TriggerCutscene()
    {
        if (cutscene == null)return;

        _cutsceneEvent.cutscene = cutscene;
        EventController.TriggerEvent(_cutsceneEvent);
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
        // TODO Mariano: Persistence
        return true;
    }

    public bool DDFinished()
    {
        // TODO Mariano: Persistence
        return false;
    }
    
    public bool DDCheckQuest()
    {
        // TODO Mariano: Persistence
        return false;
    }

    #endregion
}