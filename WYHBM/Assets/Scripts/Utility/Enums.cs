﻿public enum ANIM_STATE
{
    Idle = 0,
    Hit = 1,
    Death = 2,
    AttackMelee = 3,
    AttackOneHand = 4,
    AttackTwoHands = 5,
    ItemHeal = 6,
    ItemDefense = 7,
    ItemGrenade = 8
}

public enum COMBAT_STATE
{
    None = 0,
    SelectAction = 1,
    SelectCharacter = 2,
    MakeAction = 3,
    Wait = 4
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

public enum NPC_INTERACTION_TYPE
{
    none = 0,
    dialog = 1,
    fight = 2,
    dialogAndFight = 3
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

public enum MENU_TYPE
{
    None = 0,
    Continue = 1,
    NewGame = 2,
    Options = 3,
    Credits = 4,
    Back = 6,
    Exit = 7,
    YesPopup = 8,
}

public enum QUEST_STATE
{
    None = 0,
    Ready = 1,
    InProgress = 2,
    Completed = 3
}

public enum DIALOG_TYPE
{
    None = 0,
    Ready = 1,
    InProgress = 2,
    Completed = 3
}

public enum LANGUAGE
{
    English = 0,
    Spanish = 1
}

public enum SCENE_INDEX
{
    MainMenu = 0,
    Master = 1,
}