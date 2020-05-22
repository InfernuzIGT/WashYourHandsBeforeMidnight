using System.Collections;
using DG.Tweening;
using Events;
using UnityEngine;

public class CombatCharacter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string _name;
    [Space]
    [SerializeField, Range(0f, 100f)] private int _statsHealthMax = 100;
    [SerializeField, Range(0f, 20f)] private int _statsBaseDamage = 10;
    [SerializeField, Range(1f, 10f)] private int _statsBaseDefense = 5;
    [SerializeField, Range(1f, 10f)] private int _statsReaction = 1;

    [Header("Inventory")]
    public ItemSO weapon;
    public ItemSO item;
    public ItemSO defense;

    // Protected
    protected bool _isActionDone;
    protected WaitForSeconds _waitPerAction;

    private CharacterUI _characterUI;
    private CombatAnimator _combatAnimator;
    private SpriteRenderer _spriteRenderer;
    // private Vector3 _scaleNormal;
    private Vector2 _infoTextPosition;
    private bool _inDefense;

    private InfoTextEvent infoTextEvent = new InfoTextEvent();

    // Combat variables
    private float _healthActual;
    private int _totalValue;
    private int _totalDamage;
    private int _totalDefense;
    private int _combatIndex;

    // Combat Properties
    public string Name { get { return _name; } }
    public int StatsHealthMax { get { return _statsHealthMax; } }
    public int StatsBaseDamage { get { return _statsBaseDamage; } }
    public int StatsBaseDefense { get { return _statsBaseDefense; } }
    public int StatsReaction { get { return _statsReaction; } }

    // Properties
    protected bool _isMyTurn;
    public bool IsMyTurn { get { return _isMyTurn; } set { _isMyTurn = value; } }

    private Vector3 _startPosition;
    public Vector3 StartPosition { get { return _startPosition; } }

    private float _startPositionX;
    public float StartPositionX { get { return _startPositionX; } }

    public virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _combatAnimator = GetComponent<CombatAnimator>();

        _waitPerAction = new WaitForSeconds(GameData.Instance.combatConfig.waitTimePerAction);

        _infoTextPosition = new Vector2(transform.position.x, GameData.Instance.combatConfig.positionYTextStart);
    }

    public void SetCharacter(int index)
    {
        // _scaleNormal = transform.localScale;
        _startPosition = transform.position;
        _startPositionX = transform.position.x;

        _isMyTurn = false;

        _combatIndex = index;
        _healthActual = _statsHealthMax;

        Vector3 healthBarPos = new Vector3(
            transform.position.x,
            transform.position.y + GameData.Instance.combatConfig.offsetHealthBar,
            transform.position.z);

        _characterUI = Instantiate(GameData.Instance.gameConfig.characterUIPrefab, healthBarPos, Quaternion.identity, this.transform);
        _characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.startFillDuration);
    }

    #region Actions

    public void Select(COMBAT_STATE combatState, CombatCharacter currentCharacter)
    {
        switch (combatState)
        {
            case COMBAT_STATE.Attack:
                ActionReceiveDamage(currentCharacter.GetDamage());
                break;

            case COMBAT_STATE.Item:
                ActionUseItem(currentCharacter);
                break;

            case COMBAT_STATE.Defense:
                _inDefense = true;
                break;

            default:
                break;
        }

        currentCharacter.AnimationAction(combatState);
        currentCharacter.DoAction();

        AnimationRecovery();
    }

    public virtual void ActionUseItem(CombatCharacter currentCharacter)
    {
        _totalValue = Random.Range(currentCharacter.item.valueMin, currentCharacter.item.valueMax);

        switch (currentCharacter.item.type)
        {
            case ITEM_TYPE.Damage:
                ActionReceiveDamage(_totalValue);
                break;

            case ITEM_TYPE.Heal:
                ActionHeal(_totalValue);
                break;

            default:
                break;
        }
    }

    public virtual void ActionReceiveDamage(int damageReceived)
    {
        if (_healthActual == 0)
            return;

        if (_inDefense)
        {
            _inDefense = false;

            _totalDefense = GetDefense();

            AnimationAction(COMBAT_STATE.Defense);
        }
        else
        {
            _totalDefense = 0;

            AnimationAction(COMBAT_STATE.Hit);
        }

        _totalDamage = damageReceived - _totalDefense;

        _healthActual -= _totalDamage;
        if (_healthActual < 0)_healthActual = 0;

        _characterUI.healthBar.
        DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration).
        OnComplete(Kill);

        // ShowInfoText(totalDamage, GameData.Instance.textConfig.colorMsgDamage);
    }

    private void ActionHeal(int amountHeal)
    {
        AnimationAction(COMBAT_STATE.Item);

        _healthActual += amountHeal;

        // ShowInfoText(amountHeal, GameData.Instance.textConfig.colorMsgHeal);

        if (_healthActual > _statsHealthMax)_healthActual = _statsHealthMax;

        _characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration);
    }

    #endregion

    #region Animation

    public void AnimationAction(COMBAT_STATE combatState)
    {
        _combatAnimator.Action(combatState);

        if (combatState == COMBAT_STATE.Attack || combatState == COMBAT_STATE.Item)
        {
            AnimationActionStart();
        }
        else
        {
            AnimationActionEnd();
        }
    }

    public virtual void AnimationActionStart()
    {
        // transform.DOScale(GameData.Instance.combatConfig.scaleCombat, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void AnimationActionEnd()
    {
        // transform.DOScale(_scaleNormal, GameData.Instance.combatConfig.transitionDuration);
    }

    public void AnimationRecovery()
    {
        StartCoroutine(Recovery());
    }

    private IEnumerator Recovery()
    {
        yield return _waitPerAction;
        AnimationAction(COMBAT_STATE.Idle);
    }

    // TODO Mariano: Review
    private void ShowInfoText(float value, Color color)
    {
        infoTextEvent.text = value.ToString("F0");
        infoTextEvent.position = _infoTextPosition;
        infoTextEvent.color = color;
        EventController.TriggerEvent(infoTextEvent);
    }

    #endregion

    private void Kill()
    {
        if (_healthActual <= 0)
        {
            _healthActual = 0;

            _characterUI.Kill();

            _spriteRenderer.
            DOFade(0, GameData.Instance.combatConfig.canvasFadeDuration).
            SetEase(Ease.OutQuad).OnComplete(CheckCharacters);
        }
    }

    public virtual void CheckCharacters()
    {
        gameObject.SetActive(false);
    }

    public int GetDamage()
    {
        return weapon ? Random.Range(weapon.valueMin, weapon.valueMax) : _statsBaseDamage;
    }

    public int GetDefense()
    {
        return defense ? Random.Range(defense.valueMin, defense.valueMax) : _statsBaseDefense;
    }

    #region Turn System

    /// <summary>
    /// Ejecuta la accion
    /// </summary>
    public void DoAction()
    {
        if (_isMyTurn)
        {
            _isActionDone = true;
            _isMyTurn = false;
        }
    }

    /// <summary>
    /// Comienza a esperar la accion
    /// </summary>
    public Coroutine StartWaitingForAction()
    {
        return StartCoroutine(WaitingForAction());
    }

    /// <summary>
    /// Espera la accion
    /// </summary>
    public virtual IEnumerator WaitingForAction()
    {
        yield return null;
    }

    public void StartGettingAhead()
    {
        StartCoroutine(GettingAhead());
    }

    /// <summary>
    /// Luego de un lapso de tiempo, se mueve hacia la punta de la lista de Characters
    /// </summary>
    private IEnumerator GettingAhead()
    {
        float elapsedTime = 0;

        float timeThreshold = (10 / _statsReaction) * GameData.Instance.combatConfig.actionTimeThresholdMultiplier;

        while (elapsedTime <= timeThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.combatManager.CharacterIsReadyToGoAhead(this);
    }

    #endregion 

}