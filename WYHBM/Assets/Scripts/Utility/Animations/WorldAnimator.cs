using UnityEngine;

public class WorldAnimator : AnimatorController
{
    public bool flipSprite;

    private bool _isFlipped;

    public bool isFlipped { get {return _isFlipped; } }

    private void Start()
    {
        _animModeCombat.Execute(_animator, false);
    }

    public void Movement(Vector3 movement, bool isRunning = false, bool isGrounded = true)
    {
        FlipSprite(movement);

        _animValueX.Execute(_animator, movement.x);
        _animValueY.Execute(_animator, movement.z);
        _animIsRunning.Execute(_animator, isRunning);
        _animIsGrounded.Execute(_animator, isGrounded);
    }

    private void FlipSprite(Vector3 movement)
    {
        if (movement.x < 0)
        {
            _isFlipped = flipSprite ? false : true;
        }
        else if (movement.x > 0)
        {
            _isFlipped = flipSprite ? true : false;
        }

        _spriteRenderer.flipX = _isFlipped;
    }
}