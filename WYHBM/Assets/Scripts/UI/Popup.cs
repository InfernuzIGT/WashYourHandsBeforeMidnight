using DG.Tweening;
using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI titleTxt;

    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Tween _tweenStart;
    private Tween _tweenEnd;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }
    private void OnEnable()
    {
        Play();
    }

    private void Play()
    {
        _rectTransform.anchoredPosition = _originalPosition;

        _tweenStart = transform
            .DOLocalMoveY(-145, .75f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0);

        _tweenEnd = transform
            .DOLocalMoveY(145, .75f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0)
            .SetDelay(5)
            .OnComplete(Complete);
    }

    public void SetTitle(string title)
    {
        titleTxt.text = title;
    }

    public void Complete()
    {
        gameObject.SetActive(false);

        _tweenStart.Kill();
        _tweenEnd.Kill();
    }

}