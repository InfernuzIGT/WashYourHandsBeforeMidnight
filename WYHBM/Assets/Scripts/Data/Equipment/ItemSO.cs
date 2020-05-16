using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Equipment/Item", order = 0)]
public class ItemSO : EquipmentSO
{
    [Header("Item")]
    public bool isEquippable = false;
    public ITEM_TYPE itemType;
    public int level;
}