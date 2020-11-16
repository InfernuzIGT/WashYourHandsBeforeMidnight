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

    [Header("Interaction")]
    public QuestStatusSO questStatus;
    // public QuestData questData;
    [Space]
    public CutsceneData cutsceneData;
    [Space]
    [Space]
    public InteractionUnityEvent onEnter;
    public InteractionUnityEvent onExit;

    private SpriteRenderer _popupImage;

    private CutsceneEvent _cutsceneEvent;

    public virtual void Awake()
    {
        _popupImage = transform.GetComponentInChildren<SpriteRenderer>();

        _popupImage.enabled = false;

        _cutsceneEvent = new CutsceneEvent();
    }

    #region Interaction

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke(other);
        ShowPopup(true);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke(other);
        ShowPopup(false);
    }

    private void ShowPopup(bool show)
    {
        _popupImage.enabled = show;

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