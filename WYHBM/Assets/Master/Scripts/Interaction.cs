using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[System.Serializable]
public class QuestData
{
    public QuestStatusSO questStatus;
    [Space]
    public QuestSO quest;
    public QUEST_STATE state;
    public int[] progress;
}

[System.Serializable]
public class CutsceneData
{
    public PlayableAsset playableAsset;
    public bool playInCollision;
}

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour
{
    [System.Serializable]
    public class InteractionUnityEvent : UnityEvent<Collider> { }

    // [Header("Interaction")]
    private QuestStatusSO questStatus;
    // public QuestData questData;
    [Space]
    private CutsceneData cutsceneData;
    [Space]
    [Space]
    public InteractionUnityEvent onEnter;
    public InteractionUnityEvent onExit;

    private SpriteRenderer _hintSprite;

    private CutsceneEvent _cutsceneEvent;
    private ShowInteractionHintEvent _showInteractionHintEvent;

    public virtual void Awake()
    {
        _hintSprite = transform.GetComponentInChildren<SpriteRenderer>();

        _hintSprite.enabled = false;

        _cutsceneEvent = new CutsceneEvent();
        _showInteractionHintEvent = new ShowInteractionHintEvent();;
    }

    #region Interaction

    private void OnTriggerEnter(Collider other)
    {
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

    // protected void AddListenerQuest()
    // {
    //     if (questData.quest == null)return;

    //     EventController.AddListener<InteractionEvent>(OnInteractQuest);
    // }

    // protected void RemoveListenerQuest()
    // {
    //     if (questData.quest == null)return;

    //     EventController.RemoveListener<InteractionEvent>(OnInteractQuest);
    // }

    // private void OnInteractQuest(InteractionEvent evt)
    // {
    //     GameManager.Instance.ProgressQuest(questData.quest, questData.progress[0]);

    //     EventController.RemoveListener<InteractionEvent>(OnInteractQuest);
    // }

    protected void PlayCutscene()
    {
        if (cutsceneData.playableAsset == null)return;
        if (_cutsceneEvent.isTriggered == true)return;

        _cutsceneEvent.cutscene = cutsceneData.playableAsset;

        _cutsceneEvent.isTriggered = true;

        EventController.TriggerEvent(_cutsceneEvent);
    }

    #endregion

    #region Execution

    public virtual void Execute() { }
    public virtual void Execute(bool enable, NPCController currentNPC) { }

    #endregion
}