using UnityEngine;

[CreateAssetMenu(fileName = "New armor", menuName = "Equipment/Armor", order = 0)]
public class ArmorSO : EquipmentSO
{
    [Header("Armor")]
    public ARMOR_TYPE armorType;
}