using TMPro;
using UnityEngine;

public class ButtonDoubleUI : ButtonUI
{
    [SerializeField, ConditionalHide] private TextMeshProUGUI _optionTxt = null;

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

}