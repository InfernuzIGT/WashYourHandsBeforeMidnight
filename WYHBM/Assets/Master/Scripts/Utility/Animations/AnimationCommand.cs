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

    // Object
    protected readonly int hash_ObjectTrigger = Animator.StringToHash("objectTrigger"); // Trigger
    protected readonly int hash_ObjectBool = Animator.StringToHash("objectBool"); // Bool
    protected readonly int hash_AnimationTrigger = Animator.StringToHash("Trigger"); // Trigger
    protected readonly int hash_AnimationBool = Animator.StringToHash("Bool"); // Bool

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
    public abstract void ExecuteInstant(bool value);
}
#endregion

#region Animation Command - Trigger
public abstract class AnimationCommandTrigger : AnimationCommand
{
    public AnimationCommandTrigger(Animator animator) : base(animator) { }
    public abstract void Execute();
    public abstract void ExecuteInstant();
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

    public override void ExecuteInstant(bool value) { }
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

    public override void ExecuteInstant(bool value) { }

}

public class AnimIsGrounded : AnimationCommandBool
{
    public AnimIsGrounded(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_IsGrounded, value);
    }

    public override void ExecuteInstant(bool value) { }

}

public class AnimIsFalling : AnimationCommandBool
{
    public AnimIsFalling(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_isFalling, value);
    }

    public override void ExecuteInstant(bool value) { }

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

    public override void ExecuteInstant() { }

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

    public override void ExecuteInstant(bool value) { }

}

#endregion

#region Object

public class AnimObjectTrigger : AnimationCommandTrigger
{
    public AnimObjectTrigger(Animator animator) : base(animator) { }

    public override void Execute()
    {
        _animator.SetTrigger(hash_ObjectTrigger);
    }

    public override void ExecuteInstant()
    {
        Execute();
        _animator.Play(hash_AnimationTrigger, 0, 1);
    }
}

public class AnimObjectBool : AnimationCommandBool
{
    public AnimObjectBool(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_ObjectBool, value);
    }

    public override void ExecuteInstant(bool value)
    {
        Execute(value);
        _animator.Play(hash_AnimationBool, 0, 1);
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

    public override void ExecuteInstant(bool value) { }

}

public class AnimCanClimbLadder : AnimationCommandBool
{
    public AnimCanClimbLadder(Animator animator) : base(animator) { }

    public override void Execute(bool value)
    {
        _animator.SetBool(hash_CanClimbLadder, value);
    }

    public override void ExecuteInstant(bool value) { }

}

#endregion