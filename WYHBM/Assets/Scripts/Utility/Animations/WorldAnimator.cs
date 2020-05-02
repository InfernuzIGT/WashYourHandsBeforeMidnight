using UnityEngine;

public class WorldAnimator : AnimatorController
{
    [Header("Textures")]
    public Texture2D textureIdle;
    public Texture2D textureWalk;
    public Texture2D textureRun;
    public Texture2D textureJump;

    // public Texture2D normalIdle;
    // public Texture2D normalWalk;
    // public Texture2D normalRun;
    // public Texture2D normalJump;

    [Header("Material")]
    public Material dither;

    private bool _isFlipped;

    private MOVEMENT_STATE _movementState = MOVEMENT_STATE.Idle;
    private MOVEMENT_STATE _currentState;

    private void Start()
    {
        _animModeCombat.Execute(_animator, false);
    }

    public override void SetTexture()
    {
        base.SetTexture();

        switch (_currentState)
        {
            case MOVEMENT_STATE.Idle:
                _material.SetTexture(_textureBase, textureIdle);
                // _material.SetTexture(_textureNormal, normalIdle);
                dither.SetTexture(_textureBase, textureIdle);
                break;

            case MOVEMENT_STATE.Walk:
                _material.SetTexture(_textureBase, textureWalk);
                // _material.SetTexture(_textureNormal, normalWalk);
                dither.SetTexture(_textureBase, textureWalk);
                break;

            case MOVEMENT_STATE.Run:
                _material.SetTexture(_textureBase, textureRun);
                // _material.SetTexture(_textureNormal, normalMovement);
                dither.SetTexture(_textureBase, textureRun);
                break;

            case MOVEMENT_STATE.Jump:
                _material.SetTexture(_textureBase, textureJump);
                // _material.SetTexture(_textureNormal, normalMovement);
                dither.SetTexture(_textureBase, textureJump);
                break;

            default:
                break;
        }

        _movementState = _currentState;
    }

    public void Movement(Vector3 movement, bool isRunning = false, bool isGrounded = true)
    {
        FlipSprite(movement.x);
        ChangeState(movement, isRunning, isGrounded);
        SetTexture();

        _animValueX.Execute(_animator, movement.x);
        _animValueY.Execute(_animator, movement.z);
        _animIsRunning.Execute(_animator, isRunning);
        _animIsGrounded.Execute(_animator, isGrounded);
    }

    private void FlipSprite(float valueX)
    {
        if (valueX < 0)
        {
            _isFlipped = true;
        }
        else if (valueX > 0)
        {
            _isFlipped = false;
        }

        _spriteRenderer.flipX = _isFlipped;
    }

    private void ChangeState(Vector3 movement, bool isRunning, bool isGrounded)
    {
        if (movement.x == 0 && movement.z == 0 && isGrounded)
        {
            _currentState = MOVEMENT_STATE.Idle;
        }
        else
        {
            if (isGrounded)
            {
                _currentState = isRunning ? MOVEMENT_STATE.Run : MOVEMENT_STATE.Walk;
            }
            else
            {
                _currentState = MOVEMENT_STATE.Jump;
            }
        }
    }
}