using UnityEngine;

public abstract class AnimationCommand
{
    protected readonly int hash_IsAlive = Animator.StringToHash("isAlive"); // Bool
    protected readonly int hash_IsRunning = Animator.StringToHash("isRunning"); // Bool
    protected readonly int hash_IsGrounded = Animator.StringToHash("isGrounded"); // Bool
    protected readonly int hash_ModeCombat = Animator.StringToHash("modeCombat"); // Bool
    protected readonly int hash_ValueX = Animator.StringToHash("valueX"); // Float
    protected readonly int hash_ValueY = Animator.StringToHash("valueY"); // Float
    protected readonly int hash_ActionAttack = Animator.StringToHash("actionAttack"); // Bool
    protected readonly int hash_ActionItem = Animator.StringToHash("actionItem"); // Bool
    protected readonly int hash_ActionReceiveDamage = Animator.StringToHash("actionReceiveDamage"); // Bool
    protected readonly int hash_ActionType = Animator.StringToHash("actionType"); // Int
}

#region Animation Command - Float
public abstract class AnimationCommandFloat : AnimationCommand
{
    public abstract void Execute(Animator anim, float value);
}
#endregion

#region Animation Command - Int
public abstract class AnimationCommandInt : AnimationCommand
{
    public abstract void Execute(Animator anim, int value);
}
#endregion

#region Animation Command - Bool
public abstract class AnimationCommandBool : AnimationCommand
{
    public abstract void Execute(Animator anim, bool value);
}
#endregion

#region Animation Command - Trigger
public abstract class AnimationCommandTrigger : AnimationCommand
{
    public abstract void Execute(Animator anim);
}
#endregion

//---------------------------------------------------------------------------

public class AnimIsAlive : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_IsAlive, value);
    }
}

public class AnimIsRunning : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_IsRunning, value);
    }
}

public class AnimIsGrounded : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_IsGrounded, value);
    }
}

public class AnimModeCombat : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_ModeCombat, value);
    }
}

public class AnimValueX : AnimationCommandFloat
{
    public override void Execute(Animator anim, float value)
    {
        anim.SetFloat(hash_ValueX, value);
    }
}

public class AnimValueY : AnimationCommandFloat
{
    public override void Execute(Animator anim, float value)
    {
        anim.SetFloat(hash_ValueY, value);
    }
}

public class AnimActionAttack : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_ActionAttack, value);
    }
}

public class AnimActionItem : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_ActionItem, value);
    }
}

public class AnimActionReceiveDamage : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_ActionReceiveDamage, value);
    }
}

public class AnimActionType : AnimationCommandInt
{
    public override void Execute(Animator anim, int value)
    {
        anim.SetInteger(hash_ActionType, value);
    }
}