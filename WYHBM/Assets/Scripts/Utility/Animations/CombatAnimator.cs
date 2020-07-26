public class CombatAnimator : AnimatorController
{
    private void Start()
    {
        _animModeCombat.Execute(_animator, true);
    }

    public void Action(ANIM_STATE animState)
    {
        _animActionType.Execute(_animator, (int)animState);
    }
}