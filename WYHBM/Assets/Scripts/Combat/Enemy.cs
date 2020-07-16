using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Enemy : CombatCharacter
{
	public override void Start()
	{
		base.Start();
	}

	public void Select()
	{
		int randomPlayer = Random.Range(0, GameManager.Instance.combatManager.listPlayers.Count);

		GameManager.Instance.combatManager.listPlayers[randomPlayer].ActionReceiveDamage();
		GameManager.Instance.combatManager.listPlayers[randomPlayer].AnimationRecovery();

		// TODO Mariano: Agregar animacion correspondiente
		AnimationAction(ANIM_STATE.AttackMelee);

		DoAction();
	}

	public override void CheckCharacters()
	{
		base.CheckCharacters();

		GameManager.Instance.combatManager.CheckGame(this);
	}

	#region Animation

	public override void AnimationActionStart()
	{
		base.AnimationActionStart();

		transform.
		DOMove(transform.position + GameData.Instance.combatConfig.positionAction, GameData.Instance.combatConfig.animationDuration).
		SetEase(Ease.OutQuad);

		// transform.
		// DOMoveX(-GameData.Instance.combatConfig.positionXCharacter, GameData.Instance.combatConfig.waitCombatDuration).
		// SetEase(Ease.OutQuad).
		// SetDelay(GameData.Instance.combatConfig.transitionDuration);
	}

	public override void AnimationActionEnd()
	{
		base.AnimationActionEnd();

		transform.
		DOMove(StartPosition, GameData.Instance.combatConfig.animationDuration).
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

		GameManager.Instance.combatUI.ShowPlayerPanel(true, false);

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