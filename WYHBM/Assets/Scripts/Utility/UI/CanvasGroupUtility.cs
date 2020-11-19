using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class CanvasGroupUtility : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Range(0, 5)] private float fadeDuration = 1;

    private bool _isShowing;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _isShowing = _canvasGroup.alpha == 1 ? true : false;
        
        SetProperties(true);
    }

    public void Show(bool isShowing)
    {
        _isShowing = isShowing;

        if (isShowing)
        {
            _canvasGroup
                .DOFade(1, fadeDuration);
            SetCanvas(true);
            
            // _canvasGroup
            //     .DOFade(1, fadeDuration)
            //     .OnComplete(() => SetProperties(true));
            // SetCanvas(true);
        }
        else
        {
            _canvasGroup
                .DOFade(0, fadeDuration)
                .OnComplete(() => SetCanvas(false));

            // SetProperties(false);
        }
    }

    public void ShowInstant(bool isShowing)
    {
        _isShowing = isShowing;

        SetCanvas(isShowing);
        // SetProperties(isShowing);
    }

    private void SetCanvas(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
    }

    private void SetProperties(bool isEnabled)
    {
        _canvasGroup.interactable = isEnabled;
        _canvasGroup.blocksRaycasts = isEnabled;
    }

}