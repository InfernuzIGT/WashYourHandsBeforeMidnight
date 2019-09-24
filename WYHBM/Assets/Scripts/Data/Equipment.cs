using UnityEngine;

public class Equipment : ScriptableObject
{
    [Header ("General")]
    public new string name;
    [TextArea]
    public string description;
    [PreviewTexture(48)]
    public Sprite sprite;
    [Space]
    public TIER tier;
    public float price;
}