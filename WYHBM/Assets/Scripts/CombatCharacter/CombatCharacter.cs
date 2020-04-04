using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;

public class CombatCharacter : MonoBehaviour
{
    [Header("Character")]
    public CharacterSO character;

    [Header("Interface")]
    [SerializeField] private CharacterUI characterUI = null; // TODO Mariano: Instancia el prefab como hijo

    private string _name;
    private CHARACTER_TYPE _characterType = CHARACTER_TYPE.none;
    private CHARACTER_BEHAVIOUR _characterBehaviour = CHARACTER_BEHAVIOUR.none;

    private float _healthActual;
    private bool _isActionDone;

    private Vector3 _scaleNormal;
    private Vector2 _infoTextPosition;

    private List<WeaponSO> _equipmentWeapon;
    private List<ItemSO> _equipmentItem;
    private List<ArmorSO> _equipmentArmor;

    private InfoTextEvent infoTextEvent = new InfoTextEvent();

    #region Properties

    private bool _isAlive;
    public bool IsAlive { get { return _isAlive; } }

    private bool _isMyTurn;
    public bool IsMyTurn { get { return _isMyTurn; } set { _isMyTurn = value; } }

    private Vector3 _startPosition;
    public Vector3 StartPosition { get { return _startPosition; } }

    // private BoxCollider _boxCollider;
    // public BoxCollider BoxCollider { get { return _boxCollider; } }

    private SpriteRenderer _spriteRenderer;
    // public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }

    private int _statsHealthMax;
    public int StatsHealthMax { get { return _statsHealthMax; } }

    private int _statsDamageMelee;
    public int StatsDamageMelee { get { return _statsDamageMelee; } }

    private int _statsDamageDistance;
    public int StatsDamageDistance { get { return _statsDamageDistance; } }

    private int _statsStrength;
    public int StatsStrength { get { return _statsStrength; } }

    private int _statsDexterity;
    public int StatsDexterity { get { return _statsDexterity; } }

    private int _statsDefense;
    public int StatsDefense { get { return _statsDefense; } }

    private int _statsAgility;
    public int StatsAgility { get { return _statsAgility; } }

    private int _statsLuck;
    public int StatsLuck { get { return _statsLuck; } }

    private int _statsReaction;
    public int StatsReaction { get { return _statsReaction; } }

    #endregion

    private void Awake()
    {
        // _boxCollider = GetComponent<BoxCollider>();
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

        _healthActual = _statsHealthMax;
        characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration);
    }

    public virtual void SetCharacter()
    {
        _isAlive = true;
        _isMyTurn = false;
        _name = character.name;
        _characterType = character.characterType;
        _characterBehaviour = character.characterBehaviour;

        // Stats
        _statsHealthMax = character.statsHealthMax;
        _statsDamageMelee = character.statsDamageMelee;
        _statsDamageDistance = character.statsDamageDistance;
        _statsStrength = character.statsStrength;
        _statsDexterity = character.statsDexterity;
        _statsDefense = character.statsDefense;
        _statsAgility = character.statsAgility;
        _statsLuck = character.statsLuck;
        _statsReaction = character.statsReaction;

        _equipmentWeapon?.AddRange(character.equipmentWeapon);
        _equipmentItem?.AddRange(character.equipmentItem);
        _equipmentArmor?.AddRange(character.equipmentArmor);

        // _spriteRenderer.sprite = character.spriteCharacter;
    }

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

    public void DoAction()
    {
        if (_isMyTurn)
        {
            _isActionDone = true;
            _isMyTurn = false;
        }
    }

    public Coroutine StartWaitingForAction()
    {
        return StartCoroutine(WaitingForAction());
    }

    private IEnumerator WaitingForAction()
    {
        _isActionDone = false;

        while (!_isActionDone)
        {
            yield return null;
        }
        yield return null;
    }

    public void StartGettingAhead()
    {
        StartCoroutine(GettingAhead());
    }

    private IEnumerator GettingAhead()
    {
        float elapsedTime = 0;

        float timeThreshold = (10 / _statsReaction) * GameData.Instance.combatConfig.actionTimeThresholdMultiplier;

        while (elapsedTime <= timeThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // TODO Mariano: Add to COMBAT MANAGER
        // TurnsController.control.CharacterIsReadyToGoAhead(this);
    }

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

    public virtual void StartCombat()
    {
        SetCharacter();
        _statsDamageMelee *= _statsStrength;
    }

    public virtual void ActionStartCombat()
    {
        transform.DOScale(GameData.Instance.combatConfig.scaleCombat, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void ActionStopCombat()
    {
        transform.DOScale(_scaleNormal, GameData.Instance.combatConfig.transitionDuration);

        // _defense = 0; // TODO Mariano: Use the default Defense
    }

    public virtual void SetDefense(int newDefense)
    {
        _statsDefense = newDefense;
    }

    public virtual void ActionHeal(int amountHeal)
    {
        _healthActual += amountHeal;

        ShowInfoText(amountHeal, GameData.Instance.textConfig.colorMsgHeal);

        if (_healthActual > _statsHealthMax)_healthActual = _statsHealthMax;

        characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration);
    }

    public virtual void ActionDefense(int amountDefense)
    {
        _statsDefense = amountDefense;

        ShowInfoText(amountDefense, GameData.Instance.textConfig.colorMsgDefense);
    }

    public virtual void ActionReceiveDamage(int damageReceived)
    {
        if (_healthActual == 0)
            return;

        int totalDamage = damageReceived - _statsDefense;

        _healthActual -= totalDamage;
        if (_healthActual < 0)_healthActual = 0;
        _isAlive = _healthActual >= 0;

        characterUI.healthBar.
        DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration).
        OnComplete(Kill);

        ShowInfoText(totalDamage, GameData.Instance.textConfig.colorMsgDamage);
    }

    private void ShowInfoText(float value, Color color)
    {
        infoTextEvent.text = value.ToString("F0");
        infoTextEvent.position = _infoTextPosition;
        infoTextEvent.color = color;
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