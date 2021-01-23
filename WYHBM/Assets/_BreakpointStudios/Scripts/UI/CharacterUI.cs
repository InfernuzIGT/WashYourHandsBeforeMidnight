using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Image healthBar;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void Kill()
    {
        // _canvasGroup.
        // DOFade(0, GameData.Instance.combatConfig.canvasFadeDuration).
        // SetEase(Ease.OutQuad);
    }
}