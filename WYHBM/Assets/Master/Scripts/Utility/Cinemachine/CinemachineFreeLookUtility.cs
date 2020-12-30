using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CinemachineFreeLookUtility : MonoBehaviour
{
    [Range(0, 100), SerializeField] private float _lookSpeedX = 100f;
    [Range(0, 1), SerializeField] private float _lookSpeedY = 0.5f;

    private CinemachineFreeLook _cinemachineFreeLook;
    private Vector2 _lookMovement;
    private bool _invertY;
    private Vector2 _originalCameraPosition;

    private void Awake()
    {
        _cinemachineFreeLook = GetComponent<CinemachineFreeLook>();

        _originalCameraPosition = new Vector2(0, 0.5f);
    }

    public void Init(PlayerController target, InputSystemUIInputModule InputUIModule)
    {
        target.Input.Player.Look.performed += ctx => _lookMovement = ctx.ReadValue<Vector2>().normalized;
        InputUIModule.point.action.performed += ctx => _lookMovement = ctx.ReadValue<Vector2>().normalized;

        target.Input.Player.ResetCamera.performed += ctx => ResetCamera();
        InputUIModule.middleClick.action.performed += ctx => ResetCamera();

        _cinemachineFreeLook.m_Follow = target.transform;
        _cinemachineFreeLook.m_LookAt = target.transform;
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