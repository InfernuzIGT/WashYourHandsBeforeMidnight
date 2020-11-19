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
    [SerializeField] private GameObject _selection = null;
    [SerializeField] private Image _inputImg = null;

    private void Start()
    {
        _inputImg.enabled = false;
        _selection.SetActive(false);
        _canvasUtility.ShowInstant(false);
    }

    public void AddListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

    public void Show(string text)
    {
        _text.text = text;

        _canvasUtility.Show(true);
    }

    public void Hide()
    {
        _selection.SetActive(false);
        _inputImg.enabled = false;
        _canvasUtility.ShowInstant(false);
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
        _selection.SetActive(true);
    }

	public void OnDeselect(BaseEventData eventData)
	{
        _selection.SetActive(false);
	}
}