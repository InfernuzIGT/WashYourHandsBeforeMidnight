﻿using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Equipment/Item", order = 0)]
public class ItemSO : EquipmentSO
{
	[Header("Item")]
	public ITEM_TYPE itemType;
}