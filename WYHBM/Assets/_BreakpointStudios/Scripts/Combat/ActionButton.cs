using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour
{
    public Image itemImg;

    private ItemSO _item;
    private Button _actionButton;

    // Events
    private EventSystemEvent _eventSystemEvent;
    private CombatActionEvent _combatActionEvent;

    private void Start()
    {

    }

    public void Init(ItemSO item)
    {
        _item = item;
        itemImg.sprite = item.icon;

        _eventSystemEvent = new EventSystemEvent();
        _eventSystemEvent.objectSelected = gameObject;

        _combatActionEvent = new CombatActionEvent();
        _combatActionEvent.item = item;
        
        _actionButton = GetComponent<Button>();
        _actionButton.onClick.AddListener(() => DoAction());
    }

    private void DoAction()
    {
        EventController.TriggerEvent(_combatActionEvent);
    }

    public void SelectFirstButton()
    {
        EventController.TriggerEvent(_eventSystemEvent);
    }

}