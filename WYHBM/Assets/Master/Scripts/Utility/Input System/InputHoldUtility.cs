using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupUtility))]
public class InputHoldUtility : MonoBehaviour, IHoldeable
{
    [Header("Hold System")]
    [SerializeField, Range(0f, 10f)] private float _duration = 3;
    [SerializeField] private Image _fillImage;

    private CanvasGroupUtility _canvasGroupUtility;
    private Tween _fillAnimation;

    // Properties
    private UnityEvent _onStarted = new UnityEvent();
    public UnityEvent OnStarted { get { return _onStarted; } }
    private UnityEvent _onCanceled = new UnityEvent();
    public UnityEvent OnCanceled { get { return _onCanceled; } }
    private UnityEvent _onFinished = new UnityEvent();
    public UnityEvent OnFinished { get { return _onFinished; } }

    private void Start()
    {
        _canvasGroupUtility = GetComponent<CanvasGroupUtility>();
        _canvasGroupUtility.SetCanvasCamera();
        _canvasGroupUtility.ShowInstant(false);
    }
    public void OnStart()
    {
        _canvasGroupUtility.ShowInstant(true);
        _fillImage.fillAmount = 0;
        
        _onStarted.Invoke();

        _fillAnimation = _fillImage
            .DOFillAmount(1, _duration)
            .OnComplete(() => OnFinish());
    }

    public void OnCancel()
    {
        _canvasGroupUtility.Show(false);
        
        _fillAnimation.Kill();

        _fillImage.fillAmount = 0;

        _onCanceled.Invoke();
    }

    public void OnFinish()
    {
        _canvasGroupUtility.Show(false, 1f);
        
        _fillImage.fillAmount = 1;

        _onFinished.Invoke();
    }

}