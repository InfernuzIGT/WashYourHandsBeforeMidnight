using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Equipment/Item", order = 0)]
public class ItemSO : EquipmentSO
{
	[Header("Item")]
	public ITEM_TYPE itemType;
	public float itemPoints;

	[Header("Action")]
	public ActionSO actionItem;

	private void OnEnable()
	{
		if (actionItem != null)
			actionItem.value = itemPoints;
	}
}