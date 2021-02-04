using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private CombatConfig _combatConfig = null;
    [SerializeField, ConditionalHide] private Image _itemImg = null;

    private ItemSO _item;
    private Button _actionButton;

    // Events
    private EventSystemEvent _eventSystemEvent;
    private CombatActionEvent _combatActionEvent;

    public void Init(ItemSO item)
    {
        _item = item;
        _itemImg.sprite = item.icon;

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

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOScale(1.25f, _combatConfig.animationDuration);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.DOScale(1f, _combatConfig.animationDuration);
    }

}