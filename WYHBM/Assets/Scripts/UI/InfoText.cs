using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;

public class InfoText : MonoBehaviour
{
    public TextMeshProUGUI infoTxt;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        EventController.AddListener<InfoTextEvent>(InitInfoText);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<InfoTextEvent>(InitInfoText);
    }

    private void InitInfoText(InfoTextEvent evt)
    {
        transform.position = evt.position;
        infoTxt.text = evt.text;

        _canvasGroup.alpha = 1;

        transform.
        DOMoveY(GameData.Instance.combatConfig.positionYTextEnd, GameData.Instance.combatConfig.infoTextMoveDuration).
        SetEase(Ease.OutQuad);

        _canvasGroup.
        DOFade(0, GameData.Instance.combatConfig.infoTextFadeDuration).
        SetEase(Ease.OutQuad).
        SetDelay(GameData.Instance.combatConfig.infoTextFadeDelay);
    }
}