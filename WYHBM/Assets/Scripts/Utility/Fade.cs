using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image _fadeImg;
    private TweenCallback _callbackMid;
    private TweenCallback _callbackEnd;
    private bool _fadeFast;

    private void Start()
    {
        _fadeImg = GetComponent<Image>();
        _fadeImg.enabled = false;
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

        _fadeImg.enabled = true;

        _fadeImg.DOFade(
                1,
                _fadeFast ? GameData.Instance.gameConfig.fadeFastDuration : GameData.Instance.gameConfig.fadeSlowDuration)
            .OnKill(FadeIn);
    }

    private void FadeIn()
    {
        _callbackMid?.Invoke();

        _fadeImg.DOFade(
                0,
                _fadeFast ? GameData.Instance.gameConfig.fadeFastDuration : GameData.Instance.gameConfig.fadeSlowDuration)
            .OnKill(FadeOut);
    }

    private void FadeOut()
    {
        _callbackEnd?.Invoke();

        _fadeImg.enabled = false;
    }

}