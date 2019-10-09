using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Equipment/Weapon", order = 0)]
public class WeaponSO : EquipmentSO
{
	[Header("Weapon")]
	public WEAPON_TYPE weaponType;
	[Range(0, 100)]
	public int weaponCriticalChance;
}