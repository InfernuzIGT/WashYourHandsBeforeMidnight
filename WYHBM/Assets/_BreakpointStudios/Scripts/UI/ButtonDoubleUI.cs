using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class ButtonDoubleUI : ButtonUI, IMoveHandler
{
    [SerializeField, ConditionalHide] private TextMeshProUGUI _optionTxt = null;
    [SerializeField, ConditionalHide] private Image _arrowLeftImg = null;
    [SerializeField, ConditionalHide] private Image _arrowRightImg = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringEvent = null;

    EventInstance changeSound;

    [Header("Button Double")]
    [SerializeField] private bool _dynamicArrows = true;

    private bool _isSelected;
    private bool _arrowLeftState;
    private bool _arrowRightState;
    private UnityAction _actionLeft;
    private UnityAction _actionRight;

   

    protected override void StartExtra()
    {
        changeSound = FMODUnity.RuntimeManager.CreateInstance(_FMODConfig.back);
    }

    public void AddListenerHorizontal(UnityAction actionLeft, UnityAction actionRight)
    {
        _actionLeft = actionLeft;
        _actionRight = actionRight;
    }

    public void UpdateUI(string text)
    {
        _optionTxt.text = text;
    }

    public void UpdateUI(string text, bool isLeft)
    {
        _optionTxt.text = text;

        if (!_dynamicArrows)return;

        _arrowLeftState = !isLeft;
        _arrowRightState = isLeft;

        SetArrowState();
    }

    public void UpdateUI(LocalizedString text, bool isLeft)
    {
        _localizeStringEvent.StringReference = text;
        _localizeStringEvent.OnUpdateString.Invoke(_optionTxt.text);

        if (!_dynamicArrows)return;

        _arrowLeftState = !isLeft;
        _arrowRightState = isLeft;

        SetArrowState();
    }

    public void UpdateUI(string text, int index, int indexMax)
    {
        _optionTxt.text = text;

        if (!_dynamicArrows)return;

        _arrowLeftState = index != 0;
        _arrowRightState = index != indexMax;

        SetArrowState();
    }

    public void UpdateUI(LocalizedString text, int index, int indexMax)
    {
        _localizeStringEvent.StringReference = text;
        _localizeStringEvent.OnUpdateString.Invoke(_optionTxt.text);

        if (!_dynamicArrows)return;

        _arrowLeftState = index != 0;
        _arrowRightState = index != indexMax;

        SetArrowState();
    }

    protected override void OnSelectExtra()
    {
        PlaySound();
        _isSelected = true;
        _optionTxt.color = Color.black;

        if (_dynamicArrows)
        {
            SetArrowState();
        }
        else
        {
            _arrowLeftImg.enabled = true;
            _arrowRightImg.enabled = true;
        }
    }

    protected override void OnDeselectExtra()
    {
        _isSelected = false;
        _optionTxt.color = Color.white;

        _arrowLeftImg.enabled = false;
        _arrowRightImg.enabled = false;
    }

    private void SetArrowState()
    {
        if (!_isSelected)return;

        _arrowLeftImg.enabled = _arrowLeftState;
        _arrowRightImg.enabled = _arrowRightState;
    }

    public void OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Left:
                _actionLeft.Invoke();
                changeSound.start();
                break;

            case MoveDirection.Right:
                _actionRight.Invoke();
                changeSound.start();
                break;
        }
    }

}