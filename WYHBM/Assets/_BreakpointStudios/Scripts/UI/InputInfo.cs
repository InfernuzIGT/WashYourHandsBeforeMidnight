using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class InputInfo : MonoBehaviour
{
    [Header("Input Info")]
    [SerializeField] private DEVICE currentDevice = DEVICE.PC;
    [SerializeField] private INPUT_ACTION action = INPUT_ACTION.None;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private DeviceConfig _deviceConfig = null;
    [SerializeField, ConditionalHide] private Image _infoImg = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringEvent = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _infoTxt = null;

    private DeviceAction _deviceAction;
    
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
        currentDevice = evt.device;
        
        _deviceAction = _deviceConfig.GetDeviceAction(action);

        _infoImg.sprite = _deviceConfig.GetDeviceIcon(_deviceAction, currentDevice);
        _localizeStringEvent.StringReference = _deviceAction.localizedString;
        _localizeStringEvent.OnUpdateString.Invoke(_infoTxt.text);
    }

}