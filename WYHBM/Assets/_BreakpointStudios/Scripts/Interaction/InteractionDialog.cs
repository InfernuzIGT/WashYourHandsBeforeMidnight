using Events;
using UnityEngine;
using UnityEngine.Localization;

public class InteractionDialog : Interaction
{
    [Header("Dialog")]
    [SerializeField] private LocalizedString _localizedDialog;

    private DialogSimpleEvent _interactionDialogEvent;

    private void Start()
    {
        _interactionDialogEvent = new DialogSimpleEvent();
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

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.localizedString = _localizedDialog;
        _interactionDialogEvent.questData = GetQuestData();
        _interactionDialogEvent.questState = GetQuestState();
        _interactionDialogEvent.objectName = gameObject.name;

        EventController.TriggerEvent(_interactionDialogEvent);
    }
}