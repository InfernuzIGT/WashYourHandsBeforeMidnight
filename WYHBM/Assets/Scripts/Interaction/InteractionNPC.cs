using Events;
using UnityEngine;

public class InteractionNPC : Interaction
{
    [Header("NPC")]
    public NPCSO npc;

    private EnableDialogEvent _interactionDialogEvent;
    private EnterCombatEvent _interactionCombatEvent;

    private bool _canInteraction;
    private void Start()
    {
        _interactionDialogEvent = new EnableDialogEvent();
        _interactionCombatEvent = new EnterCombatEvent();
        _canInteraction = true;
    }

    public override void Execute(bool enable, NPCController currentNPC)
    {
        base.Execute();
        if (!_canInteraction)
        {
            return;
        }

        switch (npc.interactionType)
        {
            case NPC_INTERACTION_TYPE.none:
                // Nothing
                break;

            case NPC_INTERACTION_TYPE.dialog:
                _canInteraction = false;
                _interactionDialogEvent.dialog = npc.dialog;
                _interactionDialogEvent.enable = enable;
                EventController.TriggerEvent(_interactionDialogEvent);
                Debug.Log("DIALOG!");
                break;

            case NPC_INTERACTION_TYPE.fight:
                _interactionCombatEvent.npc = npc;
                _interactionCombatEvent.currentNPC = currentNPC;
                EventController.TriggerEvent(_interactionCombatEvent);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Stinger");
                break;

            case NPC_INTERACTION_TYPE.dialogAndFight:
                // TODO Mariano: Primero se fuerza un dialog, y al finalizar se va al combate
                break;

            default:
                break;
        }

    }
}