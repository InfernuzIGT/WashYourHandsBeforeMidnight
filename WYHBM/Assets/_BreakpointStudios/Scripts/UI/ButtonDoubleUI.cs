using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonDoubleUI : ButtonUI, IMoveHandler
{
    [SerializeField, ConditionalHide] private TextMeshProUGUI _optionTxt = null;

    private UnityAction _actionLeft;
    private UnityAction _actionRight;

    public void AddListenerHorizontal(UnityAction actionLeft, UnityAction actionRight)
    {
        _actionLeft = actionLeft;
        _actionRight = actionRight;
    }

    public void SetText(string text)
    {
        _optionTxt.text = text;
    }

    protected override void OnSelectExtra()
    {
        _optionTxt.color = Color.black;
    }

    protected override void OnDeselectExtra()
    {
        _optionTxt.color = Color.white;
    }

    public void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Left:
                _actionLeft.Invoke();
                break;

            case MoveDirection.Right:
                _actionRight.Invoke();
                break;
        }
    }

}