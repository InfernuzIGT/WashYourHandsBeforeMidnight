using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player", order = 0)]
public class PlayerSO : ScriptableObject
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField, PreviewTexture(64)] private Sprite _sprite;
    [SerializeField] private AnimatorOverrideController _animatorController;

    // Properties
    public int ID { get { return _id; } }
    public string Name { get { return _name; } }
    public Sprite Sprite { get { return _sprite; } }
    public AnimatorOverrideController AnimatorController { get { return _animatorController; } }
}