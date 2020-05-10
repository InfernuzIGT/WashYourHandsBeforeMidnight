using Events;
using UnityEngine;

public class InteractionDialog : Interaction
{
    [Header("Dialog")]
    public DialogSO dialog;

    private EnableDialogEvent _interactionDialogEvent;

    private void Start()
    {
        _interactionDialogEvent = new EnableDialogEvent();
        _interactionDialogEvent.dialog = dialog;
    }

    public override void Execute(bool enable, NPCController currentNPC)
    {
        base.Execute();

        _interactionDialogEvent.enable = enable;
        EventController.TriggerEvent(_interactionDialogEvent);
    }
}