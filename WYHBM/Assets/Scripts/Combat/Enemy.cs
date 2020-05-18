using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Enemy : CombatCharacter
{
	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
	}

	/// <summary>
	/// Espera la accion
	/// </summary>
	public override IEnumerator WaitingForAction()
	{
		base.WaitingForAction();
		
		GameManager.Instance.combatUI.ShowPlayerPanel(false);

		_isActionDone = false;

		Select();

		while (!_isActionDone)
		{
			yield return null;
		}

		Debug.Log($"<color=red><b> [COMBAT] </b></color> Action by {gameObject.name}");

		yield return _waitPerAction;

		AnimationAction(COMBAT_STATE.Idle);
	}

	public void Select()
	{
		Debug.Log ($"<b> Select enemy </b>");
		// TODO Mariano: Seleccionar Jugadores
		GameManager.Instance.combatManager.listPlayers[0].ActionReceiveDamage(StatsDamage);

		DoAction();
	}

	public void Select(COMBAT_STATE combatState, CombatCharacter currentCharacter)
	{
		switch (combatState)
		{
			case COMBAT_STATE.Attack:
				ActionReceiveDamage(currentCharacter.StatsDamage);
				currentCharacter.AnimationAction(combatState);
				break;

			case COMBAT_STATE.Item:
				// TODO Mariano: Damage with items
				break;

			case COMBAT_STATE.Defense:
				// TODO Mariano: Add defense per 1 turn
				break;

			default:
				break;
		}
		Debug.Log ($"<b> Select Player </b>");

		currentCharacter.DoAction();
	}

	public override void SetCharacter()
	{
		base.SetCharacter();
		// SpriteRenderer.flipX = true;
	}

	public override void ActionStartCombat()
	{
		base.ActionStartCombat();

		transform.
		DOMove(-GameData.Instance.combatConfig.positionCombat, GameData.Instance.combatConfig.transitionDuration).
		SetEase(Ease.OutQuad);

		transform.
		DOMoveX(-GameData.Instance.combatConfig.positionXCharacter, GameData.Instance.combatConfig.waitCombatDuration).
		SetEase(Ease.OutQuad).
		SetDelay(GameData.Instance.combatConfig.transitionDuration);
	}

	public override void ActionStopCombat()
	{
		base.ActionStopCombat();

		transform.
		DOMove(StartPosition, GameData.Instance.combatConfig.transitionDuration).
		SetEase(Ease.OutQuad);
	}

	public override void CheckCharacters()
	{
		base.CheckCharacters();

		GameManager.Instance.combatManager.CheckGame(this);
	}

}