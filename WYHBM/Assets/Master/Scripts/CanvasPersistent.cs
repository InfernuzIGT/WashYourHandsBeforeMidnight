using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CanvasPersistent : MonoSingleton<CanvasPersistent>
{
    [Header("General")]
    [SerializeField] private WorldConfig _worldConfig = null;

    [Header("Fade")]
    [SerializeField] private Image _fadeImg = null;
    [SerializeField] private Image _letterboxTopImg = null;
    [SerializeField] private Image _letterboxBotImg = null;

    [Header("Save")]
    [SerializeField] private Animator _animatorSave = null;

    // Fade
    private TweenCallback _callbackMid;
    private TweenCallback _callbackEnd;
    private bool _fadeFast;
    private bool _show;
    private float _letterboxSize;

    // Save
    protected readonly int hash_IsSaving = Animator.StringToHash("isSaving");

    private Canvas _canvas;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        _letterboxSize = _letterboxTopImg.rectTransform.sizeDelta.y;
    }

    private void OnEnable()
    {
        EventController.AddListener<FadeEvent>(OnFade);
        EventController.AddListener<SaveAnimationEvent>(OnSaveAnimation);
        EventController.AddListener<CutsceneEvent>(OnCutscene);
        EventController.AddListener<CustomFadeEvent>(OnCustomFade);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<FadeEvent>(OnFade);
        EventController.RemoveListener<SaveAnimationEvent>(OnSaveAnimation);
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
        EventController.RemoveListener<CustomFadeEvent>(OnCustomFade);
    }

    private void OnFade(FadeEvent evt)
    {
        _fadeFast = evt.fast;
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
            .OnKill(() => _callbackEnd?.Invoke());
    }

    private void OnCustomFade(CustomFadeEvent evt)
    {
        if (evt.fadeIn)
        {
            _fadeImg
                .DOFade(1, evt.instant ? 0 : 1)
                .OnComplete(() => evt.callbackFadeIn?.Invoke());

            SetCanvas(true);
        }
        else
        {
            _fadeImg
                .DOFade(0, evt.instant ? 0 : 1)
                .OnComplete(() => SetCanvas(false));
        }
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

    private void OnSaveAnimation(SaveAnimationEvent evt)
    {
        ShowSaveAnimation();
    }
    
    public void ShowSaveAnimation()
    {
        _animatorSave.SetTrigger(hash_IsSaving);
    }

}