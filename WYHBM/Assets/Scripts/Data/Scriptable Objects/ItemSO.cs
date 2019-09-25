using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Data/Item", order = 0)]
public class ItemSO : Equipment
{
	[Header ("Item")]
	public ITEM_TYPE itemType;
	public float itemValue;
}