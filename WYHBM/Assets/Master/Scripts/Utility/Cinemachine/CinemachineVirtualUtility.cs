using Cinemachine;
using Events;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineVirtualUtility : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerConfig;

    [Range(0, 100), /* SerializeField */] private float _lookSpeed = 30f;
    [Range(0, 10), /* SerializeField */] private float _limit = 5f;
    [Space]
    [ReadOnly, SerializeField] private int _startIndexZoom = 1;

    private CinemachineVirtualCamera _cinemachineVirtual;
    private CinemachineFramingTransposer _cinemachineFramingTransposer;

    private Vector2 _lookVector;
    private Vector2 _mouseVector;
    [ReadOnly, SerializeField] private Vector2 _tempVector;
    private PlayerController _player;
    private DEVICE _currentDevice;
    private Camera _camera;

    private float _originalX;
    private float _originalZ;
    private bool _isFade;
    private int _indexZoom;
    private bool _canMove = true;
    private bool _isInitialized;

    // Combat
    private bool _inCombat;
    private int _lastIndexZoom;

    private void Awake()
    {
        _cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        _cinemachineFramingTransposer = _cinemachineVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();

        _originalX = _cinemachineFramingTransposer.m_TrackedObjectOffset.x;
        _originalZ = _cinemachineFramingTransposer.m_TrackedObjectOffset.z;
    }

    private void OnEnable()
    {
        // EventController.AddListener<CustomFadeEvent>(OnCustomFade);
        // EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);
        // EventController.AddListener<EnableMovementEvent>(OnStopMovement);
        EventController.AddListener<CombatEvent>(OnCombat);
    }

    private void OnDisable()
    {
        // EventController.RemoveListener<CustomFadeEvent>(OnCustomFade);
        // EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
        // EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        EventController.RemoveListener<CombatEvent>(OnCombat);
    }

    public void Init(PlayerController target, Camera camera)
    {
        _player = target;
        _camera = camera;

        _player.Input.Player.Look.performed += ctx => _lookVector = ctx.ReadValue<Vector2>();
        // InputUIModule.point.action.performed += ctx => _lookVector = ctx.ReadValue<Vector2>();

        _player.Input.Player.Zoom.performed += ctx => ChangeZoom();
        EventSystemUtility.Instance.InputUIModule.middleClick.action.performed += ctx => ChangeZoom();

        _cinemachineVirtual.m_Follow = _player.transform;

        _indexZoom = _startIndexZoom;
        _cinemachineFramingTransposer.m_CameraDistance = _playerConfig.zoomValues[_indexZoom];

        _cinemachineFramingTransposer.m_TrackedObjectOffset.x = _originalX;
        _cinemachineFramingTransposer.m_TrackedObjectOffset.z = _originalZ;

        _isInitialized = true;
    }

    // private void Update()
    // {
    //     if (_isFade || !_canMove || !_isInitialized || !Application.isFocused || _inCombat || _currentDevice == DEVICE.PC)
    //     {
    //         _cinemachineFramingTransposer.m_TrackedObjectOffset.x = _originalX;
    //         _cinemachineFramingTransposer.m_TrackedObjectOffset.z = _originalZ;
    //         return;
    //     }

    //     UpdateValuesGamepad();
        
    //     // if (_currentDevice != DEVICE.PC)
    //     // {
    //     //     UpdateValuesGamepad();
    //     // }
    //     // else
    //     // {
    //     //     UpdateValuesMouse();
    //     // }
    // }

    private void UpdateValuesGamepad()
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

    private void UpdateValuesMouse()
    {
        _mouseVector = _camera.ScreenToViewportPoint(_lookVector);
        _mouseVector -= _playerConfig.mouseOffset;

        if (_mouseVector.x > 0)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.x = Mathf.Lerp(_originalX, _originalX + _limit, _mouseVector.x);
        }
        else if (_mouseVector.x < 0)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.x = Mathf.Lerp(_originalX, _originalX - _limit, Mathf.Abs(_mouseVector.x));
        }

        if (_mouseVector.y > 0)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(_originalZ, _originalZ + _limit, _mouseVector.y);
        }
        else if (_mouseVector.y < 0)
        {
            _cinemachineFramingTransposer.m_TrackedObjectOffset.z = Mathf.Lerp(_originalZ, _originalZ - _limit, Mathf.Abs(_mouseVector.y));
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

    private void OnStopMovement(EnableMovementEvent evt)
    {
        _canMove = evt.canMove;
    }

    private void OnCombat(CombatEvent evt)
    {
        _inCombat = evt.isEnter;

        if (_inCombat)
        {
            _cinemachineVirtual.m_Follow = evt.enemy;

            _lastIndexZoom = _indexZoom;
            _indexZoom = 0;
        }
        else
        {
            _cinemachineVirtual.m_Follow = _player.transform;

            _indexZoom = _lastIndexZoom;
        }

        _cinemachineFramingTransposer.m_CameraDistance = _playerConfig.zoomValues[_indexZoom];
    }

    #endregion
}