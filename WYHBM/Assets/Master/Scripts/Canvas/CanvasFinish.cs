using System.Collections;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFinish : MonoBehaviour
{
    [Header("Finish")]
    [SerializeField] private TextMeshProUGUI _finishTxt = null;

    private CustomFadeEvent _customfadeEvent;
    private EnableMovementEvent _enableMovementEvent;

    private void Start()
    {
        _customfadeEvent = new CustomFadeEvent();
        _customfadeEvent.instant = false;
        _customfadeEvent.fadeIn = true;
        _customfadeEvent.duration = 5;

        _enableMovementEvent = new EnableMovementEvent();
        _enableMovementEvent.canMove = false;
        _enableMovementEvent.isDetected = false;

        EventController.TriggerEvent(_customfadeEvent);
        EventController.TriggerEvent(_enableMovementEvent);

        FadeIn();
    }

    private void FadeIn()
    {
        _finishTxt
            .DOFade(1, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(5)
            .OnComplete(FadeOut);
    }

    private void FadeOut()
    {
        _finishTxt
            .DOFade(0, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(5)
            .OnComplete(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);

        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.FatalError);

        // SceneManager.LoadSceneAsync(1); // Main Menu
        // Destroy(gameObject);
    }
}