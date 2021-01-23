using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupUtility))]
public class InputNewDevice : MonoBehaviour
{
    [Header("Input New Device")]
    [SerializeField] private TextMeshProUGUI _deviceTxt = null;
    [SerializeField] private Image _deviceImg = null;

    private CanvasGroupUtility _canvasUtility;
    private RectTransform _rectTransform;
    private bool _isPlaying;
    private Vector2 _originalPosition;
    private Tween _tweenStart;
    private Tween _tweenEnd;

    private void Awake()
    {
        _canvasUtility = GetComponent<CanvasGroupUtility>();

        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
    }

    private void OnDeviceChange(DeviceChangeEvent evt)
    {
        if (!evt.showPopup)return;

        GameData.Instance.SetDeviceInfo(evt.device, ref _deviceTxt, ref _deviceImg);

        Play();
    }

    private void Play()
    {
        if (_isPlaying)return;

        _isPlaying = true;

        _canvasUtility.Show(true);

        _rectTransform.anchoredPosition = _originalPosition;

        _tweenStart = transform
            .DOLocalMoveX(-500, .5f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0);

        _tweenEnd = transform
            .DOLocalMoveX(500, .5f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0)
            .SetDelay(3)
            .OnComplete(Complete);
    }

    public void Complete()
    {
        _canvasUtility.ShowInstant(false);

        _tweenStart.Kill();
        _tweenEnd.Kill();

        _isPlaying = false;

    }

}