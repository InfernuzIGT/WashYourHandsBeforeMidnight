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

    public override void RemoveCharacter()
    {
        base.RemoveCharacter();
        GameManager.Instance.combatManager.RemoveCharacter(this);
    }

    #region Animation

    public override void AnimationActionStart()
    {
        base.AnimationActionStart();

        transform.
        DOMove(transform.position - _combatConfig.positionAction, _combatConfig.animationDuration).
        SetEase(Ease.OutQuad);

        // transform.
        // DOMoveX(_combatConfig.positionXCharacter, _combatConfig.waitCombatDuration).
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

        GameManager.Instance.PlayerCanSelect(true, _combatIndex);

        _isActionDone = false;

        while (!_isActionDone)
        {
            yield return null;
        }

        Shake();

        GameManager.Instance.PlayerCanSelect(false);

        // GameManager.Instance.ReorderTurn();

        yield return _waitPerAction;

        MaterialShow(false);

        AnimationAction(ANIM_STATE.Idle);

        // GameManager.Instance.combatUI.EnableActions(true);
    }

    #endregion

}