using Events;
using UnityEngine;

public class InteractionSave : Interaction, IInteractable
{
    private SessionEvent _sessionEvent;

    private void Start()
    {
        _sessionEvent = new SessionEvent();
        _sessionEvent.option = SESSION_OPTION.Save;
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
        EventController.TriggerEvent(_sessionEvent);
        EventController.RemoveListener<InteractionEvent>(OnInteractSave);
    }
}