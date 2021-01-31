using UnityEngine;

public class WorldAnimator : AnimatorController
{
    [Header("World")]
    [SerializeField] private bool flipSprite;

    private bool _isFlipped;

    public bool isFlipped { get { return _isFlipped; } }

    public void Movement(Vector3 movement, MOVEMENT_STATE movementState = MOVEMENT_STATE.Walk)
    {
        FlipSprite(movement.x);

        _animValueX.Execute(movement.x);
        _animValueY.Execute(movement.y);
        _animMovementType.Execute((int)movementState);
    }
    
    public void Movement(float valueX, float valueY, MOVEMENT_STATE movementState = MOVEMENT_STATE.Walk)
    {
        FlipSprite(valueX);

        _animValueX.Execute(valueX);
        _animValueY.Execute(valueY);
        _animMovementType.Execute((int)movementState);
    }

    private void FlipSprite(float valueX)
    {
        if (valueX < 0)
        {
            _isFlipped = flipSprite ? false : true;
        }
        else if (valueX > 0)
        {
            _isFlipped = flipSprite ? true : false;
        }

        _spriteRenderer.flipX = _isFlipped;
    }

    public void Walk(bool active)
    {
        _animIsWalking.Execute(active);
    }

    public void Falling(bool isFalling)
    {
        _animIsFalling.Execute(isFalling);
    }

    public void ClimbLedge(bool value)
    {
        _animCanClimbLedge.Execute(value);
    }

    public void PreClimbLadder(bool canClimbLadder)
    {
        _animCanClimbLadder.Execute(canClimbLadder);
    }

    public void Detected(bool isDetected)
    {
        _animIsDetected.Execute(isDetected);
    }

}