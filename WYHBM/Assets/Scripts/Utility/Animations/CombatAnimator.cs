using UnityEngine;

public class CombatAnimator : AnimatorController
{
    [Header("Textures")]
    public Texture2D textureIdle;
    public Texture2D textureAttack;
    public Texture2D textureDefense;
    public Texture2D textureItem;

    // public Texture2D normalIdle;
    // public Texture2D normalWalk;
    // public Texture2D normalRun;
    // public Texture2D normalJump;

    private COMBAT_STATE _currentState;

    private void Start()
    {
        _animModeCombat.Execute(_animator, true);
    }

    public override void SetTexture()
    {
        base.SetTexture();

        switch (_currentState)
        {
            case COMBAT_STATE.Idle:
                _material.SetTexture(_textureBase, textureIdle);
                // _material.SetTexture(_textureNormal, normalIdle);
                break;

            case COMBAT_STATE.Attack:
                _material.SetTexture(_textureBase, textureAttack);
                // _material.SetTexture(_textureNormal, normalIdle);
                break;

            case COMBAT_STATE.Defense:
                _material.SetTexture(_textureBase, textureDefense);
                // _material.SetTexture(_textureNormal, normalIdle);
                break;

            case COMBAT_STATE.Item:
                _material.SetTexture(_textureBase, textureItem);
                // _material.SetTexture(_textureNormal, normalIdle);
                break;

            default:
                break;
        }
    }

    public void Action(COMBAT_STATE combatState)
    {
        _currentState = combatState;

        SetTexture();

        _animActionType.Execute(_animator, (int)combatState);
    }
}