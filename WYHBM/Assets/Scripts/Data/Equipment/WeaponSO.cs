using UnityEngine;

[CreateAssetMenu(fileName = "New weapon", menuName = "Equipment/Weapon", order = 0)]
public class WeaponSO : EquipmentSO
{
	[Header("Weapon")]
	public WEAPON_TYPE weaponType;
	public float weaponDamage;
	[Range(0, 100)]
	public int weaponCriticalChance;

	[Header("Action")]
	public ActionSO actionWeapon;

	private void OnEnable()
	{
		if (actionWeapon != null)
			actionWeapon.value = weaponDamage;
	}
}