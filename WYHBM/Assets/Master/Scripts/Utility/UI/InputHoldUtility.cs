using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FMODUnity;

[RequireComponent(typeof(CanvasGroupUtility))]
public class InputHoldUtility : MonoBehaviour, IHoldeable
{
    [Header("FMOD")]
    public StudioEventEmitter zombieAlertSound;
    public StudioEventEmitter chargeInteractionSound;

    [Header("Hold System")]
    [SerializeField, Range(0f, 10f)] private float _duration = 3;
    [SerializeField] private Ease _easeAnimation = Ease.Linear;
    [SerializeField] private bool _cancelOnPause = false;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private HoldIconSO _iconData = null;
    [SerializeField, ConditionalHide] private Image _iconImg = null;
    [SerializeField, ConditionalHide] private Image _fillImg = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasGroupUtility = null;
    public bool isDoor;

    private Tween _fillAnimation;
    private Tween _fillColor;
    private bool _isStarted;

    // Properties
    private UnityEvent _onStarted = new UnityEvent();
    public UnityEvent OnStarted { get { return _onStarted; } }

    private UnityEvent _onCanceled = new UnityEvent();
    public UnityEvent OnCanceled { get { return _onCanceled; } }

    private UnityEvent _onFinished = new UnityEvent();
    public UnityEvent OnFinished { get { return _onFinished; } }

    public float Duration { get { return _duration; } set { _duration = value; } }

    private void Start()
    {
        _iconData.SetIconStart(ref _iconImg);

        _canvasGroupUtility.SetCanvasCamera();
        _canvasGroupUtility.ShowInstant(false);
    }

    private void OnEnable()
    {
        EventController.AddListener<PauseEvent>(OnPause);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<PauseEvent>(OnPause);
    }

    private void OnPause(PauseEvent evt)
    {
        if (_isStarted)
        {
            if (evt.isPaused)
            {
                _fillAnimation.Pause();
                _fillColor.Pause();
            }
            else
            {
                _fillAnimation.Play();
                _fillColor.Play();
            }
        }

        if (!_cancelOnPause)return;

        OnCancel();
    }

    public void SoundDetect(bool enable)
    {
        // TODO Mariano: Implementar bien
        // if (enable)
        // {
        //     _iconData.SetIconStart(ref _iconImg);
        //     _canvasGroupUtility.ShowInstant(true);

        //     _fillImg.fillAmount = 0;
        //     _fillImg.color = _iconData.colorStart;
        // }
        // else
        // {
        //     _iconData.SetIconCancel(ref _iconImg);

        //     _canvasGroupUtility.Show(false);

        //     _fillAnimation.Kill();
        //     _fillColor.Kill();

        //     _fillImg.fillAmount = 0;
        //     _fillImg.color = _iconData.colorStart;
        // }
    }

    public void OnStart()
    {
        // zombieAlertSound.Play();
        // chargeInteractionSound.Play();

        _isStarted = true;

        _iconData.SetIconStart(ref _iconImg);

        _canvasGroupUtility.ShowInstant(true);

        _fillImg.fillAmount = 0;
        _fillImg.color = _iconData.colorStart;

        _onStarted.Invoke();

        _fillAnimation = _fillImg
            .DOFillAmount(1, _duration)
            .SetEase(_easeAnimation)
            .OnComplete(() => OnFinish());

        _fillColor = _fillImg
            .DOColor(_iconData.colorFinish, _duration)
            .SetEase(_easeAnimation);
    }

    public void OnCancel()
    {
        _isStarted = false;

        _iconData.SetIconCancel(ref _iconImg);

        _canvasGroupUtility.Show(false);

        _fillAnimation.Kill();
        _fillColor.Kill();

        _fillImg.fillAmount = 0;
        _fillImg.color = _iconData.colorStart;

        _onCanceled.Invoke();
    }

    public void OnFinish()
    {
        // zombieAlertSound.Stop();
        // chargeInteractionSound.Stop();

        _isStarted = false;

        _iconData.SetIconFinish(ref _iconImg);

        _canvasGroupUtility.Show(false, 1f);

        _fillAnimation.Kill();
        _fillColor.Kill();

        _fillImg.fillAmount = 1;
        _fillImg.color = _iconData.colorFinish;

        _onFinished.Invoke();
    }

}