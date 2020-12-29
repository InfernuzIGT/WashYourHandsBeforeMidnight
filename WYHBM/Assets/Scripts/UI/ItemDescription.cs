using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    public Image itemImg;
    public TextMeshProUGUI descriptionTxt;
    public TextMeshProUGUI valueTxt;

    public void Show(ItemSO item)
    {
        itemImg.sprite = item.icon;
        descriptionTxt.text = item.description;

        switch (item.type)
        {
            case ITEM_TYPE.WeaponMelee:
            case ITEM_TYPE.WeaponOneHand:
            case ITEM_TYPE.WeaponTwoHands:
            case ITEM_TYPE.ItemGrenade:
                // valueTxt.text = string.Format(GameData.Instance.textConfig.itemDamage, item.value.x, item.value.y);
                break;

            case ITEM_TYPE.ItemHeal:
                // valueTxt.text = string.Format(GameData.Instance.textConfig.itemHeal, item.value.x, item.value.y);
                break;

            case ITEM_TYPE.ItemDefense:
                // valueTxt.text = string.Format(GameData.Instance.textConfig.itemDefense, item.value.x);
                break;

            default:
                valueTxt.text = "";
                break;
        }
    }

    public void Hide()
    {
        // itemImg.sprite = GameData.Instance.worldConfig.spriteEmptyUI;;
        descriptionTxt.text = "";
        valueTxt.text = "";
    }

}