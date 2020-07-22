using Events;
using UnityEngine;

public class InteractionSave : Interaction, IInteractable
{
    private string title = "Save Game";

    public override void Awake()
    {
        base.Awake();

        SetPopupName(title);
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractSave);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractSave);
        }
    }

    private void OnInteractSave(InteractionEvent evt)
    {
        // GameManager.Instance.SaveGame();
    }
}