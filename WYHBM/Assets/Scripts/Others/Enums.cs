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

#region Actions

public enum ACTION_TYPE
{
    none = 0,
    weapon = 1,
    item = 2,
    defense = 3,
}

public enum OPTION_TYPE
{
    none = 0,
    option1 = 1,
    option2 = 2,
    option3 = 3,
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
    none = 0,
    heal = 1,
    poison = 2,
    grenade = 3,
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