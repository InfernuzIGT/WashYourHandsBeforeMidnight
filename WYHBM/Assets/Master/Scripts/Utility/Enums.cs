public enum ANIM_STATE
{
    Idle = 0,
    Hit = 1,
    Death = 2,
    Action_A = 3,
    Action_B = 4,
    Action_Item = 5
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
    Jump = 3,
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
    WeaponMelee = 1,
    WeaponOneHand = 2,
    WeaponTwoHands = 3,
    ItemHeal = 4,
    ItemDefense = 5,
    ItemGrenade = 6
}

public enum LADDER_EXIT
{
    Interaction = 0,
    Bot = 1,
    Top = 2,
}

public enum BUTTON_TYPE
{
    None = 0,
    Diary = 1,
    Inventory = 2,
    System = 3,
    Resume = 4,
    Options = 5,
    Quit = 6,
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

public enum DIALOG_TYPE
{
    None = 0,
    Ready = 1,
    InProgress = 2,
    Completed = 3
}

public enum SCENE_INDEX
{
    MainMenu = 0,
    Master = 1,
}

// public enum PLAYER
// {
//     Sam = 0,
//     Dallas = 1,
//     Will = 2
// }