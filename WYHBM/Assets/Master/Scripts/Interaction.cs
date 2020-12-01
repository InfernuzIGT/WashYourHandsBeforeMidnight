using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour
{
    [System.Serializable]
    public class InteractionUnityEvent : UnityEvent<Collider> { }

    [Header("Interaction")]
    public QuestSO quest;
    public PlayableAsset cutscene;
    [Space]
    public InteractionUnityEvent onEnter;
    public InteractionUnityEvent onExit;

    private SpriteRenderer _hintSprite;

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

    protected void TriggerQuest()
    {
        if (quest == null)return;

        _questEvent.quest = quest;
        EventController.TriggerEvent(_questEvent);
    }
    
    protected void TriggerCutscene()
    {
        if (cutscene == null)return;

        _cutsceneEvent.cutscene = cutscene;
        EventController.TriggerEvent(_cutsceneEvent);
    }

    #endregion

    #region Execution

    public virtual void Execute() { }
    public virtual void Execute(bool enable, NPCController currentNPC) { }

    #endregion
}