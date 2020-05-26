using UnityEngine;

public class CombatAnimator : AnimatorController
{
    private ANIM_STATE _currentState;

    private void Start()
    {
        _animModeCombat.Execute(_animator, true);
    }

    public void Action(ANIM_STATE combatState)
    {
        _animActionType.Execute(_animator, (int)combatState);
    }
}