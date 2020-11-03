using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceUtility : MonoBehaviour
{
    [SerializeField, ReadOnly] private DEVICE _currentDevice;
    private DeviceChangeEvent _deviceEvent;
    private DEVICE _lastDevice;

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
                _currentDevice = UniversalFunctions.GetStartDevice();
                break;
        }

        _deviceEvent = new DeviceChangeEvent();
        _deviceEvent.device = _currentDevice;
        EventController.TriggerEvent(_deviceEvent);

        InputSystem.onDeviceChange +=
            (device, change) =>
            {

#if UNITY_EDITOR
                Debug.Log($"<color=yellow><b> Device Change [{change}]:</b></color> {device}");
#endif

                switch (change)
                {
                    case InputDeviceChange.Added:
                    case InputDeviceChange.Reconnected:
                        _lastDevice = _currentDevice;
                        _currentDevice = UniversalFunctions.GetCurrentDevice(device);
                        break;

                    case InputDeviceChange.Removed:
                    case InputDeviceChange.Disconnected:
                        _currentDevice = _lastDevice;
                        UniversalFunctions.PrintCurrentDevice(_currentDevice);
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