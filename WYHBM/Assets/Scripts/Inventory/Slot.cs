using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemSO item;
    public Image icon;
    public Button removeButton;

    public void AddItem(ItemSO newItem)
    {
        item = newItem;
        icon.sprite = newItem.previewSprite;
    }

    public void OnClickItem()
    {
        if (GameManager.Instance.worldUI.inventorySlots.isFullEquipment)
        {
            Debug.Log($"<b> EQUIPMENT FULL! </b>");
            return;
            
        }

        SlotEquipped newSlotEquipped = Instantiate(GameData.Instance.gameConfig.slotEquippedPrefab, GameManager.Instance.worldUI.itemEquippedParents);

        newSlotEquipped.EquipItem(item);

        GameManager.Instance.worldUI.inventorySlots.AddItemEquippedList(item);

        GameManager.Instance.worldUI.inventorySlots.RemoveItemList(item);

        Destroy(gameObject);
    }

    public void OnInspectButton()
    {
        GameManager.Instance.worldUI.SetItemInformation(item);
    }

    public void OnRemoveButton()
    {
        InteractionItem newItem = Instantiate(GameData.Instance.gameConfig.itemPrefab, GameManager.Instance.GetPlayerFootPosition(), Quaternion.identity);

        newItem.AddInfo(item);

        GameManager.Instance.worldUI.inventorySlots.RemoveItemList(item);

        GameManager.Instance.worldUI.inventorySlots.isFull = false;
        GameManager.Instance.worldUI.TakeOffItemInformation();

        Destroy(gameObject);
    }

}