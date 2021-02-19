using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPersistent : MonoSingleton<CanvasPersistent>
{
    [Header("General")]
    [SerializeField] private WorldConfig _worldConfig = null;
    [SerializeField] private CanvasGroupUtility _canvasUtility = null;

    [Header("Fade")]
    [SerializeField] private Image _fadeImg = null;
    [SerializeField] private Image _letterboxTopImg = null;
    [SerializeField] private Image _letterboxBotImg = null;

    [Header("Save")]
    [SerializeField] private Animator _animatorSave = null;

    // Fade
    private TweenCallback _callbackMid;
    private TweenCallback _callbackEnd;
    private bool _fadeInstant;
    private bool _show;
    private float _letterboxSize;
    private float _delay;

    // Save
    protected readonly int hash_IsSaving = Animator.StringToHash("isSaving");
    protected readonly int hash_IsLoading = Animator.StringToHash("isLoading");

    private void Start()
    {
        _letterboxSize = _letterboxTopImg.rectTransform.sizeDelta.y;
    }

    private void OnEnable()
    {
        EventController.AddListener<FadeEvent>(OnFade);
        EventController.AddListener<SaveAnimationEvent>(OnSaveAnimation);
        EventController.AddListener<LoadAnimationEvent>(OnLoadAnimation);
        EventController.AddListener<CutsceneEvent>(OnCutscene);
        EventController.AddListener<CustomFadeEvent>(OnCustomFade);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<FadeEvent>(OnFade);
        EventController.RemoveListener<SaveAnimationEvent>(OnSaveAnimation);
        EventController.RemoveListener<LoadAnimationEvent>(OnLoadAnimation);
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
        EventController.RemoveListener<CustomFadeEvent>(OnCustomFade);
    }

    private void OnFade(FadeEvent evt)
    {
        _fadeInstant = evt.instant;
        _delay = evt.delay;
        _callbackMid = evt.callbackMid;
        _callbackEnd = evt.callbackEnd;

        evt.callbackStart?.Invoke();

        _fadeImg
            .DOFade(1, _fadeInstant ? 0 : _worldConfig.fadeDuration)
            .OnKill(FadeIn);

        SetCanvas(true);
    }

    private void FadeIn()
    {
        _callbackMid?.Invoke();

        _fadeImg
            .DOFade(0, _fadeInstant ? 0 : _worldConfig.fadeDuration)
            .SetDelay(_delay)
            .OnComplete(() => SetCanvas(false))
            .OnKill(() => _callbackEnd?.Invoke());
    }

    private void OnCustomFade(CustomFadeEvent evt)
    {
        if (evt.fadeIn)
        {
            _fadeImg
                .DOFade(1, evt.instant ? 0 : evt.duration)
                .OnComplete(() => evt.callbackFadeIn?.Invoke())
                .OnKill(() => evt.callbackFMODStop?.Invoke());
        }
        else
        {
            _fadeImg
                .DOFade(0, evt.instant ? 0 : evt.duration)
                .OnComplete(() => SetCanvas(false))
                .OnKill(() => evt.callbackFMODPlay?.Invoke());
        }

        SetCanvas(true);
    }

    private void SetCanvas(bool isEnabled)
    {
        _canvasUtility.ShowInstant(isEnabled);
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

    private void OnLoadAnimation(LoadAnimationEvent evt)
    {
        _animatorSave.SetBool(hash_IsLoading, evt.isLoading);
    }

}