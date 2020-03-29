using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimatorController : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

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
    }

    public void Movement(float valueX, float valueY)
    {
        FlipSprite(valueX);
        
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

}