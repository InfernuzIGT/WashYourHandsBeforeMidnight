using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image iconImg;
    public TextMeshProUGUI buttonTxt;
    [Space]
    public TextMeshProUGUI nameTxt;
    public Button slotBtn;
    // SpriteState sprState = new SpriteState();
    private bool _isEquipped;
    private bool _alreadyEquipped;
    private ItemSO _item;
    public ItemSO Item { get { return _item; } }

    public void SlotButton()
    {

        // if (_item.isDroppeable)
        // {
        //     slotBtn.interactable = false;
        //     // slotBtn.spriteState = slotBtn.sprState.highlightedSprite;
        //     return;
        // }

        // else 
        DropItem();
    }

    public void AddItem(ItemSO newItem)
    {
        _item = newItem;

        // GameManager.Instance.AddItem(_item);

        iconImg.sprite = newItem.icon;
        buttonTxt.text = GameData.Instance.textConfig.itemDrop;
        nameTxt.text = newItem.title;
    }

    public void DropItem()
    {
        // GameManager.Instance.DropItem(this);

        // GameManager.Instance.worldUI.ShowPopup(GameData.Instance.textConfig.popupDestroyItem);

        Destroy(gameObject);
    }

    public void EquipItem()
    {
        // if (!_isEquipped)
        // {
        //     if (GameManager.Instance.IsEquipmentFull) return;

        //     GameManager.Instance.EquipItem(_item);
        //     SetEquip(true);
        // }
        // else
        // {
        //     if (GameManager.Instance.IsInventoryFull) return;

        //     GameManager.Instance.UnequipItem(_item);
        //     SetEquip(false);
        // }
    }

    private void SetEquip(bool isEquipped)
    {
        _isEquipped = isEquipped;
        transform.SetParent(isEquipped ? GameManager.Instance.worldUI.GetSlot() : GameManager.Instance.worldUI.itemParents);
    }

    public void PointerEnter()
    {
        slotBtn.gameObject.SetActive(true);

        if (!_isEquipped)
        {
            GameManager.Instance.worldUI.itemDescription.Show(_item);
        }
    }

    public void PointerExit()
    {
        slotBtn.gameObject.SetActive(false);

        if (!_isEquipped)
        {
            GameManager.Instance.worldUI.itemDescription.Hide();
        }
    }
}