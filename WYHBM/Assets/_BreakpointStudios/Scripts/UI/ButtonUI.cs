using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private Button _buttonBtn = null;
    [SerializeField, ConditionalHide] private Image _buttonImg = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _buttonTxt = null;

    private void Start()
    {
        _buttonImg.enabled = false;
        _buttonTxt.color = Color.white;
    }

    public void AddListener(UnityAction action)
    {
        _buttonBtn.onClick.AddListener(action);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _buttonImg.enabled = true;
        _buttonTxt.color = Color.black;
        OnSelectExtra();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonImg.enabled = false;
        _buttonTxt.color = Color.white;
        OnDeselectExtra();
    }

    protected virtual void OnSelectExtra() { }
    protected virtual void OnDeselectExtra() { }

}