using UnityEngine;

public abstract class AnimationCommand
{
    protected Animator _animator;

    // General
    protected readonly int hash_IsAlive = Animator.StringToHash("isAlive"); // Bool
    protected readonly int hash_MovementType = Animator.StringToHash("movementType"); // Int
    protected readonly int hash_IsWalking = Animator.StringToHash("isWalking"); // Bool
    protected readonly int hash_IsGrounded = Animator.StringToHash("isGrounded"); // Bool
    protected readonly int hash_isFalling = Animator.StringToHash("isFalling"); // Bool
    protected readonly int hash_ValueX = Animator.StringToHash("valueX"); // Float
    protected readonly int hash_ValueY = Animator.StringToHash("valueY"); // Float
    protected readonly int hash_RandomIdle = Animator.StringToHash("randomIdle"); // Int
    protected readonly int hash_SpecialAnimation = Animator.StringToHash("specialAnimation"); // Trigger

    // Combat
    protected readonly int hash_ActionType = Animator.StringToHash("actionType"); // Int
    protected readonly int hash_IsDetected = Animator.StringToHash("isDetected"); // Bool

    // DEPRECATED
    protected readonly int hash_ClimbType = Animator.StringToHash("climbType"); // Int
    protected readonly int hash_CanClimbLedge = Animator.StringToHash("canClimbLedge"); // Bool
    protected readonly int hash_CanClimbLadder = Animator.StringToHash("canClimbLadder"); // Bool

    public AnimationCommand(Animator animator)
    {
        _animator = animator;
    }
}

#region Animation Command - Float
public abstract class AnimationCommandFloat : AnimationCommand
{
    public AnimationCommandFloat(Animator animator) : base(animator) { }
    public abstract void Execute(float value);
}
#endregion

#region Animation Command - Int
public abstract class AnimationCommandInt : AnimationCommand
{
    public AnimationCommandInt(Animator animator) : base(animator) { }
    public abstract void Execute(int value);
}
#endregion

#region Animation Command - Bool
public abstract class AnimationCommandBool : AnimationCommand
{
    public AnimationCommandBool(Animator animator) : base(animator) { }
    public abstract void Execute(bool value);
}
#endregion

#region Animation Command - Trigger
public abstract class AnimationCommandTrigger : AnimationCommand
{
    public AnimationCommandTrigger(Animator animator) : base(animator) { }
    public abstract void Execute();
}
#endregion

//---------------------------------------------------------------------------

#region General

public class AnimIsAlive : AnimationCommandBool
{
    public AnimIsAlive(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_IsAlive, value);
    }
}

public class AnimMovementType : AnimationCommandInt
{
    public AnimMovementType(Animator animator) : base(animator) { }

    public override void Execute(int value)
    {
        _animator.SetInteger(hash_MovementType, value);
    }
}

public class AnimIsWalking : AnimationCommandBool
{
    public AnimIsWalking(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_IsWalking, value);
    }
}

public class AnimIsGrounded : AnimationCommandBool
{
    public AnimIsGrounded(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_IsGrounded, value);
    }
}

public class AnimIsFalling : AnimationCommandBool
{
    public AnimIsFalling(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_isFalling, value);
    }
}

public class AnimValueX : AnimationCommandFloat
{
    public AnimValueX(Animator animator) : base(animator) { }

    public override void Execute(float value)
    {
        _animator.SetFloat(hash_ValueX, value);
    }
}

public class AnimValueY : AnimationCommandFloat
{
    public AnimValueY(Animator animator) : base(animator) { }

    public override void Execute(float value)
    {
        _animator.SetFloat(hash_ValueY, value);
    }
}

public class AnimClimbType : AnimationCommandInt
{
    public AnimClimbType(Animator animator) : base(animator) { }

    public override void Execute(int value)
    {
        _animator.SetInteger(hash_ClimbType, value);
    }
}

public class AnimRandomIdle : AnimationCommandInt
{
    public AnimRandomIdle(Animator animator) : base(animator) { }

    public override void Execute(int value)
    {
        _animator.SetInteger(hash_RandomIdle, value);
    }
}

public class AnimSpecialAnimation : AnimationCommandTrigger
{
    public AnimSpecialAnimation(Animator animator) : base(animator) { }

    public override void Execute()
    {
        _animator.SetTrigger(hash_SpecialAnimation);
    }
}

#endregion

#region Combat

public class AnimActionType : AnimationCommandInt
{
    public AnimActionType(Animator animator) : base(animator) { }

    public override void Execute(int value)
    {
        _animator.SetInteger(hash_ActionType, value);
    }
}

public class AnimIsDetected : AnimationCommandBool
{
    public AnimIsDetected(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_IsDetected, value);
    }
}

#endregion

#region Systems

public class AnimCanClimbLedge : AnimationCommandBool
{
    public AnimCanClimbLedge(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_CanClimbLedge, value);
    }
}

public class AnimCanClimbLadder : AnimationCommandBool
{
    public AnimCanClimbLadder(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_CanClimbLadder, value);
    }
}

#endregion