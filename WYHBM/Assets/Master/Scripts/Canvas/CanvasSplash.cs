using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class CanvasSplash : MonoBehaviour
{
    [Header("Splash")]
    [SerializeField] private TextMeshProUGUI _titlePoloTxt = null;
    [SerializeField] private Image _splashPoloImg = null;
    [SerializeField] private Image _splashFMODImg = null;

    private void Start()
    {
        Splash();
    }

    private void Splash()
    {
        _splashPoloImg
            .DOFade(1, 0.5f)
            .SetEase(Ease.Linear);

        _splashPoloImg
            .DOFade(0, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(1.75f)
            .OnComplete(LoadMainMenu);
            
        _titlePoloTxt
            .DOFade(1, 0.5f)
            .SetEase(Ease.Linear);

        _titlePoloTxt
            .DOFade(0, 0.5f)
            .SetEase(Ease.Linear)
            .SetDelay(1.75f);

        // _splashPoloImg
        //     .DOFade(1, 0.5f)
        //     .SetEase(Ease.Linear);

        // _splashPoloImg
        //     .DOFade(0, 0.5f)
        //     .SetEase(Ease.Linear)
        //     .SetDelay(1.75f);

        // _splashFMODImg
        //     .DOFade(1, 0.5f)
        //     .SetEase(Ease.Linear)
        //     .SetDelay(2.5f);

        // _splashFMODImg
        //     .DOFade(0, 0.5f)
        //     .SetEase(Ease.Linear)
        //     .SetDelay(3.75f)
        //     .OnComplete(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadSceneAsync(1); // Main Menu
        Destroy(gameObject);
    }

}