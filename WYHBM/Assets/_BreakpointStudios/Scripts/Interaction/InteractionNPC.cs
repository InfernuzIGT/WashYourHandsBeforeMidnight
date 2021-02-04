using Events;

public class InteractionNPC : Interaction
{
    private DialogDesignerEvent _interactionDialogEvent;

    private void Start()
    {
        _interactionDialogEvent = new DialogDesignerEvent();
    }

    public override void Execute(bool enable, NPCController currentNPC)
    {
        base.Execute();

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.npc = currentNPC;
        _interactionDialogEvent.dialogueable = currentNPC;
        _interactionDialogEvent.playerData = GameData.Instance.PlayerData;
        _interactionDialogEvent.dialogue = currentNPC.GetDialogData();

        EventController.TriggerEvent(_interactionDialogEvent);
    }

}