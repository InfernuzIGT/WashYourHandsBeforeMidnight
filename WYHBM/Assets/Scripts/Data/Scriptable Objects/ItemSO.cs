using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Data/Item", order = 0)]
public class ItemSO : Equipment
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