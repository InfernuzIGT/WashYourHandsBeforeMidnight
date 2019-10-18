using UnityEngine;

public class EquipmentSO : ScriptableObject
{
    [Header("General")]
    public new string name;
    [TextArea]
    public string description;
    [PreviewTexture(48)]
    public Sprite sprite;
    [Space]
    public TIER tier;
    public int price;
    [Space]
    public int valueMin;
    public int valueMax;

    [Header("Action")]
    public int order;
    public ACTION_TYPE actionType;
    public KeyCode actionKey;
    [TextArea]
    public string actionDescription;
    
}