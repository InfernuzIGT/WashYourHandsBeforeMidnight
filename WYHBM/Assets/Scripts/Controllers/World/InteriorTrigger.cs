using Events;
using UnityEngine;

public class InteriorTrigger : MonoBehaviour, IInteractable
{
    private InteractionInterior _interactionInterior;
    private EnableMovementEvent _enableMovementEvent;

    private void Start()
    {
        _interactionInterior = GetComponentInChildren<InteractionInterior>();

        _enableMovementEvent = new EnableMovementEvent();
        _enableMovementEvent.canMove = false;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            EventController.AddListener<InteractionEvent>(OnEnterToInterior);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            EventController.RemoveListener<InteractionEvent>(OnEnterToInterior);
        }
    }

    private void OnEnterToInterior(InteractionEvent evt)
    {
        // TODO Mariano: Habilitar/Desabilitar Movimiento Jugador
        // EventController.TriggerEvent(_enableMovementEvent);

        _interactionInterior.Execute();
    }
}