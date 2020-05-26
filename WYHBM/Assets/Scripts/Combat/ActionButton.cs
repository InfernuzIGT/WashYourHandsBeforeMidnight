using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour
{
    public Image itemImg;
    
    private ItemSO _item;

    private void Start()
    {
        Button actionButton = GetComponent<Button>();
        actionButton.onClick.AddListener(() => DoAction());
    }
    
    public void Init(ItemSO item)
    {
        _item = item;
        itemImg.sprite = item.previewSprite;
    }

    private void DoAction()
    {
        GameManager.Instance.combatManager.DoAction(_item);
    }

}