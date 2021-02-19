using DG.Tweening;
using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroupUtility))]
public class InputNewDevice : MonoBehaviour
{
    [SerializeField, ReadOnly] private DeviceInfo _deviceInfo = null;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] protected FMODConfig _FMODConfig = null;
    [SerializeField, ConditionalHide] private DeviceConfig _deviceConfig = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _deviceTxt = null;
    [SerializeField, ConditionalHide] private Image _deviceImg = null;

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

        _deviceInfo = _deviceConfig.GetDeviceInfo(evt.device);

        _deviceTxt.text = _deviceInfo.name;
        _deviceImg.sprite = _deviceInfo.icon;

        Play();
    }

    private void Play()
    {
        if (_isPlaying)return;

        _isPlaying = true;

        RuntimeManager.PlayOneShot(_FMODConfig.popupDevice);

        _canvasUtility.Show(true);

        _rectTransform.anchoredPosition = _originalPosition;

        _tweenStart = transform
            .DOLocalMoveX(-350, .5f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0);

        _tweenEnd = transform
            .DOLocalMoveX(350, .5f)
            .SetEase(Ease.InOutSine)
            .SetRelative()
            .SetLoops(0)
            .SetDelay(3)
            .OnComplete(Complete);
    }

    private void Complete()
    {
        _canvasUtility.ShowInstant(false);

        _tweenStart.Kill();
        _tweenEnd.Kill();

        _isPlaying = false;

    }

}