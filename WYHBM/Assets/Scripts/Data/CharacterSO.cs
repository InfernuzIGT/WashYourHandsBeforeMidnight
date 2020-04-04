using System.Collections.Generic;
using UnityEngine;

public class CharacterSO : ScriptableObject
{
    [Header("General")]
    public new string name;
    public int level;
    public CHARACTER_TYPE characterType = CHARACTER_TYPE.none;
    public CHARACTER_BEHAVIOUR characterBehaviour = CHARACTER_BEHAVIOUR.none;

    [Header("Stats")]
    public int statsHealthMax;
    public int statsDamageMelee;
    public int statsDamageDistance;
    public int statsStrength;
    public int statsDexterity;
    public int statsDefense;
    public int statsAgility;
    public int statsLuck;
    public int statsReaction;

    [Header("Equipment")]
    public List<WeaponSO> equipmentWeapon;
    [Space]
    public List<ItemSO> equipmentItem;
    [Space]
    public List<ArmorSO> equipmentArmor;

    [Header("Sprites")]
    public Sprite spriteCharacter;

    [Header("Textures")]
    public Texture2D textureIdle;
    public Texture2D textureWalk;
    public Texture2D textureMovement;

}