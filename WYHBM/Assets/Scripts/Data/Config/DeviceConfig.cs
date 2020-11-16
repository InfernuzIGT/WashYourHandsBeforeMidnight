using UnityEngine;

[System.Serializable]
public class DeviceIcon
{
    [PreviewTexture(48)] public Sprite iconNone;
    [PreviewTexture(48)] public Sprite iconInteraction;
}

[CreateAssetMenu(fileName = "New DeviceConfig", menuName = "Config/DeviceConfig", order = 0)]
public class DeviceConfig : ScriptableObject
{
    [Header("Device - Generic")]
    public DeviceIcon deviceGeneric;
    
    [Header("Device - PC")]
    public DeviceIcon devicePC;
    
    // TODO Mariano: Complete
    
    public Sprite GetIcon(DEVICE device, INPUT_ACTION action)
    {
        switch (device)
        {
            case DEVICE.Generic:
                return GetIconGeneric(action);
                
            case DEVICE.PC:
                return GetIconPC(action);
        
            default:
                return null;
        }
    }
    
    private Sprite GetIconGeneric(INPUT_ACTION action)
    {
        switch (action)
        {
            case INPUT_ACTION.None:
                return deviceGeneric.iconNone;
                
            case INPUT_ACTION.Interaction:
                return deviceGeneric.iconInteraction;
        
            default:
                return null;;
        }
    }
    
    private Sprite GetIconPC(INPUT_ACTION action)
    {
        switch (action)
        {
            case INPUT_ACTION.None:
                return devicePC.iconNone;
                
            case INPUT_ACTION.Interaction:
                return devicePC.iconInteraction;
        
            default:
                return null;;
        }
    }

}