using UnityEngine;

[CreateAssetMenu(fileName = "New Device", menuName = "Other/Device")]
public class DeviceSO : ScriptableObject
{
    [Header("Device Type")]
    public DEVICE type;

    [Header("Icons")]
    [PreviewTexture(48)] public Sprite iconNone;
    [PreviewTexture(48)] public Sprite iconInteraction;
    [PreviewTexture(48)] public Sprite iconAttack;
    [PreviewTexture(48)] public Sprite iconJump;
    [PreviewTexture(48)] public Sprite iconCancel;
    [PreviewTexture(48)] public Sprite iconPause;
    [PreviewTexture(48)] public Sprite iconOptions;

    public Sprite GetIcon(INPUT_ACTION action)
    {
        switch (action)
        {
            case INPUT_ACTION.None:
                return iconNone;

            case INPUT_ACTION.Interaction:
                return iconInteraction;

            case INPUT_ACTION.Attack:
                return iconAttack;

            case INPUT_ACTION.Jump:
                return iconJump;

            case INPUT_ACTION.Cancel:
                return iconCancel;

            case INPUT_ACTION.Pause:
                return iconPause;

            case INPUT_ACTION.Options:
                return iconOptions;

            default:
                return null;;
        }
    }
}