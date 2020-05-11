using Events;
using UnityEngine;

public class InteractionDoor : Interaction, IInteractable
{
    [Header("Animation")]
    public Animator animator;
    public bool isClosed = true;

    private bool _boolValue = true;
    private AnimationCommandBool _animDoorIsOpening = new AnimDoorIsOpening();
    private AnimationCommandBool _animDoorIsRunning = new AnimDoorIsRunning();
    private AnimationCommandTrigger _animDoorIsLocked = new AnimDoorIsLocked();

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnTriggerAnimation);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnTriggerAnimation);
        }
    }

    private void OnTriggerAnimation(InteractionEvent evt)
    {
        if (animator != null)
        {
            if (isClosed)
            {
                _animDoorIsLocked.Execute(animator);
            }
            else
            {
                _animDoorIsOpening.Execute(animator, _boolValue);
                _boolValue = !_boolValue;

                // TODO Mariano: Review
                // if (evt.isRunning)
                // {
                //     _animDoorIsRunning.Execute(animator, _boolValue);
                // }
                // else
                // {
                //     _animDoorIsOpening.Execute(animator, _boolValue);
                // }

                // _boolValue = !_boolValue;
            }
        }
    }
}