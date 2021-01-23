using UnityEngine;

public abstract class AnimationCommand
{
    // General
    protected readonly int hash_IsAlive = Animator.StringToHash("isAlive"); // Bool
    protected readonly int hash_MovementType = Animator.StringToHash("movementType"); // Int
    protected readonly int hash_IsWalking = Animator.StringToHash("isWalking"); // Bool
    protected readonly int hash_IsGrounded = Animator.StringToHash("isGrounded"); // Bool
    protected readonly int hash_isFalling = Animator.StringToHash("isFalling"); // Bool
    protected readonly int hash_ValueX = Animator.StringToHash("valueX"); // Float
    protected readonly int hash_ValueY = Animator.StringToHash("valueY"); // Float
    protected readonly int hash_ValueZ = Animator.StringToHash("valueZ"); // Float
    protected readonly int hash_ClimbType = Animator.StringToHash("climbType"); // Int

    // Combat
    protected readonly int hash_ActionType = Animator.StringToHash("actionType"); // Int

    // Systems
    protected readonly int hash_CanClimbLedge = Animator.StringToHash("canClimbLedge"); // Bool
    protected readonly int hash_CanClimbLadder = Animator.StringToHash("canClimbLadder"); // Bool

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

#region General

public class AnimIsAlive : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_IsAlive, value);
    }
}

public class AnimMovementType : AnimationCommandInt
{
    public override void Execute(Animator anim, int value)
    {
        anim.SetInteger(hash_MovementType, value);
    }
}

public class AnimIsWalking : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_IsWalking, value);
    }
}

public class AnimIsGrounded : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_IsGrounded, value);
    }
}

public class AnimIsFalling : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_isFalling, value);
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

public class AnimValueZ : AnimationCommandFloat
{
    public override void Execute(Animator anim, float value)
    {
        anim.SetFloat(hash_ValueZ, value);
    }
}

public class AnimClimbType : AnimationCommandInt
{
    public override void Execute(Animator anim, int value)
    {
        anim.SetInteger(hash_ClimbType, value);
    }
}

#endregion

#region Combat

public class AnimActionType : AnimationCommandInt
{
    public override void Execute(Animator anim, int value)
    {
        anim.SetInteger(hash_ActionType, value);
    }
}

#endregion

#region Systems

public class AnimCanClimbLedge : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_CanClimbLedge, value);
    }
}

public class AnimCanClimbLadder : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_CanClimbLadder, value);
    }
}

#endregion