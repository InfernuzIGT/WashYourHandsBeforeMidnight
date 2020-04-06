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

	private void Start()
	{
		_changePlayerPositionEvent = new ChangePlayerPositionEvent();
	}

	public override void Execute()
	{
		_changePlayerPositionEvent.newPosition =
			toInterior ?
			GameData.Instance.gameConfig.interiorPosition + newPlayerPosition :
			newPlayerPosition;

		EventController.TriggerEvent(_changePlayerPositionEvent);

		GameManager.Instance.CreateInterior(toInterior, interiorPrefab);
	}

}