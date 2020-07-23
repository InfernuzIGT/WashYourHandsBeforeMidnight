using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour
{
    public Image itemImg;

    private ItemSO _item;
    private Button _actionButton;

    private void Start()
    {
        _actionButton = GetComponent<Button>();
        _actionButton.onClick.AddListener(() => DoAction());
    }

    public void Init(ItemSO item)
    {
        _item = item;
        itemImg.sprite = item.previewSprite;
    }

    private void DoAction()
    {
        GameManager.Instance.combatManager.SelectAction(_item);
    }

    public void SelectFirstButton()
    {
        GameManager.Instance.SelectButton(gameObject);
    }

    // public void SetButtonEnable(bool isEnable)
    // {
    //     _actionButton.enabled = isEnable;
    // }

}