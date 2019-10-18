#if UNITY_EDITOR
using System.Text;
#endif

using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour, ICombatable, IHealeable<float>, IDamageable<float>
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
    private Vector3 _scaleNormal;
    private Vector2 _infoTextPosition;

    private List<WeaponSO> _equipmentWeapon;
    private List<ItemSO> _equipmentItem;
    private List<ArmorSO> _equipmentArmor;

    private InfoTextEvent infoTextEvent = new InfoTextEvent();

    private Vector3 _startPosition;
    public Vector3 StartPosition { get { return _startPosition; } }

    private BoxCollider2D _boxCollider;
    public BoxCollider2D BoxCollider { get { return _boxCollider; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        _equipmentWeapon = new List<WeaponSO>();
        _equipmentItem = new List<ItemSO>();
        _equipmentArmor = new List<ArmorSO>();

        _scaleNormal = transform.localScale;
        _startPosition = transform.position;
        _infoTextPosition = new Vector2(transform.position.x, GameData.Instance.combatConfig.positionYTextStart);

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

    public virtual void ActionStartCombat()
    {
        transform.DOScale(GameData.Instance.combatConfig.scaleCombat, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void ActionStopCombat()
    {
        transform.DOScale(_scaleNormal, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void ActionHeal(float amountHeal)
    {
        _healthActual += amountHeal;

        ShowInfoText(amountHeal);

        if (_healthActual > _healthMax)_healthActual = _healthMax;

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

        ShowInfoText(damageReceived);

        if (_healthActual <= 0)
        {
            _healthActual = 0;
            _healthBar.DOFillAmount(0, GameData.Instance.combatConfig.fillDuration);
        }

#if UNITY_EDITOR
        UpdateCharacterData();
#endif  
    }

    private void ShowInfoText(float value)
    {
        infoTextEvent.text = value.ToString("F0");
        infoTextEvent.position = _infoTextPosition;
        EventController.TriggerEvent(infoTextEvent);
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