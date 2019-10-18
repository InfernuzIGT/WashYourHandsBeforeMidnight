using System.Collections.Generic;
using UnityEngine;

public class CharacterSO : ScriptableObject
{
    [Header("General")]
    public new string name;
    public CHARACTER_TYPE characterType = CHARACTER_TYPE.none;
    public CHARACTER_BEHAVIOUR characterBehaviour = CHARACTER_BEHAVIOUR.none;

    [Header("Stats")]
    public int healthMax;
    public int damage;
    public int defense;

    [Header("Equipment")]
    public List<WeaponSO> equipmentWeapon;
    [Space]
    public List<ItemSO> equipmentItem;
    [Space]
    public List<ArmorSO> equipmentArmor;
    
    [Header ("Sprites")]
    public Sprite spriteCharacter;
    
}