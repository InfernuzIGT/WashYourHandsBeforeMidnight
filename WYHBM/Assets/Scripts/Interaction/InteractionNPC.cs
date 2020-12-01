using Events;
// using FMODUnity;
using UnityEngine;

public class InteractionNPC : Interaction
{
    // [Header("NPC")]
    // [SerializeField] private NPCController NPC;
    // public bool haveAmount;

    private EnableDialogEvent _interactionDialogEvent;
    // private EnterCombatEvent _interactionCombatEvent;

    private void Start()
    {
        _interactionDialogEvent = new EnableDialogEvent();
        // _interactionCombatEvent = new EnterCombatEvent();
    }

    public override void Execute(bool enable, NPCController currentNPC)
    {
        base.Execute();

        _interactionDialogEvent.npc = currentNPC;
        _interactionDialogEvent.enable = enable;

        EventController.TriggerEvent(_interactionDialogEvent);

        // switch (data.interactionType)
        // {
        //     case NPC_INTERACTION_TYPE.none:
        //         // Nothing
        //         break;

        //     case NPC_INTERACTION_TYPE.dialog:

        //         _interactionDialogEvent.npc = data;
        //         _interactionDialogEvent.enable = enable;
        //         // _interactionDialogEvent.questData = questData;
        //         PlayCutscene();

        //         EventController.TriggerEvent(_interactionDialogEvent);
        //         break;

        //     case NPC_INTERACTION_TYPE.fight:
        //         _interactionCombatEvent.npc = data;
        //         _interactionCombatEvent.currentNPC = currentNPC;
        //         EventController.TriggerEvent(_interactionCombatEvent);

        //         RuntimeManager.PlayOneShot(FMODParameters.OneShot_Stinger);
        //         break;

        //     case NPC_INTERACTION_TYPE.dialogAndFight:
        //         // TODO Mariano: Primero se fuerza un dialog, y al finalizar se va al combate
        //         break;

        //     default:
        //         break;
        // }
    }

}