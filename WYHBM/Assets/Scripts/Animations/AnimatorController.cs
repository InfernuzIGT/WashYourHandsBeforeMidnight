using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    private Animator _animator;

    private AnimationCommandBool _animIsAlive = new AnimIsAlive ();
    private AnimationCommandBool _animModeCombat= new AnimModeCombat();
    private AnimationCommandFloat _animValueX = new AnimValueX();
    private AnimationCommandFloat _animValueY = new AnimValueY ();
    private AnimationCommandBool _animActionAttack = new AnimActionAttack ();
    private AnimationCommandBool _animActionItem = new AnimActionItem ();
    private AnimationCommandBool _animActionReceiveDamage = new AnimActionReceiveDamage ();
    private AnimationCommandInt _animActionType = new AnimActionType ();

    private void Awake ()
    {
        _animator = GetComponent<Animator> ();
    }

    public void Movement (float valueX, float valueY)
    {
        _animValueX.Execute (_animator, valueX);
        _animValueY.Execute (_animator, valueY);
    }

    
}
