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
    [SerializeField] private InteractionData[] data = null;
    [SerializeField] private QUEST_STATE[] questState = null;
    [Space]
    [SerializeField] private InteractionUnityEvent onEnter = null;
    [SerializeField] private InteractionUnityEvent onExit = null;

    protected bool _showHint = true;

    private SpriteRenderer _hintSprite;
    private bool _canInteract = true;

    private QuestEvent _questEvent;
    private ShowInteractionHintEvent _showInteractionHintEvent;
    private CurrentInteractEvent _currentInteractionEvent;

    public virtual void Awake()
    {
        _hintSprite = transform.GetComponentInChildren<SpriteRenderer>();

        _hintSprite.enabled = false;

        _questEvent = new QuestEvent();
        _showInteractionHintEvent = new ShowInteractionHintEvent();
        _currentInteractionEvent = new CurrentInteractEvent();
    }

    private void OnEnable()
    {
        EventController.AddListener<CutsceneEvent>(OnCutscene);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
    }

    private void OnCutscene(CutsceneEvent evt)
    {
        _canInteract = !evt.show;
    }

    #region Interaction

    private void OnTriggerEnter(Collider other)
    {
        if (!_canInteract)return;

        if (GameData.Instance.Player.CurrentInteraction != null)return;

        // if (GameData.Instance.Player.CurrentInteraction != this)return;

        _currentInteractionEvent.currentInteraction = this;
        EventController.TriggerEvent(_currentInteractionEvent);

        onEnter.Invoke(other);
        ShowHint(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_canInteract)return;

        if (GameData.Instance.Player.CurrentInteraction != this)return;

        _currentInteractionEvent.currentInteraction = null;
        EventController.TriggerEvent(_currentInteractionEvent);

        onExit.Invoke(other);
        ShowHint(false);
    }

    protected void ShowHint(bool show)
    {
        if (!_showHint)return;

        _hintSprite.enabled = show;

        _showInteractionHintEvent.show = show;
        EventController.TriggerEvent(_showInteractionHintEvent);
    }

    public TextAsset GetDialogData()
    {
        return data[GameData.Instance.PlayerData.ID].dialogDD;
    }

    public QuestSO GetQuestData()
    {
        return data[GameData.Instance.PlayerData.ID].quest;
    }

    public QUEST_STATE GetQuestState()
    {
        return questState[GameData.Instance.PlayerData.ID];
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