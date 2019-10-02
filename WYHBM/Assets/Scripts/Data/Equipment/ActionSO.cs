using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "Action", order = 0)]
public class ActionSO : ScriptableObject
{
    [Header("General")]
    public string title;
    public ACTION_TYPE actionType;
    [TextArea]
    public string description;
    [Space]
    [PreviewTexture(48)]
    public Sprite actionSprite;
    public string information;
    public KeyCode actionKey;
    
    public float value;
}