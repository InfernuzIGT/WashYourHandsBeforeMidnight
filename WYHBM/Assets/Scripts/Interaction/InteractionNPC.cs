using Events;
using UnityEngine;

public class InteractionNPC : Interaction
{
    [Header("NPC")]
    public NPCSO npc;

    private EnableDialogEvent _interactionDialogEvent;
    private TriggerCombatEvent _interactionCombatEvent;

    private void Start()
    {
        _interactionDialogEvent = new EnableDialogEvent();
        _interactionCombatEvent = new TriggerCombatEvent();
    }

    public override void Execute(bool enable)
    {
        base.Execute();

        switch (npc.interactionType)
        {
            case NPC_INTERACTION_TYPE.none:
                // Nothing
                break;

            case NPC_INTERACTION_TYPE.dialog:
                _interactionDialogEvent.dialog = npc.dialog;
                _interactionDialogEvent.enable = enable;
                EventController.TriggerEvent(_interactionDialogEvent);
                break;

            case NPC_INTERACTION_TYPE.fight:
                _interactionCombatEvent.npc = npc;
                EventController.TriggerEvent(_interactionCombatEvent);
                break;

            case NPC_INTERACTION_TYPE.dialogAndFight:
                // TODO Mariano: Primero se fuerza un dialog, y al finalizar se va al combate
                break;

            default:
                break;
        }

    }
}