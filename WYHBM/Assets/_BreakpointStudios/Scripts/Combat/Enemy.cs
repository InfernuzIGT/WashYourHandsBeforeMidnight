using System.Collections;
using DG.Tweening;
using Events;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Enemy : CombatCharacter
{
	private CombatEnemyEvent _combatEnemyEvent;

	public override void Start()
	{
		base.Start();

		_combatEnemyEvent = new CombatEnemyEvent();

		_combatRemoveCharacterEvent.character = this;
		_combatRemoveCharacterEvent.isPlayer = false;
	}

	public void Select()
	{
		int randomPlayer = Random.Range(0, GameData.Instance.GetListPlayer().Count);
		int randomAction = Random.Range(0, 2);

		GameData.Instance.GetListPlayer()[randomPlayer].Select(randomAction == 0 ? Data.Equipment.actionA : Data.Equipment.actionB);
		AnimationAction(randomAction == 0 ? ANIM_STATE.Attack_1 : ANIM_STATE.Attack_2);

		DoAction();
	}

	#region Animation

	public override void AnimationActionStart()
	{
		base.AnimationActionStart();

		transform.
		DOMove(transform.position + _combatConfig.positionAction, _combatConfig.animationDuration).
		SetEase(Ease.OutQuad);

		// transform.
		// DOMoveX(-_combatConfig.positionXCharacter, _combatConfig.waitCombatDuration).
		// SetEase(Ease.OutQuad).
		// SetDelay(_combatConfig.transitionDuration);
	}

	public override void AnimationActionEnd()
	{
		base.AnimationActionEnd();

		transform.
		DOMove(StartPosition, _combatConfig.animationDuration).
		SetEase(Ease.OutQuad);
	}

	#endregion

	#region Turn System

	/// <summary>
	/// Espera la accion
	/// </summary>
	public override IEnumerator WaitingForAction()
	{
		base.WaitingForAction();

		MaterialShow(true);

		// GameManager.Instance.combatUI.ShowPlayerPanel(true, false);
		EventController.TriggerEvent(_combatEnemyEvent);

		yield return new WaitForSeconds(Random.Range(.25f, 1.25f));

		_isActionDone = false;

		Select();

		while (!_isActionDone)
		{
			yield return null;
		}

		Shake();

		// GameManager.Instance.ReorderTurn();

		yield return _waitPerAction;

		MaterialShow(false);

		AnimationAction(ANIM_STATE.Idle);
	}

	#endregion

}