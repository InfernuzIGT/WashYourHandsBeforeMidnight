using DG.Tweening;
// using UnityEngine;

public class Enemy : CombatCharacter
{
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

}