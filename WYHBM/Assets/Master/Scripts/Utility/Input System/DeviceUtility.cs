using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceUtility : MonoBehaviour
{
    [SerializeField, ReadOnly] private DEVICE _currentDevice;
    private DeviceChangeEvent _deviceEvent;
    private DEVICE _lastDevice;
    private bool _isAdded;
    private bool _isReconnected;

    public void DetectDevice()
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

            default:
                _lastDevice = DEVICE.PC;
                _currentDevice = InputUtility.GetStartDevice();
                break;
        }

        _deviceEvent = new DeviceChangeEvent();
        _deviceEvent.device = _currentDevice;
        EventController.TriggerEvent(_deviceEvent);

        InputSystem.onDeviceChange +=
            (device, change) =>
            {

                if (InputUtility.debugMode)Debug.Log($"<color=yellow><b> Device Change [{change}]:</b></color> {device}");

                switch (change)
                {
                    case InputDeviceChange.Added:
                        if (!_isAdded)
                        {
                            _isAdded = true;
                            _isReconnected = true;

                            _lastDevice = _currentDevice;
                            _currentDevice = InputUtility.GetCurrentDevice(device);
                        }
                        break;

                    case InputDeviceChange.Reconnected:

                        if (!_isReconnected)
                        {
                            _isReconnected = true;

                            _lastDevice = _currentDevice;
                            _currentDevice = InputUtility.GetCurrentDevice(device);
                        }
                        break;

                    case InputDeviceChange.Removed:
                        if (_isAdded)
                        {
                            _isAdded = false;

                            _currentDevice = _lastDevice;
                            InputUtility.PrintCurrentDevice(_currentDevice);
                        }
                        break;

                    case InputDeviceChange.Disconnected:
                        if (_isReconnected)
                        {
                            _isReconnected = false;

                            _currentDevice = _lastDevice;
                            InputUtility.PrintCurrentDevice(_currentDevice);
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

                _deviceEvent.device = _currentDevice;
                EventController.TriggerEvent(_deviceEvent);

            };
    }
}