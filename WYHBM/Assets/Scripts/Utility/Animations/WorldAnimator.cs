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
        _animValueZ.Execute(_animator, movement.y);
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
    
    public void MovementZipline(bool canZipline)
    {
        _animCanZipLine.Execute(_animator, canZipline);
    }

    public void ClimbLedge(bool value)
    {
        _animCanClimbLedge.Execute(_animator, value);
    }

    public void PreClimbLadder(bool canClimbLadder)
    {
        _animCanClimbLadder.Execute(_animator, canClimbLadder);
    }

    public void Falling(bool isFalling)
    {
        _animFall.Execute(_animator, isFalling);
    }
}