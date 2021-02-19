using System.Collections;
using DG.Tweening;
using Events;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Enemy : CombatCharacter
{
	private CombatEnemyEvent _combatEnemyEvent;

	private ItemSO _currentItem;

	public override void Start()
	{
		base.Start();

		_combatEnemyEvent = new CombatEnemyEvent();

		_combatRemoveCharacterEvent.character = this;
		_combatRemoveCharacterEvent.isPlayer = false;
	}

	public override void EffectReceiveDamage()
	{
		_vibrationEvent.type = RUMBLE_TYPE.Attack;
		EventController.TriggerEvent(_vibrationEvent);

	}

	public void Select()
	{
		int randomPlayer = Random.Range(0, GameData.Instance.GetListPlayer().Count);
		int randomAction = Random.Range(0, 2);

		_currentItem = randomAction == 0 ? Data.Equipment.actionA : Data.Equipment.actionB;
		GameData.Instance.GetListPlayer()[randomPlayer].Select(_currentItem);
		
		AnimationAction(randomAction == 0 ? ANIM_STATE.Attack_1 : ANIM_STATE.Attack_2);
		PlayActionSound(_currentItem);
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

		EventController.TriggerEvent(_combatEnemyEvent);

		yield return new WaitForSeconds(Random.Range(.25f, 1.25f));

		_isActionDone = false;

		Select();

		while (!_isActionDone)
		{
			yield return null;
		}

		MaterialShow(false);

		yield return _waitPerAction;

		AnimationAction(ANIM_STATE.Idle);
	}

	#endregion

}