using Events;
using UnityEngine;
using Cinemachine;

public class InteractionCutscene : Interaction, IInteractable
{
    public override void Awake()
    {
        base.Awake();
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
        
        GameManager.Instance.cinemachineManager.Cutscene();
    }
}