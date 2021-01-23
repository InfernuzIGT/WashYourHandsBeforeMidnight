using Events;
using UnityEngine;

public class InteractionLadder : Interaction, IInteractable
{
    [Header("Ladder")]
    public bool inLadder;

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractionLadder);
            EventController.AddListener<LadderEvent>(OnExitLadder);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && inLadder)
        {
            OnExitLadder(LADDER_EXIT.Top);
        }
    }

    private void OnInteractionLadder(InteractionEvent evt)
    {
        inLadder = !inLadder;

        GameManager.Instance.globalController.playerController.SwitchLadderMovement(inLadder);

        if (inLadder)
        {
            GameManager.Instance.globalController.playerController.SetNewPosition(
                transform.position.x,
                GameManager.Instance.globalController.playerController.transform.position.y + 1, // TODO Mariano: Move To Config (ladderOffsetY)
                transform.position.z);
        }
        else
        {
            OnExitLadder(LADDER_EXIT.Interaction);
        }
    }

    private void OnExitLadder(LadderEvent evt)
    {
        OnExitLadder(evt.ladderExit);
    }

    private void OnExitLadder(LADDER_EXIT ladderExit)
    {
        inLadder = false;

        EventController.RemoveListener<InteractionEvent>(OnInteractionLadder);
        EventController.RemoveListener<LadderEvent>(OnExitLadder);

        switch (ladderExit)
        {
            case LADDER_EXIT.Interaction:
                // Nothing
                break;

            case LADDER_EXIT.Bot:
                // TODO Mariano: Animation Bot
                break;

            case LADDER_EXIT.Top:
                // TODO Mariano: Animation Top
                break;

            default:
                break;
        }
    }
}