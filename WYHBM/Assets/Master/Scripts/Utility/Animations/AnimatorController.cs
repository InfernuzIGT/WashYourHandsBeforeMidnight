using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    // General
    protected AnimationCommandBool _animIsAlive = new AnimIsAlive();
    protected AnimationCommandInt _animMovementType = new AnimMovementType();
    protected AnimationCommandBool _animIsWalking = new AnimIsWalking();
    protected AnimationCommandBool _animIsGrounded = new AnimIsGrounded();
    protected AnimationCommandBool _animIsFalling = new AnimIsFalling();
    protected AnimationCommandFloat _animValueX = new AnimValueX();
    protected AnimationCommandFloat _animValueY = new AnimValueY();
    protected AnimationCommandFloat _animValueZ = new AnimValueZ();
    protected AnimationCommandInt _animClimbType = new AnimClimbType();

    // Combat
    protected AnimationCommandInt _animActionType = new AnimActionType();
    
    // Systems
    protected AnimationCommandBool _animCanClimbLedge = new AnimCanClimbLedge();
    protected AnimationCommandBool _animCanClimbLadder = new AnimCanClimbLadder();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}