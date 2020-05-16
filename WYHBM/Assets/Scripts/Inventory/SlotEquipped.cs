using UnityEngine;
using UnityEngine.UI;

public class SlotEquipped : MonoBehaviour
{
    public ItemSO item;
    public Image icon;
    public Slot slot;

    public void EquipItem(ItemSO itemEquipped)
    {
        item = itemEquipped;

        GameManager.Instance.worldUI.inventorySlots.AddItemEquippedList(item);

        icon.sprite = itemEquipped.previewSprite;

    }

    public void UnequipItem()
    {
        if (GameManager.Instance.worldUI.inventorySlots.isFull)
        {
            Debug.Log($"<b> INVENTORY FULL! </b>");
            return;
        }

        Slot newSlot = Instantiate(GameData.Instance.gameConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
        newSlot.AddItem(item);

        GameManager.Instance.worldUI.inventorySlots.RemoveItemEquippedList(item);

        Destroy(gameObject);

    }
}