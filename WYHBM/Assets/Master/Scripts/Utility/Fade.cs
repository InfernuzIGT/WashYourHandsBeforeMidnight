using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Fade : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private WorldConfig _worldConfig = null;
    [Space]
    [SerializeField] private Image _fadeImg = null;
    [SerializeField] private Image _letterboxTopImg = null;
    [SerializeField] private Image _letterboxBotImg = null;

    private TweenCallback _callbackMid;
    private TweenCallback _callbackEnd;
    private bool _fadeFast;
    private bool _show;
    private float _letterboxSize;

    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _letterboxSize = _letterboxTopImg.rectTransform.sizeDelta.y;
    }

    private void OnEnable()
    {
        EventController.AddListener<FadeEvent>(OnFade);
        EventController.AddListener<CutsceneEvent>(OnCutscene);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<FadeEvent>(OnFade);
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
    }

    private void OnFade(FadeEvent evt)
    {
        _fadeFast = evt.fadeFast;
        _callbackMid = evt.callbackMid;
        _callbackEnd = evt.callbackEnd;

        evt.callbackStart?.Invoke();

        _fadeImg
            .DOFade(1, _fadeFast ? _worldConfig.fadeFastDuration : _worldConfig.fadeSlowDuration)
            .OnKill(FadeIn);

        SetCanvas(true);
    }

    private void FadeIn()
    {
        _callbackMid?.Invoke();

        _fadeImg
            .DOFade(0, _fadeFast ? _worldConfig.fadeFastDuration : _worldConfig.fadeSlowDuration)
            .OnComplete(() => SetCanvas(false))
            .OnKill(FadeOut);
    }

    private void FadeOut()
    {
        _callbackEnd?.Invoke();
    }

    private void SetCanvas(bool isEnabled)
    {
        _canvas.enabled = isEnabled;
    }

    private void OnCutscene(CutsceneEvent evt)
    {
        _show = evt.show;

        _letterboxTopImg.rectTransform
            .DOLocalMoveY(evt.show ? -_letterboxSize : _letterboxSize, 1)
            .SetRelative();

        _letterboxBotImg.rectTransform
            .DOLocalMoveY(evt.show ? _letterboxSize : -_letterboxSize, 1)
            .SetRelative()
            .OnKill(CheckLetterbox);

        SetCanvas(true);
    }

    private void CheckLetterbox()
    {
        if (_show)return;

        SetCanvas(false);
    }

}