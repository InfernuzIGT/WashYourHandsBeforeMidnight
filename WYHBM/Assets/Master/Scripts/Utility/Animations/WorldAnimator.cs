using UnityEngine;

public class WorldAnimator : AnimatorController
{
    [SerializeField, ConditionalHide] private SpriteRenderer _spriteRenderer;
    [SerializeField, ConditionalHide] private Material _matDither;

    private Material _matCharacter;
    private int _indexRandomIdle;

    private bool _isFlipped;
    public bool isFlipped { get { return _isFlipped; } }

    private int hash_FlipUV = Shader.PropertyToID("_FlipUV");

    private void Start()
    {
        _matCharacter = _spriteRenderer.material;
        // _matDither = Instantiate(_matDither); // TODO Mariano: Se debe destruir con Resources.UnloadUnusedAssets 
    }

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

    public void FlipSprite()
    {
        _isFlipped = !_isFlipped;

        _matCharacter.SetFloat(hash_FlipUV, _isFlipped ? 1 : 0);
        _matDither.SetFloat(hash_FlipUV, _isFlipped ? 1 : 0);
    }

    public void FlipSprite(float valueX)
    {
        if (valueX < 0)
        {
            _isFlipped = true;
        }
        else if (valueX > 0)
        {
            _isFlipped = false;
        }

        _matCharacter.SetFloat(hash_FlipUV, _isFlipped ? 1 : 0);
        _matDither.SetFloat(hash_FlipUV, _isFlipped ? 1 : 0);
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

    public void RandomIdle()
    {
        _indexRandomIdle = Random.Range(1, 4);
        _animRandomIdle.Execute(_indexRandomIdle);

        _indexRandomIdle = 0;
        _animRandomIdle.Execute(_indexRandomIdle);
    }

    public void SpecialAnimation()
    {
        _animSpecialAnimation.Execute();
    }
}