using Events;
using UnityEngine;

public class InteractionQuest : Interaction, IInteractable
{
    public QuestSO quest;

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractQuest);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractQuest);
        }
    }

    private void OnInteractQuest(InteractionEvent evt)
    {
        GameManager.Instance.ProgressQuest(quest);

        EventController.RemoveListener<InteractionEvent>(OnInteractQuest);

    }

}