using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public class DeviceInfo
{
    public DEVICE type;
    public string name;
    [PreviewTexture(48)] public Sprite icon;
}

[Serializable]
public class DeviceAction
{
    public INPUT_ACTION action = INPUT_ACTION.None;
    public LocalizedString localizedString = new LocalizedString() { TableReference = "UI" };
    [Space]
    [ArrayElementTitle("type")] public ActionIcon[] actionIcon;
}

[Serializable]
public struct ActionIcon
{
    public DEVICE type;
    [PreviewTexture(48)] public Sprite icon;
}

[CreateAssetMenu(fileName = "New DeviceConfig", menuName = "Config/DeviceConfig")]
public class DeviceConfig : ScriptableObject
{
    [Header("Information")]
    [SerializeField, ArrayElementTitle("type")] private DeviceInfo[] deviceInfo;

    [Header("Actions")]
    [SerializeField, ArrayElementTitle("action")] private DeviceAction[] deviceAction;

    private Dictionary<DEVICE, DeviceInfo> deviceDictionary;
    private Dictionary<INPUT_ACTION, DeviceAction> actionDictionary;

    public DeviceInfo GetDeviceInfo(DEVICE device)
    {
        if (deviceDictionary.ContainsKey(device))
        {
            return deviceDictionary[device];
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't find Device {device}");
            return null;
        }
    }

    public Sprite GetDeviceIcon(DeviceAction deviceAction, DEVICE device)
    {
        for (int i = 0; i < deviceAction.actionIcon.Length; i++)
        {
            if (deviceAction.actionIcon[i].type == device)
            {
                return deviceAction.actionIcon[i].icon;
            }
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't find Icon Device {device}");
        return null;
    }

    public DeviceAction GetDeviceAction(INPUT_ACTION action)
    {
        if (actionDictionary.ContainsKey(action))
        {
            return actionDictionary[action];
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't find Action {action}");
            return null;
        }
    }

    public void UpdateDictionary()
    {
        UpdateDeviceDictionary();
        UpdateActionDictionary();
    }

    private void UpdateDeviceDictionary()
    {
        deviceDictionary = new Dictionary<DEVICE, DeviceInfo>();

        for (int i = 0; i < deviceInfo.Length; i++)
        {
            if (!deviceDictionary.ContainsKey(deviceInfo[i].type))
            {
                deviceDictionary.Add(deviceInfo[i].type, deviceInfo[i]);
            }
        }
    }

    private void UpdateActionDictionary()
    {
        actionDictionary = new Dictionary<INPUT_ACTION, DeviceAction>();

        for (int i = 0; i < deviceAction.Length; i++)
        {
            if (!actionDictionary.ContainsKey(deviceAction[i].action))
            {
                actionDictionary.Add(deviceAction[i].action, deviceAction[i]);
            }
        }
    }

}