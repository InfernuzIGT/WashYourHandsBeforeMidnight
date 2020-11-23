using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player", order = 0)]
public class PlayerSO : ScriptableObject
{
    public new string name;
    [PreviewTexture(64)] public Sprite spriteBody;
    public AnimatorOverrideController animatorController;
}