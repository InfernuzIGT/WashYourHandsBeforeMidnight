using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class CanvasGroupUtility : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Range(0, 5)] private float fadeDuration = 1;
    [Space]
    [SerializeField] private UnityEvent OnShow = null;
    [SerializeField] private UnityEvent OnHide = null;

    private bool _isShowing;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private LayoutElement _layoutElement;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _layoutElement = GetComponent<LayoutElement>();

        _isShowing = _canvasGroup.alpha == 1 ? true : false;

        SetProperties(true);
    }

    public void Show(bool isShowing, float delay = 0, float duration = 0)
    {
        _isShowing = isShowing;

        if (isShowing)
        {
            _canvasGroup
                .DOFade(1, duration == 0 ? fadeDuration : duration)
                .SetDelay(delay)
                .OnComplete(() => CallEvent(true));

            SetCanvas(true);
        }
        else
        {
            _canvasGroup
                .DOFade(0, duration == 0 ? fadeDuration : duration)
                .SetDelay(delay)
                .OnComplete(() => SetCanvas(false));
        }
    }

    public void ShowInstant(bool isShowing)
    {
        _isShowing = isShowing;

        if (_isShowing)
        {
            _canvasGroup.alpha = 1;
            CallEvent(true);
        }
        else
        {
            _canvasGroup.alpha = 0;
        }

        SetCanvas(isShowing);
    }

    private void SetCanvas(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
        _canvasGroup.interactable = isEnabled;

        if (_layoutElement != null)_layoutElement.ignoreLayout = !isEnabled;

        if (!isEnabled)CallEvent(false);
    }

    private void SetProperties(bool isEnabled)
    {
        _canvasGroup.interactable = isEnabled;
        _canvasGroup.blocksRaycasts = isEnabled;
    }

    private void CallEvent(bool isStarted)
    {
        if (isStarted)
        {
            OnShow.Invoke();
        }
        else
        {
            OnHide.Invoke();
        }
    }

    public void SetCanvasCamera()
    {
        _canvas.worldCamera = Camera.main;
    }

}