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
    [SerializeField, ConditionalHide] private Image _healthBackImg = null;
    [SerializeField, ConditionalHide] private Image _healthFrontImg = null;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        // _canvasGroup = GetComponent<CanvasGroup>();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void UpdateBar(float value)
    {
        _healthBackImg.DOFillAmount(value, _combatConfig.fillDuration);
        _healthFrontImg.fillAmount = value;
    }
    
    public void UpdateBar(float value, TweenCallback action)
    {
        _healthBackImg.DOFillAmount(value, _combatConfig.fillDuration).OnComplete(action);
        _healthFrontImg.fillAmount = value;
    }

    public void Kill()
    {
        // _canvasGroup.
        // DOFade(0, GameData.Instance.combatConfig.canvasFadeDuration).
        // SetEase(Ease.OutQuad);
    }
}