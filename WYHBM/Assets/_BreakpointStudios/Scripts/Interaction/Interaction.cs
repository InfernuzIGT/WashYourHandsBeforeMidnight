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

[System.Serializable]
public struct QuestData
{
    public QuestSO quest;
    public QUEST_STATE state;
    public int requiredStep;
}

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour, IDialogueable
{
    [System.Serializable]
    public class InteractionUnityEvent : UnityEvent<Collider> { }

    [Header("Interaction")]
    [SerializeField] private QuestData[] questData = null;
    [Space]
    [SerializeField] private InteractionUnityEvent onEnter = null;
    [SerializeField] private InteractionUnityEvent onExit = null;

    protected bool _showHint = true;

    // private SpriteRenderer _hintSprite;
    private bool _canInteract = true;

    private QuestEvent _questEvent;
    // private ShowInteractionHintEvent _showInteractionHintEvent;
    private CurrentInteractEvent _currentInteractionEvent;

    public virtual void Awake()
    {
        // _hintSprite = transform.GetComponentInChildren<SpriteRenderer>();

        // _hintSprite.enabled = false;

        _questEvent = new QuestEvent();
        // _showInteractionHintEvent = new ShowInteractionHintEvent();
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

        if (GameData.Instance.GetPlayerCurrentInteraction() != null)return;

        _currentInteractionEvent.currentInteraction = this;
        EventController.TriggerEvent(_currentInteractionEvent);

        onEnter.Invoke(other);
        ShowHint(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_canInteract)return;

        if (GameData.Instance.GetPlayerCurrentInteraction() != this)return;

        _currentInteractionEvent.currentInteraction = null;
        EventController.TriggerEvent(_currentInteractionEvent);

        onExit.Invoke(other);
        ShowHint(false);
    }

    protected void ShowHint(bool show)
    {
        // if (!_showHint)return;

        // _hintSprite.enabled = show;

        // _showInteractionHintEvent.show = show;
        // EventController.TriggerEvent(_showInteractionHintEvent);
    }

    protected void ForceCleanInteraction()
    {
        _currentInteractionEvent.currentInteraction = null;
        EventController.TriggerEvent(_currentInteractionEvent);
    }

    public QuestSO GetQuestData()
    {
        return questData[GameData.Instance.PlayerData.ID].quest;
    }

    public QUEST_STATE GetQuestState()
    {
        return questData[GameData.Instance.PlayerData.ID].state;
    }
    
    public int GetQuestRequiredStep()
    {
        return questData[GameData.Instance.PlayerData.ID].requiredStep;
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
        return !GameData.Instance.CheckAndWriteID(string.Format(DDParameters.Format, DDParameters.FirstTime, gameObject.name));
    }

    public bool DDFinished()
    {
        return GameData.Instance.CheckID(string.Format(DDParameters.Format, DDParameters.Finished, gameObject.name));
    }

    public bool DDCheckQuest()
    {
        return GameData.Instance.CheckQuestCurrentStep(GetQuestData());
    }

    public bool DDHaveQuest()
    {
        return GameData.Instance.HaveQuest(GetQuestData());
    }

    public void DDFinish()
    {
        GameData.Instance.WriteID(string.Format(DDParameters.Format, DDParameters.Finished, gameObject.name));
    }

    #endregion
}