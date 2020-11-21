using UnityEngine;

[CreateAssetMenu(fileName = "New Device", menuName = "Device", order = 0)]
public class DeviceSO : ScriptableObject
{
    [Header("Device Type")]
    public DEVICE type;

    [Header("Icons")]
    [PreviewTexture(48)] public Sprite iconNone;
    [PreviewTexture(48)] public Sprite iconInteraction;

    public Sprite GetIcon(INPUT_ACTION action)
    {
        switch (action)
        {
            case INPUT_ACTION.None:
                return iconNone;

            case INPUT_ACTION.Interaction:
                return iconInteraction;

            default:
                return null;;
        }
    }
}