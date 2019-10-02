using System.Collections.Generic;
using UnityEngine;

public class CharacterSO : ScriptableObject
{
    [Header("General")]
    public new string name;
    public CHARACTER_TYPE characterType = CHARACTER_TYPE.none;
    public CHARACTER_BEHAVIOUR characterBehaviour = CHARACTER_BEHAVIOUR.none;

    [Header("Stats")]
    public float healthMax;
    public float damage;
    public float defense;

    [Header("Equipment")]
    public List<WeaponSO> equipmentWeapon;
    [Space]
    public List<ItemSO> equipmentItem;
    [Space]
    public List<ArmorSO> equipmentArmor;
    
    // TODO Mariano: Add sprite
}