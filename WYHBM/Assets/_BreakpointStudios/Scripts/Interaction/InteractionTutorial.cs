using Events;
using UnityEngine;

public class InteractionTutorial : Interaction
{
    [Header("Tutorial")]
    // [SerializeField] private TUTORIAL _tutorial = TUTORIAL.None;
    [SerializeField] private INPUT_ACTION _actionTutorial = INPUT_ACTION.Interact;

    private TutorialEvent _tutorialEvent;

    private void Start()
    {
        _checkCurrentInteraction = false;

        _tutorialEvent = new TutorialEvent();
        _tutorialEvent.actionTutorial = _actionTutorial;

        CheckPersistence(string.Format(DDParameters.Format, DDParameters.SimpleDialog, gameObject.name));
    }

    public override void OnEnableExtra()
    {
        base.OnEnableExtra();
        EventController.AddListener<InteractionEvent>(OnInteract);
    }

    public override void OnDisableExtra()
    {
        base.OnDisableExtra();
        EventController.RemoveListener<InteractionEvent>(OnInteract);
    }

    private void OnInteract(InteractionEvent evt)
    {
        Execute(false);
    }

    public override void Used()
    {
        base.Used();

        ExecuteAnimation(true);
        SetCollider(false);
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(true);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(false);
        }
    }

    public override void Execute(bool enable)
    {
        base.Execute();

        _tutorialEvent.show = enable;
        EventController.TriggerEvent(_tutorialEvent);
    }

    // private void Action()
    // {
    //     Execute(false);

    //     if (!_used)
    //     {
    //         _used = true;
    //         Execute(false);
    //         SetCollider(false);

    //         GameData.Instance.WriteID(_usedId);
    //     }
    // }

}