using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputInfo : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private DEVICE currentDevice = DEVICE.PC;
    [SerializeField] private INPUT_ACTION action = INPUT_ACTION.None;

    [Header("UI")]
    [SerializeField] private Image infoImg = null;
    [SerializeField] private TextMeshProUGUI infoTxt = null;

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
        infoImg.sprite = GameData.Instance.deviceConfig.GetIcon(currentDevice, action);
    }

}