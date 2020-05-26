#region Character

public enum CHARACTER_TYPE
{
    none = 0,
    human = 1,
    infected = 2
}

public enum CHARACTER_BEHAVIOUR
{
    none = 0,
    aggressive = 1,
    defensive = 2
}

#endregion

#region Animator

public enum COMBAT_STATE
{
    Idle = 0,
    Hit = 1,
    Death = 2,
    Attack = 3,
    Defense = 4,
    Item = 5
}

#endregion

#region Equipment

public enum TIER
{
    none = 0,
    common = 1,
    uncommon = 2,
    rare = 3,
    epic = 4,
    legendary = 5,
}

public enum ITEM_TYPE
{
    None = 0,
    Weapon = 1,
    Defense = 2,
    Heal = 3,
    Damage = 4
}

public enum WEAPON_TYPE
{
    none = 0,
    melee = 1,
    pistol = 2,
    rifle = 3,
    shotgun = 3,
}

public enum ARMOR_TYPE
{
    none = 0,
    head = 1,
    chest = 2,
    legs = 3,
    ring = 4,
}

#endregion

public enum AMBIENT
{
    World = 0,
    Interior = 1,
    Location = 2,
    Combat = 3,
    Development = 4
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
    Audio = 5,
    Back = 6,
    Exit = 7,
    YesPopup = 8
}

public enum SCENE_INDEX
{
    MainMenu = 0,
    Master = 1,
}