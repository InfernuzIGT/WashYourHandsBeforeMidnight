using UnityEngine;

[CreateAssetMenu(fileName = "New armor", menuName = "Data/Armor", order = 0)]
public class ArmorSO : Equipment
{
    [Header("Armor")]
    public WEAPON_TYPE weaponType;
    public float weaponValue;
}