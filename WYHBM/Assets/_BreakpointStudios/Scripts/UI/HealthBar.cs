using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private CombatConfig _combatConfig = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasUtility = null;
    [SerializeField, ConditionalHide] private Image _healthBackImg = null;
    [SerializeField, ConditionalHide] private Image _healthFrontImg = null;

    private CanvasGroup _canvasGroup;
    private Tween _barAnimation;

    private void Start()
    {
        _canvasUtility.SetCanvasCamera();
    }

    public void UpdateBar(bool isDamage, float value)
    {
        _barAnimation.Kill();

        if (isDamage)
        {
            _barAnimation = _healthBackImg.DOFillAmount(value, _combatConfig.fillDuration);
            _healthFrontImg.fillAmount = value;
        }
        else
        {
            _healthBackImg.fillAmount = value;
            _barAnimation = _healthFrontImg.DOFillAmount(value, _combatConfig.fillDuration);
        }
    }

    public void UpdateBar(bool isDamage, float value, TweenCallback action)
    {
        _barAnimation.Kill();

        if (isDamage)
        {
            _barAnimation = _healthBackImg.DOFillAmount(value, _combatConfig.fillDuration).OnComplete(action);
            _healthFrontImg.fillAmount = value;
        }
        else
        {
            _healthBackImg.fillAmount = value;
            _barAnimation = _healthFrontImg.DOFillAmount(value, _combatConfig.fillDuration).OnComplete(action);
        }
    }

    public void Kill()
    {
        _canvasUtility.ShowInstant(false);
        // _canvasUtility.Show(false, 0, _combatConfig.waitTimePerAction);
    }
}