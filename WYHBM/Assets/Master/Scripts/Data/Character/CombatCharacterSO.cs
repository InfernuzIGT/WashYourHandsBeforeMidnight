using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Character", menuName = "Characters/Combat", order = 10)]
public class CombatCharacterSO : ScriptableObject
{
    [Header("Character")]
    [SerializeField] private string _name = "-";
    [SerializeField, PreviewTexture(64)] private Sprite _sprite = null;
    [SerializeField, PreviewTexture(64)] private Sprite _previewSprite = null;
    [SerializeField, PreviewTexture(64)] private Sprite _turnSprite = null;
    [SerializeField] private AnimatorOverrideController _animatorController = null;

    [Header("Combat")]
    [SerializeField] private Equipment _equipment = null;
    [Space]
    [SerializeField, Range(0f, 100f)] private int _statsHealthMax = 100;
    [SerializeField, Range(0f, 100f)] private int _statsHealthStart = 100;
    [SerializeField, Range(0f, 20f)] private int _statsBaseDamage = 30;
    [SerializeField, Range(1f, 10f)] private int _statsBaseDefense = 45;
    [SerializeField, Range(1f, 10f)] private int _statsReaction = 1;

    // Properties
    public string Name { get { return _name; } }
    public Sprite Sprite { get { return _sprite; } }
    public Sprite PreviewSprite { get { return _previewSprite; } }
    public Sprite TurnSprite { get { return _turnSprite; } }
    public AnimatorOverrideController AnimatorController { get { return _animatorController; } }
    public Equipment Equipment { get { return _equipment; } }
    public int StatsHealthMax { get { return _statsHealthMax; } }
    public int StatsHealthStart { get { return _statsHealthStart; } }
    public int StatsBaseDamage { get { return _statsBaseDamage; } }
    public int StatsBaseDefense { get { return _statsBaseDefense; } }
    public int StatsReaction { get { return _statsReaction; } }

}