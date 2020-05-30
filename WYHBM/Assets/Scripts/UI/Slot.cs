﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image iconImg;
    public TextMeshProUGUI buttonTxt;
    [Space]
    public TextMeshProUGUI nameTxt;
    public GameObject slotButton;

    private bool _isEquipped;
    private ItemSO _item;
    public ItemSO Item { get { return _item; } }

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
        nameTxt.text = newItem.name;
    }

    public void DropItem()
    {
        InteractionItem newItem = Instantiate(GameData.Instance.worldConfig.itemPrefab, GameManager.Instance.GetPlayerFootPosition(), Quaternion.identity);
        newItem.AddInfo(_item);
        newItem.DetectSize();

        GameManager.Instance.DropItem(this);

        Destroy(gameObject);
    }

    public void EquipItem()
    {
        if (_isEquipped || GameManager.Instance.IsEquipmentFull)return;

        GameManager.Instance.EquipItem(_item);
        SetEquip(true);
    }

    public void UnequipItem()
    {
        if (GameManager.Instance.IsInventoryFull)return;

        GameManager.Instance.UnequipItem(_item);
        SetEquip(false);
    }

    private void SetEquip(bool isEquipped)
    {
        _isEquipped = isEquipped;
        transform.SetParent(isEquipped ? GameManager.Instance.worldUI.GetSlot() : GameManager.Instance.worldUI.itemParents);
        buttonTxt.text = isEquipped ? GameData.Instance.textConfig.itemUnequip : GameData.Instance.textConfig.itemDrop;
    }

    public void PointerEnter()
    {
        nameTxt.gameObject.SetActive(false);
        slotButton.SetActive(true);

        if (!_isEquipped)
        {
            GameManager.Instance.worldUI.itemDescription.Show(_item);
        }
    }

    public void PointerExit()
    {
        nameTxt.gameObject.SetActive(true);
        slotButton.SetActive(false);

        if (!_isEquipped)
        {
            GameManager.Instance.worldUI.itemDescription.Hide();
        }
    }
}