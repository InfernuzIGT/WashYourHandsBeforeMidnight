using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image iconImg;
    public TextMeshProUGUI buttonTxt;

    private ItemSO _item;
    private bool _isEquipped;

    public void SlotButton()
    {
        if (_isEquipped)
        {
            UnequipItem();
        }
        else
        {
            DropItem();
        }
    }

    public void AddItem(ItemSO newItem)
    {
        _item = newItem;

        GameManager.Instance.AddItem(_item);

        iconImg.sprite = newItem.previewSprite;
        buttonTxt.text = GameData.Instance.textConfig.itemDrop;
    }

    public void DropItem()
    {
        InteractionItem newItem = Instantiate(GameData.Instance.gameConfig.itemPrefab, GameManager.Instance.GetPlayerFootPosition(), Quaternion.identity);
        newItem.AddInfo(_item);
        newItem.DetectSize();

        GameManager.Instance.DropItem(_item);

        Destroy(gameObject);
    }

    // TODO Mariano: REVIEW
    public void EquipItem()
    {
        if ( /* GameManager.Instance.IsEquipmentFull || */ _isEquipped)
        {
            return;
        }

        switch (_item.type)
        {
            case ITEM_TYPE.Damage:
                if (GameManager.Instance.combatCharacters[0].weapon == null)
                {
                    GameManager.Instance.EquipItem(_item);
                    SetEquip(true);
                };
                break;

            case ITEM_TYPE.Heal: // TODO Mariano: Change to Generic Item
                if (GameManager.Instance.combatCharacters[0].item == null)
                {
                    GameManager.Instance.EquipItem(_item);
                    SetEquip(true);
                };
                break;

            case ITEM_TYPE.Defense:
                if (GameManager.Instance.combatCharacters[0].defense == null)
                {
                    GameManager.Instance.EquipItem(_item);
                    SetEquip(true);
                };
                break;

            default:
                break;
        }
    }

    public void UnequipItem()
    {
        if (GameManager.Instance.IsInventoryFull)
        {
            return;
        }

        GameManager.Instance.UnequipItem(_item);
        SetEquip(false);
    }

    private void SetEquip(bool isEquipped)
    {
        _isEquipped = isEquipped;
        transform.SetParent(isEquipped ? GameManager.Instance.worldUI.itemEquippedParents : GameManager.Instance.worldUI.itemParents);
        buttonTxt.text = isEquipped ? GameData.Instance.textConfig.itemUnequip : GameData.Instance.textConfig.itemDrop;
    }

    public void PointerEnter()
    {
        if (!_isEquipped)
        {
            GameManager.Instance.worldUI.itemDescription.Show(_item);
        }
    }

    public void PointerExit()
    {
        if (!_isEquipped)
        {
            GameManager.Instance.worldUI.itemDescription.Hide();
        }
    }
}