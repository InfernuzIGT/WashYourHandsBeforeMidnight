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
    [SerializeField, Range(0f, 20f)] private int _statsBaseDamage = 100;
    [SerializeField, Range(1f, 10f)] private int _statsBaseDefense = 5;
    [SerializeField, Range(1f, 10f)] private int _statsReaction = 1;

    [Header("Inventory")]
    public ItemSO weapon;
    public ItemSO item;
    public ItemSO defense;

    // Protected
    protected bool _isActionDone;
    protected WaitForSeconds _waitPerAction;

    private CharacterUI characterUI;
    private CombatAnimator _combatAnimator;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _scaleNormal;
    private Vector2 _infoTextPosition;
    private int _combatIndex;

    private InfoTextEvent infoTextEvent = new InfoTextEvent();

    // Stats
    private float _healthActual;

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

    public virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _combatAnimator = GetComponent<CombatAnimator>();

        _waitPerAction = new WaitForSeconds(GameData.Instance.combatConfig.waitTimePerAction);

        _scaleNormal = transform.localScale;
        _startPosition = transform.position;
        _infoTextPosition = new Vector2(transform.position.x, GameData.Instance.combatConfig.positionYTextStart);
    }

    public void SetCharacter(int index)
    {
        _isMyTurn = false;

        _combatIndex = index;
        _healthActual = _statsHealthMax;

        Vector3 healthBarPos = new Vector3(
            transform.position.x,
            transform.position.y + GameData.Instance.combatConfig.offsetHealthBar,
            transform.position.z);

        characterUI = Instantiate(GameData.Instance.gameConfig.characterUIPrefab, healthBarPos, Quaternion.identity, this.transform);
        characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.startFillDuration);
    }

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

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

        // Debug.Log($"<b> Character: {gameObject.name} - Reaction: {_statsReaction} </b>");

        float timeThreshold = (10 / _statsReaction) * GameData.Instance.combatConfig.actionTimeThresholdMultiplier;

        while (elapsedTime <= timeThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.combatManager.CharacterIsReadyToGoAhead(this);
    }

    public void Select(COMBAT_STATE combatState, CombatCharacter currentCharacter)
    {
        switch (combatState)
        {
            case COMBAT_STATE.Attack:
                ActionReceiveDamage(currentCharacter.GetDamage());
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

        currentCharacter.AnimationAction(combatState);
        currentCharacter.DoAction();
    }

    #endregion 

    //------------------------------------------------------------------
    //------------------------------------------------------------------
    //------------------------------------------------------------------

    public virtual void ActionStartCombat()
    {
        transform.DOScale(GameData.Instance.combatConfig.scaleCombat, GameData.Instance.combatConfig.transitionDuration);
    }

    public virtual void ActionStopCombat()
    {
        transform.DOScale(_scaleNormal, GameData.Instance.combatConfig.transitionDuration);

        // _defense = 0; // TODO Mariano: Use the default Defense
    }

    public virtual void ActionHeal(int amountHeal)
    {
        _healthActual += amountHeal;

        // ShowInfoText(amountHeal, GameData.Instance.textConfig.colorMsgHeal);

        if (_healthActual > _statsHealthMax)_healthActual = _statsHealthMax;

        characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration);
    }

    public virtual void ActionReceiveDamage(int damageReceived)
    {
        if (_healthActual == 0)
            return;

        int totalDamage = damageReceived - _statsBaseDefense;

        _healthActual -= totalDamage;
        if (_healthActual < 0)_healthActual = 0;

        characterUI.healthBar.
        DOFillAmount(_healthActual / _statsHealthMax, GameData.Instance.combatConfig.fillDuration).
        OnComplete(Kill);

        // ShowInfoText(totalDamage, GameData.Instance.textConfig.colorMsgDamage);
    }

    public void AnimationAction(COMBAT_STATE combatState)
    {
        _combatAnimator.Action(combatState);
    }

    // TODO Mariano: Review
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

}