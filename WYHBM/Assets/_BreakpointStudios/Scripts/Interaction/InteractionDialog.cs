using Events;
using UnityEngine;

public class InteractionDialog : Interaction
{
    private EnableDialogEvent _interactionDialogEvent;

    private void Start()
    {
        _interactionDialogEvent = new EnableDialogEvent();
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
        _interactionDialogEvent.npc = null;
        _interactionDialogEvent.dialogueable = this;
        _interactionDialogEvent.dialogue = GetDialogData();

        EventController.TriggerEvent(_interactionDialogEvent);
    }
}