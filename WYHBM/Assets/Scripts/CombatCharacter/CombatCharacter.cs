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

    private float _healthActual;
    private bool _isActionDone;

    private Vector3 _scaleNormal;
    private Vector2 _infoTextPosition;

    private InfoTextEvent infoTextEvent = new InfoTextEvent();

    #region Properties

    private List<ItemSO> _items;
    public List<ItemSO> Items { get { return _items; } set { _items = value; } }

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

    private int _combatIndex;
    public int CombatIndex { get { return _combatIndex; } set { _combatIndex = value; } }

    private int _statsHealthMax;
    public int StatsHealthMax { get { return _statsHealthMax; } }

    private int _statsDamage;
    public int StatsDamage { get { return _statsDamage; } }

    private int _statsDefense;
    public int StatsDefense { get { return _statsDefense; } }

    private int _statsReaction;
    public int StatsReaction { get { return _statsReaction; } }

    #endregion

    public virtual void Awake()
    {
        // _boxCollider = GetComponent<BoxCollider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SetCharacter();
    }

    public virtual void Start()
    {
        _items = new List<ItemSO>();

        _scaleNormal = transform.localScale;
        _startPosition = transform.position;
        _infoTextPosition = new Vector2(transform.position.x, GameData.Instance.combatConfig.positionYTextStart);

        _healthActual = _statsHealthMax;
        characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration);
    }

    public virtual void SetCharacter()
    {

        _isAlive = true;
        _isMyTurn = false;
        _name = character.name;

        _statsHealthMax = character.statsHealthMax;
        _statsDamage = character.statsDamage;
        _statsReaction = character.statsReaction;

        Debug.Log($"<b> SET: {gameObject.name} - REACT: {_statsReaction} </b>");

        // TODO Mariano: Add ITEMS
        // _items?.AddRange(character.equipmentItem);
    }

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

    #region Turn System

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
    }

    public void StartGettingAhead()
    {
        StartCoroutine(GettingAhead());
    }

    private IEnumerator GettingAhead()
    {
        float elapsedTime = 0;

        Debug.Log($"<b> Character: {gameObject.name} - Reaction: {_statsReaction} </b>");

        float timeThreshold = (10 / _statsReaction) * GameData.Instance.combatConfig.actionTimeThresholdMultiplier;

        while (elapsedTime <= timeThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.combatManager.CharacterIsReadyToGoAhead(this);
    }

    #endregion 

    public void Select(COMBAT_STATE combatState, CombatCharacter currentCharacter)
    {
        Debug.Log($"<b> HIT </b>");

        switch (combatState)
        {
            case COMBAT_STATE.Attack:
                ActionReceiveDamage(currentCharacter.StatsDamage);
                break;

            case COMBAT_STATE.Item:
                // TODO Mariano: Damage with items
                break;

            case COMBAT_STATE.Defense:
                // TODO Mariano: Add defense per 1 turn
                break;

            default:
                break;
        }

    }

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

    public virtual void StartCombat()
    {
        SetCharacter();
        // _statsDamage *= _statsStrength;
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