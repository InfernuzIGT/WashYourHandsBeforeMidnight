using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("General")]
    [SerializeField] private int _index = 0;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private CombatConfig _combatConfig = null;
    [SerializeField, ConditionalHide] private Image _selectionImg = null;
    [SerializeField, ConditionalHide] private Image _itemImg = null;
    [SerializeField, ConditionalHide] private Image _inputImg = null;

    private ItemSO _item;
    private Button _actionButton;
    private UnityAction<int> _actionIndex;

    // Events
    private EventSystemEvent _eventSystemEvent;
    private CombatActionEvent _combatActionEvent;

    public void Init(ItemSO item, UnityAction<int> actionInt)
    {
        _item = item;
        _itemImg.sprite = item.icon;

        _eventSystemEvent = new EventSystemEvent();
        _eventSystemEvent.objectSelected = gameObject;

        _combatActionEvent = new CombatActionEvent();
        _combatActionEvent.item = item;

        _actionButton = GetComponent<Button>();
        _actionButton.onClick.AddListener(() => DoAction());
        _actionIndex = actionInt;

        _selectionImg.enabled = false;
        _inputImg.enabled = false;
    }

    private void DoAction()
    {
        EventController.TriggerEvent(_combatActionEvent);
        _actionIndex.Invoke(_index);
    }

    private void SetIndex(int index)
    {

    }

    public void SelectButton()
    {
        EventController.TriggerEvent(_eventSystemEvent);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _selectionImg.enabled = true;
        _inputImg.enabled = true;

        transform.DOScale(1.2f, _combatConfig.animationDuration);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _selectionImg.enabled = false;
        _inputImg.enabled = false;

        transform.DOScale(0.8f, _combatConfig.animationDuration);
    }

}