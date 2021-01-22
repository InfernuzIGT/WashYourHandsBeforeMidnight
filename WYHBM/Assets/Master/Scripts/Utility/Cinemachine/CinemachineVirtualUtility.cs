using Cinemachine;
using Events;
using UnityEngine;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineVirtualUtility : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;

    [Range(0, 100), SerializeField] private float _lookSpeed = 30f;
    [Range(0, 10), SerializeField] private float _limit = 5f;
    [Space]
    [SerializeField] private int _startIndexZoom = 2;
    [Range(0, 10), SerializeField] private float _zoomSpeedY = 2f;

    private CinemachineVirtualCamera _cinemachineVirtual;
    private CinemachineFramingTransposer _cinemachineFramingTransposer;

    private Vector2 _lookVector;
    private Vector2 _zoomVector;
    // private bool _invertY;
    private PlayerController _player;
    private DEVICE _currentDevice;

    private float _originalX;
    private float _originalZ;
    private bool _isFade;
    private int _indexZoom;

    private void Awake()
    {
        _cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        _cinemachineFramingTransposer = _cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();

        _originalX = _cinemachineFramingTransposer.m_TrackedObjectOffset.x;
        _originalZ = _cinemachineFramingTransposer.m_TrackedObjectOffset.z;
    }

    private void OnEnable()
    {
        EventController.AddListener<CustomFadeEvent>(OnCustomFade);
        EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<CustomFadeEvent>(OnCustomFade);
        EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
    }

    public void Init(PlayerController target, InputSystemUIInputModule InputUIModule)
    {
        _player = target;

        _player.Input.Player.Look.performed += ctx => _lookVector = ctx.ReadValue<Vector2>();
        InputUIModule.point.action.performed += ctx => _lookVector = ctx.ReadValue<Vector2>();

        _player.Input.Player.Zoom.performed += ctx => _zoomVector = ctx.ReadValue<Vector2>().normalized;
        InputUIModule.scrollWheel.action.performed += ctx => _zoomVector = ctx.ReadValue<Vector2>().normalized;

        _player.Input.Player.ResetCamera.performed += ctx => ChangeZoom();
        InputUIModule.middleClick.action.performed += ctx => ChangeZoom();

        _cinemachineVirtual.m_Follow = _player.transform;

        _indexZoom = _startIndexZoom;
        _cinemachineFramingTransposer.m_CameraDistance = _playerConfig.zoomValues[_indexZoom];
    }

    private void Update()
    {
        if (_isFade)return;

        if (_currentDevice != DEVICE.PC)
        {
            // TODO Mariano: Add Invert option

            UpdateValueX();
            UpdateValueZ();
        }
        else
        {
            // TODO Mariano: Add PC Input
            // _cinemachineVirtual.m_RecenterToTargetHeading.m_enabled = false;

            // _zoomVector.y = _invertY ? -_zoomVector.y : _zoomVector.y;

            // _cinemachineVirtual.m_YAxis.Value += _zoomVector.y * _zoomSpeedY * Time.deltaTime;
        }
    }

    private void UpdateValueX()
    {
        if (_lookVector.x > _playerConfig.stickDeadZone)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.x = Mathf.Lerp(_originalX, _originalX + _limit, _lookVector.x);

        }
        else if (_lookVector.x < -_playerConfig.stickDeadZone)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.x = Mathf.Lerp(_originalX, _originalX - _limit, Mathf.Abs(_lookVector.x));
        }
        else
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.x = _originalX + _lookVector.x * _lookSpeed * Time.deltaTime;
        }

    }

    private void UpdateValueZ()
    {
        if (_lookVector.y > _playerConfig.stickDeadZone)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(_originalZ, _originalZ + _limit, _lookVector.y);
        }
        else if (_lookVector.y < -_playerConfig.stickDeadZone)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(_originalZ, _originalZ - _limit, Mathf.Abs(_lookVector.y));
        }
        else
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.z = _originalZ + _lookVector.y * _lookSpeed * Time.deltaTime;
        }
    }

    public void SetFOV(float value)
    {
        _cinemachineVirtual.m_Lens.FieldOfView = value;
    }

    private void ChangeZoom()
    {
        _indexZoom = _indexZoom > 0 ? _indexZoom - 1 : _playerConfig.zoomValues.Length - 1;

        _cinemachineFramingTransposer.m_CameraDistance = _playerConfig.zoomValues[_indexZoom];
    }

    #region Events

    private void OnCustomFade(CustomFadeEvent evt)
    {
        _isFade = evt.fadeIn;
    }

    private void OnDeviceChange(DeviceChangeEvent evt)
    {
        _currentDevice = evt.device;
    }

    #endregion
}