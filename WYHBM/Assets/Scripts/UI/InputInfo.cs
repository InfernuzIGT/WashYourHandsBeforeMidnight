using Events;
using UnityEngine;
using UnityEngine.UI;

public class InputInfo : MonoBehaviour
{
    [Header("Input Info")]
    [SerializeField] private DEVICE currentDevice = DEVICE.PC;
    [SerializeField] private INPUT_ACTION action = INPUT_ACTION.None;
    [Space]
    [SerializeField] private Image infoImg = null;

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