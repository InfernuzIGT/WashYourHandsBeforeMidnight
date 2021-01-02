using Cinemachine;
using Events;
using UnityEngine;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CinemachineFreeLookUtility : MonoBehaviour
{
    [Range(0, 100), SerializeField] private float _lookSpeedX = 100f;
    [Range(0, 1), SerializeField] private float _lookSpeedY = 0.5f;

    private CinemachineFreeLook _cinemachineFreeLook;
    private CinemachineOrbitalTransposer[] _cinemachineOrbitalTransposers;
    private Vector3[] _cinemachineDamping;
    private Vector2 _lookMovement;
    private bool _invertY;
    private Vector2 _originalCameraPosition;
    private PlayerController _player;

    private void Awake()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();
        
        _cinemachineOrbitalTransposers = new CinemachineOrbitalTransposer[3];
        _cinemachineDamping = new Vector3[3];

        for (int i = 0; i < 3; i++)
        {
            _cinemachineOrbitalTransposers[i] = _cinemachineFreeLook.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>();

            _cinemachineDamping[i] = new Vector3(_cinemachineOrbitalTransposers[i].m_XDamping, _cinemachineOrbitalTransposers[i].m_YDamping, _cinemachineOrbitalTransposers[i].m_ZDamping);
        }
        
        _originalCameraPosition = new Vector2(0, 0.5f);
    }

    private void OnEnable()
    {
        EventController.AddListener<CustomFadeEvent>(OnCustomFade);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<CustomFadeEvent>(OnCustomFade);
    }

    private void OnCustomFade(CustomFadeEvent evt)
    {
        for (int i = 0; i < 3; i++)
        {
            _cinemachineOrbitalTransposers[i].m_XDamping = evt.fadeIn ? 0 : _cinemachineDamping[i].x;
            _cinemachineOrbitalTransposers[i].m_YDamping = evt.fadeIn ? 0 : _cinemachineDamping[i].y;
            _cinemachineOrbitalTransposers[i].m_ZDamping = evt.fadeIn ? 0 : _cinemachineDamping[i].z;
        }
    }

    public void Init(PlayerController target, InputSystemUIInputModule InputUIModule)
    {
        _player = target;

        _player.Input.Player.Look.performed += ctx => _lookMovement = ctx.ReadValue<Vector2>().normalized;
        InputUIModule.point.action.performed += ctx => _lookMovement = ctx.ReadValue<Vector2>().normalized;

        _player.Input.Player.ResetCamera.performed += ctx => ResetCamera();
        InputUIModule.middleClick.action.performed += ctx => ResetCamera();

        _cinemachineFreeLook.m_Follow = _player.transform;
        _cinemachineFreeLook.m_LookAt = _player.transform;
    }

    private void Update()
    {
        OnLook();
    }

    private void OnLook()
    {
        _cinemachineFreeLook.m_RecenterToTargetHeading.m_enabled = Mathf.Abs(_lookMovement.x) < 0.9f;

        _lookMovement.y = _invertY ? -_lookMovement.y : _lookMovement.y;

        _cinemachineFreeLook.m_XAxis.Value += _lookMovement.x * _lookSpeedX * Time.deltaTime;
        _cinemachineFreeLook.m_YAxis.Value += _lookMovement.y * _lookSpeedY * Time.deltaTime;
    }

    private void ResetCamera()
    {
        _lookMovement = _originalCameraPosition;

        _cinemachineFreeLook.m_XAxis.Value = _lookMovement.x;
        _cinemachineFreeLook.m_YAxis.Value = _lookMovement.y;
    }
}