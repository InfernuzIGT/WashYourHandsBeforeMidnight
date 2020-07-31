using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected AnimationCommandBool _animIsAlive = new AnimIsAlive();
    protected AnimationCommandBool _animIsRunning = new AnimIsRunning();
    protected AnimationCommandBool _animIsGrounded = new AnimIsGrounded();
    protected AnimationCommandBool _animModeCombat = new AnimModeCombat();
    protected AnimationCommandFloat _animValueX = new AnimValueX();
    protected AnimationCommandFloat _animValueY = new AnimValueY();
    protected AnimationCommandFloat _animValueZ = new AnimValueZ();
    protected AnimationCommandInt _animActionType = new AnimActionType();
    protected AnimationCommandBool _animCanZipLine = new AnimCanZipline();
    protected AnimationCommandBool _animCanClimbLedge = new AnimCanClimbLedge();
    protected AnimationCommandBool _animCanClimbLadder = new AnimCanClimbLadder();
    protected AnimationCommandBool _animFall = new AnimFall();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}