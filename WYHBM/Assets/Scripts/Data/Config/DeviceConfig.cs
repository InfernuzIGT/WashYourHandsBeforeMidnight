using UnityEngine;

[System.Serializable]
public class DeviceIcon
{
    [PreviewTexture(48)] public Sprite iconTestA;
    [PreviewTexture(48)] public Sprite iconTestB;
    [PreviewTexture(48)] public Sprite iconTestC;
}

[CreateAssetMenu(fileName = "New DeviceConfig", menuName = "Config/DeviceConfig", order = 0)]
public class DeviceConfig : ScriptableObject
{
    [Header("Device - PC")]
    public DeviceIcon devicePC;
    
    [Header("Device - USB Joystick")]
    public DeviceIcon deviceUSBJoystick;

}