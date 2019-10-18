using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour, ICombatable, IHealeable<int>, IDamageable<int>
{
    [Header("Character")]
    public CharacterSO character;

    [Header("Interface")]
    [SerializeField] private CharacterUI characterUI = null;

    private string _name;
    private CHARACTER_TYPE _characterType = CHARACTER_TYPE.none;
    private CHARACTER_BEHAVIOUR _characterBehaviour = CHARACTER_BEHAVIOUR.none;

    private float _healthActual;
    private float _healthMax;
    private int _damage;
    private int _defense;
    private Vector3 _scaleNormal;
    private Vector2 _infoTextPosition;

    private List<WeaponSO> _equipmentWeapon;
    private List<ItemSO> _equipmentItem;
    private List<ArmorSO> _equipmentArmor;

    private InfoTextEvent infoTextEvent = new InfoTextEvent();

    private bool _isAlive;
    public bool IsAlive { get { return _isAlive; } }

    private Vector3 _startPosition;
    public Vector3 StartPosition { get { return _startPosition; } }

    private BoxCollider2D _boxCollider;
    public BoxCollider2D BoxCollider { get { return _boxCollider; } }

    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        characterUI.healthBar.DOFillAmount(_healthActual / _healthMax, GameData.Instance.combatConfig.fillDuration);
    }

    public virtual void SetCharacter()
    {
        _isAlive = true;
        _name = character.name;
        _characterType = character.characterType;
        _characterBehaviour = character.characterBehaviour;

        _healthMax = character.healthMax;
        _damage = character.damage;
        _defense = character.defense;

        _equipmentWeapon?.AddRange(character.equipmentWeapon);
        _equipmentItem?.AddRange(character.equipmentItem);
        _equipmentArmor?.AddRange(character.equipmentArmor);

        _spriteRenderer.sprite = character.spriteCharacter;
    }

    public virtual void ActionStartCombat()
    {
        transform.DOScale(GameData.Instance.combatConfig.scaleCombat, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void ActionStopCombat()
    {
        transform.DOScale(_scaleNormal, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void ActionHeal(int amountHeal)
    {
        _healthActual += amountHeal;

        ShowInfoText(amountHeal);

        if (_healthActual > _healthMax)_healthActual = _healthMax;

        characterUI.healthBar.DOFillAmount(_healthActual / _healthMax, GameData.Instance.combatConfig.fillDuration);
    }

    public virtual void ActionDefense(int amountDefense)
    {
        _defense = amountDefense;
    }

    public virtual void ActionReceiveDamage(int damageReceived)
    {
        if (_healthActual == 0)
            return;

        _healthActual -= damageReceived;

        _isAlive = _healthActual >= 0;

        characterUI.healthBar.
        DOFillAmount(_healthActual / _healthMax, GameData.Instance.combatConfig.fillDuration).
        OnComplete(Kill);

        ShowInfoText(damageReceived);
    }

    private void ShowInfoText(float value)
    {
        infoTextEvent.text = value.ToString("F0");
        infoTextEvent.position = _infoTextPosition;
        EventController.TriggerEvent(infoTextEvent);
    }

    private void Kill()
    {
        if (_healthActual <= 0)
        {
            _healthActual = 0;

            characterUI.Kill();

            _spriteRenderer.
            DOFade(0, GameData.Instance.combatConfig.canvasFadeDuration).
            SetEase(Ease.OutQuad).OnComplete(CheckGame);
        }
    }

    private void CheckGame()
    {
        // TODO Mariano: ADD EVENT TO CHECK THE LIST OF ENEMIES
        gameObject.SetActive(false);
    }

}