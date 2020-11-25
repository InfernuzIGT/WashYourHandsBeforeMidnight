using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputHoldUtility : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float duration = 5;
    [SerializeField] private Image image = null;

    [Header("Events")]
    [SerializeField] private UnityEvent OnStarted = null;
    [SerializeField] private UnityEvent OnFinish = null;
    [SerializeField] private UnityEvent OnCanceled = null;

    private bool _success;
    private Tween _fillAnimation;

    public void StartHold()
    {
        OnStarted.Invoke();

        _success = false;

        _fillAnimation = image
            .DOFillAmount(1, duration)
            .OnComplete(() => Finish(true));
    }

    public void StopHold()
    {
        _fillAnimation.Kill();

        Finish(false);
    }

    private void Finish(bool success)
    {
        if (_success)return;

        _success = success;

        if (_success)
        {
            OnFinish.Invoke();
            image.fillAmount = 1;
        }
        else
        {
            OnCanceled.Invoke();
            image.fillAmount = 0;
        }
    }

}