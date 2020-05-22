using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Player : CombatCharacter
{
    public override void Start()
    {
        base.Start();
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
        DOMove(transform.position - GameData.Instance.combatConfig.positionAction, GameData.Instance.combatConfig.animationDuration).
        SetEase(Ease.OutQuad);

        // transform.
        // DOMoveX(GameData.Instance.combatConfig.positionXCharacter, GameData.Instance.combatConfig.waitCombatDuration).
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

        GameManager.Instance.combatUI.ShowPlayerPanel(true);

        _isActionDone = false;

        while (!_isActionDone)
        {
            yield return null;
        }

        yield return _waitPerAction;

        GameManager.Instance.combatUI.ShowPlayerPanel(false);
        
        AnimationAction(COMBAT_STATE.Idle);
    }

    #endregion

}