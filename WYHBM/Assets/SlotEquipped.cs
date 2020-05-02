using UnityEngine;
using UnityEngine.UI;

public class SlotEquipped : MonoBehaviour
{
    public ItemSO item;
    public Image icon;
    public Slot slot;

    public void EquipItem(ItemSO itemEquipped)
    {
        if (itemEquipped.isEquippable)
        {
            item = itemEquipped;
            icon.sprite = itemEquipped.sprite;
        }
        else
            return;
    }

    public void UnequipItem(ItemSO itemUnequipped)
    {
        item = itemUnequipped;
        slot.AddItem(itemUnequipped);
    }
}