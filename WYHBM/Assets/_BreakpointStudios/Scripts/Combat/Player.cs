using System.Collections;
using DG.Tweening;
using Events;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Player : CombatCharacter
{
    private CombatPlayerEvent _combatPlayerEvent;

    public override void Start()
    {
        base.Start();

        _combatPlayerEvent = new CombatPlayerEvent();

        _combatRemoveCharacterEvent.character = this;
        _combatRemoveCharacterEvent.isPlayer = true;
    }

    public override void EffectReceiveDamage()
    {
        EventController.TriggerEvent(_shakeEvent);

        _vibrationEvent.type = RUMBLE_TYPE.Hit;
        EventController.TriggerEvent(_vibrationEvent);
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

        _combatPlayerEvent.canSelect = true;
        _combatPlayerEvent.combatIndex = _combatIndex;
        EventController.TriggerEvent(_combatPlayerEvent);

        _isActionDone = false;

        while (!_isActionDone)
        {
            yield return null;
        }

        _combatPlayerEvent.canSelect = false;
        EventController.TriggerEvent(_combatPlayerEvent);

        MaterialShow(false);

        yield return _waitPerAction;

        AnimationAction(ANIM_STATE.Idle);
    }

    #endregion

}