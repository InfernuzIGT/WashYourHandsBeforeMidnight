using System.Collections;
using Events;
using FMODUnity;
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

[System.Serializable]
public struct AnimationData
{
    public ObjectAnimator objectAnimator;
    public bool isTrigger;
    [EventRef] public string sound;
}

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour, IDialogueable
{
    [System.Serializable]
    public class InteractionUnityEvent : UnityEvent<Collider> { }

    [Header("Interaction")]
    [SerializeField] private QuestData[] questData = null;
    [Space]
    [SerializeField] private AnimationData[] animationData = null;
    [Space]
    [SerializeField] private InteractionUnityEvent onEnter = null;
    [SerializeField] private InteractionUnityEvent onExit = null;

    // Persistence
    protected bool _used;
    protected string _usedId;

    // Events
    private QuestEvent _questEvent;
    // private ShowInteractionHintEvent _showInteractionHintEvent;
    private CurrentInteractEvent _currentInteractionEvent;

    // private SpriteRenderer _hintSprite;
    private BoxCollider _boxCollider;
    private bool _canInteract = true;
    private bool _animationReady;
    protected bool _showHint = true;

    public virtual void Awake()
    {
        // _hintSprite = transform.GetComponentInChildren<SpriteRenderer>();

        // _hintSprite.enabled = false;

        _boxCollider = GetComponent<BoxCollider>();

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

    protected void CanInteractEvent(bool canInteract)
    {
        if (canInteract)
        {
            EventController.AddListener<InteractionEvent>(OnInteractEvent);
        }
        else
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractEvent);
        }
    }

    private void OnInteractEvent(InteractionEvent evt)
    {
        OnInteractEvent();
    }

    public virtual void OnInteractEvent()
    {
        CanInteractEvent(false);
    }

    protected void ShowHint(bool show)
    {
        // if (!_showHint)return;

        // _hintSprite.enabled = show;

        // _showInteractionHintEvent.show = show;
        // EventController.TriggerEvent(_showInteractionHintEvent);
    }

    protected void ExecuteAnimation(bool instant = false)
    {
        if (animationData == null || _animationReady)return;

        _animationReady = true;

        if (!instant)
        {
            for (int i = 0; i < animationData.Length; i++)
            {
                animationData[i].objectAnimator.Execute(animationData[i].isTrigger);
                if (animationData[i].sound != "")RuntimeManager.PlayOneShot(animationData[i].sound, animationData[i].objectAnimator.gameObject.transform.position);
            }
        }
        else
        {
            for (int i = 0; i < animationData.Length; i++)
            {
                animationData[i].objectAnimator.Execute(animationData[i].isTrigger, true);
            }
        }

    }

    protected void ForceCleanInteraction()
    {
        _currentInteractionEvent.currentInteraction = null;
        EventController.TriggerEvent(_currentInteractionEvent);
    }

    protected void SetCollider(bool enabled)
    {
        _boxCollider.enabled = enabled;
    }

    protected void CheckPersistence(string id)
    {
        _usedId = id;
        StartCoroutine(CheckingPersistence());
    }

    private IEnumerator CheckingPersistence()
    {
        yield return new WaitForSeconds(0.5f);

        _used = GameData.Instance.CheckID(_usedId);
        if (_used)Used();
    }

    public virtual void Used() { }

    public QuestSO GetQuestData()
    {
        if (questData.Length == 0)return null;

        return questData[GameData.Instance.PlayerData.ID].quest;
    }

    public QUEST_STATE GetQuestState()
    {
        if (questData.Length == 0)return QUEST_STATE.New;

        return questData[GameData.Instance.PlayerData.ID].state;
    }

    public int GetQuestRequiredStep()
    {
        if (questData.Length == 0)return 0;

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