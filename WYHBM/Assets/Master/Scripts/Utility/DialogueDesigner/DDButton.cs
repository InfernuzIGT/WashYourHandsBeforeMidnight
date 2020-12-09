using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(CanvasGroupUtility))]
public class DDButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("References")]
    [SerializeField] private Button _button = null;
    [SerializeField] private CanvasGroupUtility _canvasUtility = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _text = null;
    [SerializeField] private Image _selectionImg = null;
    [SerializeField] private Image _inputImg = null;

    private void Start()
    {
        _inputImg.enabled = false;
        _selectionImg.enabled = false;
        _canvasUtility.ShowInstant(false);
    }

    public void AddListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

    public void Show(string text)
    {
        // gameObject.SetActive(true);

        _text.text = text;

        _canvasUtility.Show(true);

        _selectionImg.enabled = false;
    }

    public void Hide()
    {
        _canvasUtility.ShowInstant(false);
        // gameObject.SetActive(false);
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }

    public void Select()
    {
        _button.Select();
    }

    public void SetInput()
    {
        _inputImg.enabled = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _selectionImg.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _selectionImg.enabled = false;
    }
}