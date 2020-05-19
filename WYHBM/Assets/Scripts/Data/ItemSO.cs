using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item", order = 0)]
public class ItemSO : ScriptableObject
{
    [Header("Item Data")]
    public new string name;
    [TextArea] public string description;
    [PreviewTexture(48)] public Sprite sprite;
    [PreviewTexture(48)] public Sprite previewSprite;
    public ITEM_TYPE itemType;
    public int valueMin;
    public int valueMax;
}