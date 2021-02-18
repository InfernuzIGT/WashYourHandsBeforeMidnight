public class ObjectAnimator : AnimatorController
{
    private bool _isTrue;

    public void Execute(bool isTrigger, bool instant = false)
    {
        if (isTrigger)
        {
            SetTrigger(instant);
        }
        else
        {
            SetBool(instant);
        }
    }

    private void SetTrigger(bool instant = false)
    {
        if (!instant)
        {
            _animObjectTrigger.Execute();
        }
        else
        {
            _animObjectTrigger.ExecuteInstant();
        }
    }

    private void SetBool(bool instant = false)
    {
        _isTrue = !_isTrue;

        if (!instant)
        {
            _animObjectBool.ExecuteInstant(_isTrue);
        }
        else
        {
            _animObjectBool.Execute(_isTrue);
        }
    }
}