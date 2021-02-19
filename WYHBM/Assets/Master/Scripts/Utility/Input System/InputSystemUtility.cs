using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemUtility : MonoBehaviour
{
    [SerializeField, ReadOnly] private DEVICE _currentDevice;
    private DeviceChangeEvent _deviceEvent;
    private DEVICE _lastDevice;
    private bool _isAdded;
    private bool _isReconnected;

    private void Start()
    {
        _deviceEvent = new DeviceChangeEvent();
    }

    public void DetectDevice(InputDevice inputDevice = null)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Switch:
                _lastDevice = DEVICE.Switch;
                _currentDevice = DEVICE.Switch;
                break;

            case RuntimePlatform.PS4:
                _lastDevice = DEVICE.PS4;
                _currentDevice = DEVICE.PS4;
                break;

            case RuntimePlatform.XboxOne:
                _lastDevice = DEVICE.XboxOne;
                _currentDevice = DEVICE.XboxOne;
                break;

            case RuntimePlatform.WindowsPlayer:
                _lastDevice = DEVICE.PC;
                _currentDevice = inputDevice != null ? InputSystemAdapter.GetCurrentDevice(inputDevice) : InputSystemAdapter.GetStartDevice();
                break;

            default:
                _lastDevice = DEVICE.PC;
                _currentDevice = inputDevice != null ? InputSystemAdapter.GetCurrentDevice(inputDevice) : InputSystemAdapter.GetStartDevice();
                break;
        }

        _deviceEvent.showPopup = false;
        _deviceEvent.device = _currentDevice;
        _deviceEvent.gamepad = InputSystemAdapter.GetCurrentGamepad();
        EventController.TriggerEvent(_deviceEvent);

        // InputSystem.onActionChange += (obj, change) =>
        // {
        //     if (change == InputActionChange.ActionStarted)
        //     {
        //         var inputAction = (InputAction)obj;
        //         var lastDevice = inputAction.activeControl.device;

        //         Debug.Log($"Device: {lastDevice.displayName}");
        //     }
        // };

        InputSystem.onDeviceChange +=
            (device, change) =>
            {

                if (InputSystemAdapter.printInfo)Debug.Log($"<color=yellow><b> Device Change [{change}]:</b></color> {device}");

                switch (change)
                {
                    case InputDeviceChange.Added:
                        if (!_isAdded)
                        {
                            _isAdded = true;
                            _isReconnected = true;

                            _lastDevice = _currentDevice;
                            _currentDevice = InputSystemAdapter.GetCurrentDevice(device);
                        }
                        break;

                    case InputDeviceChange.Reconnected:

                        if (!_isReconnected)
                        {
                            _isReconnected = true;

                            _lastDevice = _currentDevice;
                            _currentDevice = InputSystemAdapter.GetCurrentDevice(device);
                        }
                        break;

                    case InputDeviceChange.Removed:
                        if (_isAdded)
                        {
                            _isAdded = false;

                            _currentDevice = _lastDevice;
                            InputSystemAdapter.PrintCurrentDevice(_currentDevice);
                        }
                        break;

                    case InputDeviceChange.Disconnected:
                        if (_isReconnected)
                        {
                            _isReconnected = false;

                            _currentDevice = _lastDevice;
                            InputSystemAdapter.PrintCurrentDevice(_currentDevice);
                        }
                        break;

                        // case InputDeviceChange.ConfigurationChanged:
                        //     break;
                        // case InputDeviceChange.Destroyed:
                        //     break;
                        // case InputDeviceChange.Disabled:
                        //     break;
                        // case InputDeviceChange.Enabled:
                        //     break;
                        // case InputDeviceChange.UsageChanged:
                        //     break;

                    default:
                        break;
                }

                _deviceEvent.showPopup = true;
                _deviceEvent.device = _currentDevice;
                _deviceEvent.gamepad = InputSystemAdapter.GetCurrentGamepad();
                EventController.TriggerEvent(_deviceEvent);

            };

    }
}