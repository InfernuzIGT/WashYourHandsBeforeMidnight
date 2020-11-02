using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player", order = 0)]
public class PlayerSO : ScriptableObject
{
    public new string name;
    public AnimatorOverrideController animatorController;
    [Space]
    [PreviewTexture(64)] public Sprite spriteBody;
    [PreviewTexture(64)] public Sprite spriteFace;
}