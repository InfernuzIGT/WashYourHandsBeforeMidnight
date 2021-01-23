﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupUtility))]
public class InputHoldUtility : MonoBehaviour, IHoldeable
{
    [Header("Hold System")]
    [SerializeField, Range(0f, 10f)] private float _duration = 3;
    [SerializeField] private Ease _easeAnimation = Ease.Linear;

    [Header("References")]
    [SerializeField] private bool ShowReferences = true;
    [SerializeField, ConditionalHide] private HoldIconSO _iconData = null;
    [SerializeField, ConditionalHide] private Image _iconImg = null;
    [SerializeField, ConditionalHide] private Image _fillImg = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasGroupUtility = null;

    private Tween _fillAnimation;
    private Tween _fillColor;

    // Properties
    private UnityEvent _onStarted = new UnityEvent();
    public UnityEvent OnStarted { get { return _onStarted; } }

    private UnityEvent _onCanceled = new UnityEvent();
    public UnityEvent OnCanceled { get { return _onCanceled; } }

    private UnityEvent _onFinished = new UnityEvent();
    public UnityEvent OnFinished { get { return _onFinished; } }

    public float Duration { get { return _duration; } set { _duration = value; } }

    private void Start()
    {
        _iconData.SetIconStart(ref _iconImg);

        _canvasGroupUtility.SetCanvasCamera();
        _canvasGroupUtility.ShowInstant(false);
    }

    public void SoundDetect(bool enable)
    {
        if (enable)
        {
            _iconData.SetIconStart(ref _iconImg);
            _canvasGroupUtility.ShowInstant(true);

            _fillImg.fillAmount = 0;
            _fillImg.color = _iconData.colorStart;
        }
        else
        {
            _iconData.SetIconCancel(ref _iconImg);

            _canvasGroupUtility.Show(false);

            _fillAnimation.Kill();
            _fillColor.Kill();

            _fillImg.fillAmount = 0;
            _fillImg.color = _iconData.colorStart;
        }
    }

    public void OnStart()
    {
        _iconData.SetIconStart(ref _iconImg);

        _canvasGroupUtility.ShowInstant(true);

        _fillImg.fillAmount = 0;
        _fillImg.color = _iconData.colorStart;

        _onStarted.Invoke();

        _fillAnimation = _fillImg
            .DOFillAmount(1, _duration)
            .SetEase(_easeAnimation)
            .OnComplete(() => OnFinish());

        _fillColor = _fillImg
            .DOColor(_iconData.colorFinish, _duration)
            .SetEase(_easeAnimation);
    }

    public void OnCancel()
    {
        _iconData.SetIconCancel(ref _iconImg);

        _canvasGroupUtility.Show(false);

        _fillAnimation.Kill();
        _fillColor.Kill();

        _fillImg.fillAmount = 0;
        _fillImg.color = _iconData.colorStart;

        _onCanceled.Invoke();
    }

    public void OnFinish()
    {
        _iconData.SetIconFinish(ref _iconImg);

        _canvasGroupUtility.Show(false, 1f);

        _fillAnimation.Kill();
        _fillColor.Kill();

        _fillImg.fillAmount = 1;
        _fillImg.color = _iconData.colorFinish;

        _onFinished.Invoke();
    }

}