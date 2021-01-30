using UnityEngine;

public class WorldAnimator : AnimatorController
{
    public bool flipSprite;

    private bool _isFlipped;

    public bool isFlipped { get { return _isFlipped; } }

    public void Movement(Vector3 movement, MOVEMENT_STATE movementState = MOVEMENT_STATE.Walk)
    {
        FlipSprite(movement);

        _animValueX.Execute(_animator, movement.x);
        _animValueY.Execute(_animator, movement.y);
        _animMovementType.Execute(_animator, (int)movementState);
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
    
    public void Walk(bool active)
    {
        _animIsWalking.Execute(_animator, active);
    }

    public void Falling(bool isFalling)
    {
        _animIsFalling.Execute(_animator, isFalling);
    }

    public void ClimbLedge(bool value)
    {
        _animCanClimbLedge.Execute(_animator, value);
    }

    public void PreClimbLadder(bool canClimbLadder)
    {
        _animCanClimbLadder.Execute(_animator, canClimbLadder);
    }

}