using DG.Tweening;
using Events;
using UnityEngine;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
public class Fade : MonoBehaviour
{
    private TweenCallback _callbackMid;
    private TweenCallback _callbackEnd;
    private bool _fadeFast;

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        EventController.AddListener<FadeEvent>(OnFade);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<FadeEvent>(OnFade);
    }

    private void OnFade(FadeEvent evt)
    {
        _fadeFast = evt.fadeFast;
        _callbackMid = evt.callbackMid;
        _callbackEnd = evt.callbackEnd;

        evt.callbackStart?.Invoke();

        _canvasGroup
            .DOFade(1, _fadeFast ? GameData.Instance.worldConfig.fadeFastDuration : GameData.Instance.worldConfig.fadeSlowDuration)
            .OnComplete(() => SetProperties(true))
            .OnKill(FadeIn);

        SetCanvas(true);
    }

    private void FadeIn()
    {
        _callbackMid?.Invoke();

        _canvasGroup
            .DOFade(0, _fadeFast ? GameData.Instance.worldConfig.fadeFastDuration : GameData.Instance.worldConfig.fadeSlowDuration)
            .OnComplete(() => SetCanvas(false))
            .OnKill(FadeOut);

        SetProperties(false);
    }

    private void FadeOut()
    {
        _callbackEnd?.Invoke();
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