using Events;
using UnityEngine;

public class InteractionNPC : Interaction
{
    private DialogDesignerEvent _interactionDialogEvent;
    private TextAsset _dialogue;

    private void Start()
    {
        _interactionDialogEvent = new DialogDesignerEvent();
    }

    public override void Execute(bool enable, NPCController currentNPC)
    {
        base.Execute();

        _dialogue = currentNPC.GetDialogData();

        if (_dialogue == null)return;

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.npc = currentNPC;
        _interactionDialogEvent.dialogueable = currentNPC;
        _interactionDialogEvent.playerData = GameData.Instance.PlayerData;
        _interactionDialogEvent.dialogue = _dialogue;

        EventController.TriggerEvent(_interactionDialogEvent);
    }

}