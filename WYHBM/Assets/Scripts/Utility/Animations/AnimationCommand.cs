using UnityEngine;

public abstract class AnimationCommand
{
    // General
    protected readonly int hash_IsAlive = Animator.StringToHash("isAlive"); // Bool
    protected readonly int hash_IsRunning = Animator.StringToHash("isRunning"); // Bool
    protected readonly int hash_IsGrounded = Animator.StringToHash("isGrounded"); // Bool
    protected readonly int hash_ModeCombat = Animator.StringToHash("modeCombat"); // Bool
    protected readonly int hash_ValueX = Animator.StringToHash("valueX"); // Float
    protected readonly int hash_ValueY = Animator.StringToHash("valueY"); // Float

    // Combat
    protected readonly int hash_ActionType = Animator.StringToHash("actionType"); // Int

    // Interaction
    protected readonly int hash_InteractionBool = Animator.StringToHash("interactionBool"); // Bool
    protected readonly int hash_InteractionTrigger = Animator.StringToHash("interactionTrigger"); // Trigger

    // Door
    protected readonly int hash_DoorIsOpening = Animator.StringToHash("isOpening"); // Bool
    protected readonly int hash_DoorIsRunning = Animator.StringToHash("isRunning"); // Bool
    protected readonly int hash_DoorIsLocked = Animator.StringToHash("isLocked"); // Trigger
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

#region Interaction

public class AnimInteractionBool : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_InteractionBool, value);
    }
}

public class AnimInteractionTrigger : AnimationCommandTrigger
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger(hash_InteractionTrigger);
    }
}

#endregion

#region Door

public class AnimDoorIsOpening : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_DoorIsOpening, value);
    }
}

public class AnimDoorIsRunning : AnimationCommandBool
{
    public override void Execute(Animator anim, bool value)
    {
        anim.SetBool(hash_DoorIsRunning, value);
    }
}

public class AnimDoorIsLocked : AnimationCommandTrigger
{
    public override void Execute(Animator anim)
    {
        anim.SetTrigger(hash_DoorIsLocked);
    }
}

#endregion