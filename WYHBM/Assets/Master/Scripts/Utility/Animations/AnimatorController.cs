using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour
{
    [Header("Animator")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] protected Animator _animator;
    [SerializeField, ConditionalHide] protected SpriteRenderer _spriteRenderer;


    // General
    protected AnimationCommandBool _animIsAlive;
    protected AnimationCommandInt _animMovementType;
    protected AnimationCommandBool _animIsWalking;
    protected AnimationCommandBool _animIsGrounded;
    protected AnimationCommandBool _animIsFalling;
    protected AnimationCommandFloat _animValueX;
    protected AnimationCommandFloat _animValueY;
    protected AnimationCommandInt _animRandomIdle;
    protected AnimationCommandTrigger _animSpecialAnimation;

    // Combat
    protected AnimationCommandInt _animActionType;
    protected AnimationCommandBool _animIsDetected;

    // DEPRECATED
    protected AnimationCommandInt _animClimbType;
    protected AnimationCommandBool _animCanClimbLedge;
    protected AnimationCommandBool _animCanClimbLadder;

    private void Awake()
    {
        // General
        _animIsAlive = new AnimIsAlive(_animator);
        _animMovementType = new AnimMovementType(_animator);
        _animIsWalking = new AnimIsWalking(_animator);
        _animIsGrounded = new AnimIsGrounded(_animator);
        _animIsFalling = new AnimIsFalling(_animator);
        _animValueX = new AnimValueX(_animator);
        _animValueY = new AnimValueY(_animator);
        _animRandomIdle = new AnimRandomIdle(_animator);
        _animSpecialAnimation = new AnimSpecialAnimation(_animator);

        // Combat
        _animActionType = new AnimActionType(_animator);
        _animIsDetected = new AnimIsDetected(_animator);

        // DEPRECATED
        _animClimbType = new AnimClimbType(_animator);
        _animCanClimbLedge = new AnimCanClimbLedge(_animator);
        _animCanClimbLadder = new AnimCanClimbLadder(_animator);

    }
}