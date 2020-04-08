using Events;
using UnityEngine;

public class InteractionInterior : Interaction
{
	[Header("Interior")]
	public bool toInterior = true;
	public Vector3 newPlayerPosition;
	[Space]
	public GameObject interiorPrefab;

	private ChangePlayerPositionEvent _changePlayerPositionEvent;

	private EnableMovementEvent _enableMovementEvent;
	private FadeEvent _fadeEvent;
	private CreateInteriorEvent _createInteriorEvent;

	private void Start()
	{
		_changePlayerPositionEvent = new ChangePlayerPositionEvent();

		_enableMovementEvent = new EnableMovementEvent();

		_fadeEvent = new FadeEvent();
		_fadeEvent.fadeFast = true;
		_fadeEvent.callbackStart = CallbackStart;
		_fadeEvent.callbackMid = CallbackMid;
		_fadeEvent.callbackEnd = CallbackExit;

		_createInteriorEvent = new CreateInteriorEvent();
		_createInteriorEvent.isCreating = toInterior;
		_createInteriorEvent.newInterior = interiorPrefab;
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
		Execute();
	}

	public override void Execute()
	{
		_changePlayerPositionEvent.newPosition =
			toInterior ?
			GameData.Instance.gameConfig.interiorPosition + newPlayerPosition :
			newPlayerPosition;

		EventController.TriggerEvent(_fadeEvent);
	}

	private void CallbackStart()
	{
		_enableMovementEvent.canMove = false;
		_enableMovementEvent.enterToInterior = toInterior;
		EventController.TriggerEvent(_enableMovementEvent);
	}

	private void CallbackMid()
	{
		EventController.TriggerEvent(_changePlayerPositionEvent);

		EventController.TriggerEvent(_createInteriorEvent);
	}

	private void CallbackExit()
	{
		_enableMovementEvent.canMove = true;
		_enableMovementEvent.enterToInterior = toInterior;
		EventController.TriggerEvent(_enableMovementEvent);
	}

}