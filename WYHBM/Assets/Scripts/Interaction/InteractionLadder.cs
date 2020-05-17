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
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractionLadder);

            OnExitLadder();
        }
    }

    private void OnInteractionLadder(InteractionEvent evt)
    {
        inLadder = !inLadder;

        GameManager.Instance.globalController.player.SwitchLadderMovement(inLadder);

        if (inLadder)
        {
            GameManager.Instance.globalController.player.SetNewPosition(
                transform.position.x,
                GameManager.Instance.globalController.player.transform.position.y + 0.5f, // TODO Mariano: Move To Config
                transform.position.z);
        }
    }

    private void OnExitLadder()
    {
        if (inLadder)
        {
            Debug.Log($"<b> EXIT LADDER </b>");
        }

        inLadder = false;
    }
}