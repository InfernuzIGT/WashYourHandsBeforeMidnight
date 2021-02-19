using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "DEPRECATED/Item")]
public class ItemSO : ScriptableObject
{
    [Header("General")]
    public string title;
    [TextArea] public string description;
    [PreviewTexture(48)] public Sprite icon;

    [Header("Data")]
    public ITEM_TYPE type;
    public Vector2Int value;
    [Range(0f, 1f)] public float probability = 1;
}