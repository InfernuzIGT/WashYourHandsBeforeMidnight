using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Data/Weapon", order = 0)]
public class WeaponSO : Equipment
{
	[Header("Weapon")]
	public WEAPON_TYPE weaponType;
	public float weaponValue;
	[Range (0, 100)]
	public int weaponCriticalChance;
}