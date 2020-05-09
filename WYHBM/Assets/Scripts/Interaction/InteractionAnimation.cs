using Events;
using UnityEngine;

public class InteractionAnimation : Interaction, IInteractable
{
	[Header("Animation")]
	public Animator animator;
	public bool isTrigger;

	private bool _boolValue = true;
	private AnimationCommandBool _animInteractionBool = new AnimInteractionBool();
	private AnimationCommandTrigger _animInteractionTrigger = new AnimInteractionTrigger();

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

			if (isTrigger)
			{
				_animInteractionTrigger.Execute(animator);
			}
			else
			{
				_animInteractionBool.Execute(animator, _boolValue);
				_boolValue = !_boolValue;
			}
		}
	}

}