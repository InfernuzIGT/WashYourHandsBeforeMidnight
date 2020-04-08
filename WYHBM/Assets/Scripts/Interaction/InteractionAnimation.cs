using Events;
using UnityEngine;

public class InteractionAnimation : Interaction, IInteractable
{
	[Header("Animation")]
	public bool isTrigger;
	public bool boolValue = true;

	private Animator _animator;

	private AnimationCommandBool _animInteractionBool = new AnimInteractionBool();
	private AnimationCommandTrigger _animInteractionTrigger = new AnimInteractionTrigger();

	private void Awake()
	{
		_animator = GetComponentInParent<Animator>();
	}

	public void OnInteractionEnter(Collider other)
	{
		if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
		{
			EventController.AddListener<InteractionEvent>(OnTriggerAnimation);
		}
	}

	public void OnInteractionExit(Collider other)
	{
		if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
		{
			EventController.RemoveListener<InteractionEvent>(OnTriggerAnimation);
		}
	}

	private void OnTriggerAnimation(InteractionEvent evt)
	{
		if (isTrigger)
		{
			_animInteractionTrigger.Execute(_animator);
		}
		else
		{
			_animInteractionBool.Execute(_animator, boolValue);
			boolValue = !boolValue;
		}
	}

}