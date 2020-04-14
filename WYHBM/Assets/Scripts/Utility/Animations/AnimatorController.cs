using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour
{
    private enum AnimationState
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
        Jump = 3
    }

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

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private AnimationState _animationState = AnimationState.Idle;
    private AnimationState _currentState;

    private Material _material;
    private string _textureBase = "_MainTex";
    // private string _textureNormal = "_BumpMap";

    private bool _isFlipped;

    private AnimationCommandBool _animIsAlive = new AnimIsAlive();
    private AnimationCommandBool _animIsRunning = new AnimIsRunning();
    private AnimationCommandBool _animIsGrounded = new AnimIsGrounded();
    private AnimationCommandBool _animModeCombat = new AnimModeCombat();
    private AnimationCommandFloat _animValueX = new AnimValueX();
    private AnimationCommandFloat _animValueY = new AnimValueY();
    private AnimationCommandBool _animActionAttack = new AnimActionAttack();
    private AnimationCommandBool _animActionItem = new AnimActionItem();
    private AnimationCommandBool _animActionReceiveDamage = new AnimActionReceiveDamage();
    private AnimationCommandInt _animActionType = new AnimActionType();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
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
            _currentState = AnimationState.Idle;
        }
        else
        {
            if (isGrounded)
            {
                _currentState = isRunning ? AnimationState.Run : AnimationState.Walk;
            }
            else
            {
                _currentState = AnimationState.Jump;
            }
        }
    }

    private void SetTexture()
    {
        if (_animationState == _currentState)return;

        switch (_currentState)
        {
            case AnimationState.Idle:
                _material.SetTexture(_textureBase, textureIdle);
                // _material.SetTexture(_textureNormal, normalIdle);
                dither.SetTexture(_textureBase, textureIdle);
                break;

            case AnimationState.Walk:
                _material.SetTexture(_textureBase, textureWalk);
                // _material.SetTexture(_textureNormal, normalWalk);
                dither.SetTexture(_textureBase, textureWalk);
                break;

            case AnimationState.Run:
                _material.SetTexture(_textureBase, textureRun);
                // _material.SetTexture(_textureNormal, normalMovement);
                dither.SetTexture(_textureBase, textureRun);
                break;

            case AnimationState.Jump:
                _material.SetTexture(_textureBase, textureJump);
                // _material.SetTexture(_textureNormal, normalMovement);
                dither.SetTexture(_textureBase, textureJump);
                break;

            default:
                break;
        }

        _animationState = _currentState;
    }

}