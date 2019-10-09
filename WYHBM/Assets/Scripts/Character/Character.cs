#if UNITY_EDITOR
using System.Text;
#endif

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour, IAttackable, IHealeable<float>, IDamageable<float>
{
    [Header("Character")]
    public CharacterSO character;

    [Header("Interface")]
    [SerializeField] private Image _healthBar = null;

#if UNITY_EDITOR
    private StringBuilder _characterData = new StringBuilder();
    public StringBuilder CharacterData { get { return _characterData; } }
#endif    

    private string _name;
    private CHARACTER_TYPE _characterType = CHARACTER_TYPE.none;
    private CHARACTER_BEHAVIOUR _characterBehaviour = CHARACTER_BEHAVIOUR.none;

    private float _healthActual;
    private float _healthMax;
    private float _damage;
    private float _defense;

    private List<WeaponSO> _equipmentWeapon;
    private List<ItemSO> _equipmentItem;
    private List<ArmorSO> _equipmentArmor;

    private BoxCollider2D _boxCollider;
    public BoxCollider2D BoxCollider { get { return _boxCollider; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _equipmentWeapon = new List<WeaponSO>();
        _equipmentItem = new List<ItemSO>();
        _equipmentArmor = new List<ArmorSO>();

        SetCharacter();

        _healthActual = _healthMax;
        _healthBar.DOFillAmount(_healthActual / _healthMax, GameData.Instance.combatConfig.fillDuration);

#if UNITY_EDITOR
        UpdateCharacterData();
#endif    
    }

    private void SetCharacter()
    {
        _name = character.name;
        _characterType = character.characterType;
        _characterBehaviour = character.characterBehaviour;

        _healthMax = character.healthMax;
        _damage = character.damage;
        _defense = character.defense;

        _equipmentWeapon?.AddRange(character.equipmentWeapon);
        _equipmentItem?.AddRange(character.equipmentItem);
        _equipmentArmor?.AddRange(character.equipmentArmor);
    }

    public virtual void ActionAttack()
    {

    }

    public virtual void ActionHeal(float amountHeal)
    {
        _healthActual += amountHeal;
        _healthBar.DOFillAmount(_healthActual / _healthMax, GameData.Instance.combatConfig.fillDuration);

#if UNITY_EDITOR
        UpdateCharacterData();
#endif  
    }

    public virtual void ActionReceiveDamage(float damageReceived)
    {
        if (_healthActual == 0)
            return;

        _healthActual -= damageReceived;
        _healthBar.DOFillAmount(_healthActual / _healthMax, GameData.Instance.combatConfig.fillDuration);

        if (_healthActual <= 0)
        {
            _healthActual = 0;
            _healthBar.DOFillAmount(0, GameData.Instance.combatConfig.fillDuration);
        }

#if UNITY_EDITOR
        UpdateCharacterData();
#endif  
    }

#if UNITY_EDITOR

    private void UpdateCharacterData()
    {
        _characterData.Clear();

        _characterData.AppendFormat(
            "- Name: {0}\n- Type: {1}\n- Behaviour: {2}\n\n- Health: {3}/{4}\n\n- Damage: {5}\n- Defense: {6}",
            _name,
            _characterType.ToString(),
            _characterBehaviour.ToString(),
            _healthActual,
            _healthMax,
            _damage,
            _defense
        );
    }

#endif 

}