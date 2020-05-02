using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Equipment/Item", order = 0)]
public class ItemSO : EquipmentSO
{
    [Header("Item")]
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public ITEM_TYPE itemType;
    public int level;

}