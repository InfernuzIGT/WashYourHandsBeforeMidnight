using Events;
using TMPro;
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

    [Header("Popup")]
    public bool showPopup = true;
    public bool showPopupText = true;

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

    private SpriteRenderer _popupImage;
    private TextMeshPro _popupText;

    private CutsceneEvent _cutsceneEvent;

    public virtual void Awake()
    {
        _popupImage = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _popupText = transform.GetChild(1).GetComponent<TextMeshPro>();

        _popupImage.enabled = false;
        _popupText.enabled = false;

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

        _popupImage.enabled = show;

        if (showPopupText)
        {
            _popupText.enabled = show;
        }
    }

    protected void SetPopupName(string name)
    {
        _popupText.text = name;
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