using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[System.Serializable]
public class QuestData
{
    public QuestSO quest;
    public QUEST_STATE state;
    public int[] progress;
}

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour
{

    [System.Serializable]
    public class InteractionUnityEvent : UnityEvent<Collider> { }

    public bool showPopup = true;

    [Header("Quest")]
    public QuestData questData;

    public QuestSO quest; // TODO Marcos: Remove
    public int progress; // TODO Marcos: Remove

    [Header("Cutscene")]
    public PlayableAsset cutscene;
    public bool playInCollision;

    [Space]

    public InteractionUnityEvent onEnter;
    public InteractionUnityEvent onExit;

    private GameObject _popupGO;
    
    private CutsceneEvent _cutsceneEvent;

    public virtual void Awake()
    {
        _popupGO = transform.GetChild(0).gameObject;
        _popupGO.SetActive(false);
        
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
        if (!showPopup)return;

        _popupGO.SetActive(show);
    }

    protected void AddListenerQuest()
    {
        if (quest == null)return;

        EventController.AddListener<InteractionEvent>(OnInteractQuest);
    }

    protected void RemoveListenerQuest()
    {
        if (quest == null)return;

        EventController.RemoveListener<InteractionEvent>(OnInteractQuest);
    }

    private void OnInteractQuest(InteractionEvent evt)
    {
        GameManager.Instance.ProgressQuest(quest, progress);

        EventController.RemoveListener<InteractionEvent>(OnInteractQuest);
    }

    protected void PlayCutscene()
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