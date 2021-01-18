using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Hold Icon", menuName = "Other/Hold Icon")]
public class HoldIconSO : ScriptableObject
{
    [System.Serializable]
    public class Icon
    {
        [PreviewTexture(48)] public Sprite sprite;
        public Color color = new Color(1, 1, 1, 1);
    }

    [Header("Fill Bar")]
    public Color colorStart = new Color(1, 1, 1, 1);
    public Color colorFinish = new Color(1, 1, 1, 1);

    [Header("Icons")]
    public Icon iconStart;
    public Icon iconCancel;
    public Icon iconFinish;

    public void SetIconStart(ref Image icon)
    {
        SetIcon(ref icon, iconStart);
    }

    public void SetIconCancel(ref Image icon)
    {
        SetIcon(ref icon, iconCancel);
    }

    public void SetIconFinish(ref Image icon)
    {
        SetIcon(ref icon, iconFinish);
    }

    private void SetIcon(ref Image icon, Icon iconData)
    {
        if (iconData.sprite != null)
        {
            icon.enabled = true;

            icon.sprite = iconData.sprite;
            icon.color = iconData.color;
        }
        else
        {
            icon.enabled = false;
        }
    }

}