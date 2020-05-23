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
        itemImg.sprite = item.previewSprite;
        descriptionTxt.text = item.description;

        switch (item.type)
        {
            case ITEM_TYPE.None:
                valueTxt.text = "";
                break;

            case ITEM_TYPE.Heal:
                valueTxt.text = string.Format(GameData.Instance.textConfig.itemHeal, item.valueMin, item.valueMax);
                break;

            case ITEM_TYPE.Damage:
            case ITEM_TYPE.Weapon:
                valueTxt.text = string.Format(GameData.Instance.textConfig.itemDamage, item.valueMin, item.valueMax);
                break;

            case ITEM_TYPE.Defense:
                valueTxt.text = string.Format(GameData.Instance.textConfig.itemDefense, item.valueMin);
                break;

            default:
                break;
        }
    }

    public void Hide()
    {
        itemImg.sprite = GameData.Instance.gameConfig.spriteEmptyUI;;
        descriptionTxt.text = "";
        valueTxt.text = "";
    }

}