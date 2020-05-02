using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public SlotEquipped slotEquipped;
    public ItemSO item;
    public Image icon;
    public Button removeButton;

    public void AddItem(ItemSO newItem)
    {
        item = newItem;
        icon.sprite = newItem.sprite;
    }

    public void OnClickItem(SlotEquipped slotFree)
    {
        slotEquipped = slotFree;
        
        slotFree.EquipItem(item);
    }

    public void OnRemoveButton()
    {
        InteractionItem newItem = Instantiate(GameData.Instance.gameConfig.itemPrefab, GameManager.Instance.GetPlayerFootPosition(), Quaternion.identity);

        newItem.AddInfo(item);

        GameManager.Instance.worldUI.inventorySlots.RemoveItemList(item);

        Destroy(gameObject);
    }

}