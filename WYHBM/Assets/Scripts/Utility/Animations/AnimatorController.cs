using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour
{
    private enum AnimationState
    {
        Idle = 0,
        Walk = 1,
        Run = 2
    }

    [Header("Textures")]
    public Texture2D textureIdle;
    // public Texture2D textureWalk;
    public Texture2D textureMovement;

    // public Texture2D normalIdle;
    // public Texture2D normalWalk;
    // public Texture2D normalMovement;

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

    public void Movement(float valueX, float valueY)
    {
        FlipSprite(valueX);
        SetTexture(valueX, valueY);

        _animValueX.Execute(_animator, valueX);
        _animValueY.Execute(_animator, valueY);
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

    private void SetTexture(float valueX, float valueY)
    {
        if (valueX == 0 && valueY == 0)
        {
            _currentState = AnimationState.Idle;
        }
        else
        {
            _currentState = AnimationState.Run;
        }

        if (_animationState == _currentState)return;

        switch (_currentState)
        {
            case AnimationState.Idle:
                _material.SetTexture(_textureBase, textureIdle);
                // _material.SetTexture(_textureNormal, normalIdle);
                dither.SetTexture(_textureBase, textureIdle);
                break;

                // case AnimationState.Walk:
                //     _material.SetTexture(_textureBase, spriteWalk);
                //     _material.SetTexture(_textureNormal, normalWalk);
                //     dither.SetTexture(_textureBase, spriteWalk);
                //     break;

            case AnimationState.Run:
                _material.SetTexture(_textureBase, textureMovement);
                // _material.SetTexture(_textureNormal, normalMovement);
                dither.SetTexture(_textureBase, textureMovement);
                break;

            default:
                break;
        }

        _animationState = _currentState;
    }

}