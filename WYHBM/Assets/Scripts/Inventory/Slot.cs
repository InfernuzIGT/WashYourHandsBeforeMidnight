using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    ItemSO item;
    public Image icon;
    public Button removeButton;

    public void AddItem(ItemSO newItem)
    {
        icon.sprite = newItem.icon;

        item = newItem;

    }

    public void OnRemoveButton()
    {
        InteractionItem Item = Instantiate(GameData.Instance.gameConfig.itemPrefab, GameManager.Instance.GetPlayerFootPosition(), Quaternion.identity);

        Debug.Log ($"<b> Vector3  </b>" + GameManager.Instance.player.gameObject.transform.position.x);
        Item.AddInfo(item);

        GameManager.Instance.worldUI.inventorySlots.RemoveItemList(item);
        Destroy(gameObject);

    }

}