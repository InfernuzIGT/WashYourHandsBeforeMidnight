public class CombatAnimator : AnimatorController
{
    public void Action(ANIM_STATE animState)
    {
        _animActionType.Execute((int)animState);
    }
}