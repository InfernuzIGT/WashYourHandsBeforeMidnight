public enum ANIM_STATE
{
    Idle = 0,
    Attack_1 = 1,
    Attack_2 = 2,
    Item = 3,
    Hit = 4,
    Dead = 5,
    Dodge = 6,
}

public enum COMBAT_STATE
{
    None = 0,
    SelectAction = 1,
    SelectCharacter = 2,
    MakeAction = 3,
    Wait = 4
}

public enum MOVEMENT_STATE
{
    Walk = 0,
    Run = 1,
    Crouch = 2,
    ClimbShort = 3,
    ClimbLarge = 4,
    Ladder = 5,
}

public enum DIRECTION
{
    UP = 0,
    DOWN = 1,
    LEFT = 2,
    RIGHT = 3
}

public enum DIALOG_STATE
{
    Ready = 0,
    InProgress = 1,
    Done = 2,
    Locked = 3
}

// public enum TIER
// {
//     none = 0,
//     common = 1,
//     uncommon = 2,
//     rare = 3,
//     epic = 4,
//     legendary = 5,
// }

public enum ITEM_TYPE
{
    None = 0,
    ActionA = 1,
    ActionB = 2,
    ActionHeal = 3,
    ActionDefense = 4,
    ActionItem = 5
}

public enum LADDER_EXIT
{
    Interaction = 0,
    Bot = 1,
    Top = 2,
}

public enum PAUSE_TYPE
{
    None = 0,
    PauseMenu = 1,
    Inventory = 2,
    Note = 3,
}

public enum UI_TYPE
{
    None = 0,
    Play = 1,
    Options = 2,
    Quit = 3,
}