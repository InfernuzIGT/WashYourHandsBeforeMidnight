using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CanvasSplash : MonoBehaviour
{
    [Header("Splash")]
    [SerializeField] private Image _splashImg = null;

    private void Start()
    {
        FadeIn();
    }

    private void FadeIn()
    {
        _splashImg
            .DOFade(1, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(FadeOut);
    }

    private void FadeOut()
    {
        _splashImg
            .DOFade(0, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(1)
            .OnComplete(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(1); // Main Menu
        Destroy(gameObject);
    }

}