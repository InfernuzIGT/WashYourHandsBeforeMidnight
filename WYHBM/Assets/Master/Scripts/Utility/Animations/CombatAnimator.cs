public class CombatAnimator : AnimatorController
{
    public void Action(ANIM_STATE animState)
    {
        _animActionType.Execute(_animator, (int)animState);
    }
}