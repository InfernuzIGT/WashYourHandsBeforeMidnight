using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;

[System.Serializable]
public class Equipment
{
    public ItemSO actionA;
    public ItemSO actionB;
    public List<ItemSO> actionItem = new List<ItemSO>();
}

public class CombatCharacter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] protected CombatConfig _combatConfig = null;
    [SerializeField] private string _name = null;
    [SerializeField] private Equipment _equipment = new Equipment();

    [Header("Sprites")]
    [SerializeField] private Sprite _previewSprite = null;
    [SerializeField] private Sprite _turnSprite = null;

    [Header("Stats")]
    [SerializeField, Range(0f, 100f)] private int _statsHealthMax = 100;
    [SerializeField, Range(0f, 100f)] private int _statsHealthStart = 100;
    [SerializeField, Range(0f, 20f)] private int _statsBaseDamage = 10;
    [SerializeField, Range(1f, 10f)] private int _statsBaseDefense = 5;
    [SerializeField, Range(1f, 10f)] private int _statsReaction = 1;

    // Protected
    protected SpriteRenderer _spriteRenderer;
    protected bool _isActionDone;
    protected bool _isAlive = true;
    protected Material _material;
    protected WaitForSeconds _waitPerAction;

    private CharacterUI _characterUI;
    private CombatAnimator _combatAnimator;
    // private Vector2 _infoTextPosition;
    private bool _inDefense;
    private float _varShader;
    private float _matGlowSpeed = 2.5f;

    // Events
    // private InfoTextEvent infoTextEvent;
    private ShakeEvent _shakeEvent;
    private CombatCheckGameEvent _combatCheckGameEvent;
    private CombatCharacterGoAheadEvent _combatCharacterGoAheadEvent;
    protected CombatRemoveCharacterEvent _combatRemoveCharacterEvent;

    private Coroutine _coroutineGettingAhead;

    // Shader
    private int hash_IsDamaged = Shader.PropertyToID("_IsDamaged");
    private int hash_IsHealing = Shader.PropertyToID("_IsHealing");
    private int hash_Glow = Shader.PropertyToID("_Glow");

    // Combat variables
    private float _healthActual;
    private int _totalValue;
    private int _totalDamage;
    private int _totalDefense;
    private ItemSO _itemAttack;
    private ItemSO _itemDefense;
    private ItemSO _itemHeal;

    // Combat Properties
    public string Name { get { return _name; } }
    public Equipment Equipment { get { return _equipment; } }
    public Sprite PreviewSprite { get { return _previewSprite; } }
    public Sprite TurnSprite { get { return _turnSprite; } }
    public int StatsHealthMax { get { return _statsHealthMax; } }
    public int StatsBaseDamage { get { return _statsBaseDamage; } }
    public int StatsBaseDefense { get { return _statsBaseDefense; } }
    public int StatsReaction { get { return _statsReaction; } }

    // Properties
    protected int _combatIndex;
    public int CombatIndex { get { return _combatIndex; } }

    protected bool _isMyTurn;
    public bool IsMyTurn { get { return _isMyTurn; } set { _isMyTurn = value; } }

    private Vector3 _startPosition;
    public Vector3 StartPosition { get { return _startPosition; } }

    private float _startPositionX;
    public float StartPositionX { get { return _startPositionX; } }

    public bool CanHighlight { get { return _canHighlight; } set { _canHighlight = value; } }
    protected bool _canHighlight;

    public virtual void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _combatAnimator = GetComponent<CombatAnimator>();
        _material = _spriteRenderer.material;

        // infoTextEvent = new InfoTextEvent();
        _shakeEvent = new ShakeEvent();
        _combatRemoveCharacterEvent = new CombatRemoveCharacterEvent();
        _combatCheckGameEvent = new CombatCheckGameEvent();

        _combatCharacterGoAheadEvent = new CombatCharacterGoAheadEvent();
        _combatCharacterGoAheadEvent.character = this;

        _waitPerAction = new WaitForSeconds(_combatConfig.waitTimePerAction);

        // _infoTextPosition = new Vector2(transform.position.x, _combatConfig.positionYTextStart);
    }

    public void SetCharacter(int index /* , List<ItemSO> inventoryCombat */ )
    {
        // _scaleNormal = transform.localScale;
        _startPosition = transform.position;
        _startPositionX = transform.position.x;

        _isMyTurn = false;

        _combatIndex = index;
        _healthActual = _statsHealthStart;

        // _equipment.AddRange(inventoryCombat);

        Vector3 healthBarPos = new Vector3(
            transform.position.x,
            transform.position.y + _combatConfig.offsetHealthBar,
            transform.position.z);

        _characterUI = Instantiate(_combatConfig.characterUIPrefab, healthBarPos, Quaternion.identity, this.transform);
        _characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, _combatConfig.startFillDuration);
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     if (_canHighlight && !_isMyTurn)
    //     {
    //         MaterialShow(true);
    //     }
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (_canHighlight && !_isMyTurn)
    //     {
    //         MaterialShow(false);
    //     }
    // }

    #region Actions

    public void Select(ItemSO item)
    {
        if (item == null)
        {
            _itemAttack = null;
            ActionReceiveDamage();
            AnimationRecovery();
            return;
        }

        switch (item.type)
        {
            case ITEM_TYPE.WeaponMelee:
            case ITEM_TYPE.WeaponOneHand:
            case ITEM_TYPE.WeaponTwoHands:
            case ITEM_TYPE.ItemGrenade:
                _itemAttack = item;
                ActionReceiveDamage();
                break;

            case ITEM_TYPE.ItemHeal:
                _itemHeal = item;
                ActionHeal();
                break;

            case ITEM_TYPE.ItemDefense:
                _itemDefense = item;
                _inDefense = true;
                break;
        }

        AnimationRecovery();
    }

    public void ActionReceiveDamage()
    {
        if (_healthActual == 0)return;

        if (!GetProbability())
        {
            AnimationAction(ANIM_STATE.Idle);
            Debug.Log($"<b> DODGE! </b>");
            return;
        }

        _totalDamage = GetItemDamage();

        if (_inDefense)
        {
            _inDefense = false;

            _totalDefense = GetItemDefense();
            if (_totalDefense > _totalDamage)_totalDefense = _totalDamage;

            AnimationAction(ANIM_STATE.Idle);
        }
        else
        {
            MaterialDamage();

            _totalDefense = 0;

            AnimationAction(ANIM_STATE.Hit);
        }

        _healthActual -= (_totalDamage - _totalDefense);

        if (_healthActual <= 0)
        {
            _healthActual = 0;
            _isAlive = false;
            RemoveCharacter();
        }

        _characterUI.healthBar.
        DOFillAmount(_healthActual / _statsHealthMax, _combatConfig.fillDuration).
        OnComplete(Kill);

        // ShowInfoText(totalDamage, _textConfig.colorMsgDamage);
    }

    private void ActionHeal()
    {
        MaterialHeal();

        AnimationAction(ANIM_STATE.Idle);

        _healthActual += GetItemHeal();

        // ShowInfoText(amountHeal, _textConfig.colorMsgHeal);

        if (_healthActual > _statsHealthMax)_healthActual = _statsHealthMax;

        _characterUI.healthBar.DOFillAmount(_healthActual / _statsHealthMax, _combatConfig.fillDuration);
    }

    #endregion

    #region Animation

    public void AnimationAction(ANIM_STATE combatState)
    {
        _combatAnimator.Action(combatState);

        if (combatState == ANIM_STATE.Idle ||
            combatState == ANIM_STATE.Hit ||
            combatState == ANIM_STATE.Death)
        {
            AnimationActionEnd();
        }
        else
        {
            AnimationActionStart();
        }
    }

    public virtual void AnimationActionStart()
    {
        // transform.DOScale(_combatConfig.scaleCombat, _combatConfig.transitionDuration);
    }

    public virtual void AnimationActionEnd()
    {
        // transform.DOScale(_scaleNormal, _combatConfig.transitionDuration);
    }

    public void AnimationRecovery()
    {
        StartCoroutine(Recovery());
    }

    private IEnumerator Recovery()
    {
        yield return _waitPerAction;
        AnimationAction(ANIM_STATE.Idle);
    }

    // // TODO Mariano: Review
    // private void ShowInfoText(float value, Color color)
    // {
    //     infoTextEvent.text = value.ToString("F0");
    //     infoTextEvent.position = _infoTextPosition;
    //     infoTextEvent.color = color;
    //     EventController.TriggerEvent(infoTextEvent);
    // }

    #endregion

    public void Shake()
    {
        EventController.TriggerEvent(_shakeEvent);
    }

    private void Kill()
    {
        if (!_isAlive)
        {
            _characterUI.Kill();

            _spriteRenderer.
            DOFade(0, _combatConfig.canvasFadeDuration).
            SetEase(Ease.OutQuad).OnComplete(CheckGame);
        }
    }

    public void CheckGame()
    {
        gameObject.SetActive(false);
        // GameManager.Instance.combatManager.CheckGame();
        EventController.TriggerEvent(_combatCheckGameEvent);
    }

    public void RemoveCharacter()
    {
        EventController.TriggerEvent(_combatRemoveCharacterEvent);
    }

    public int GetItemDamage()
    {
        if (_itemAttack != null)
        {
            _totalValue = Random.Range(_itemAttack.value.x, _itemAttack.value.y);
        }
        else
        {
            _totalValue = _statsBaseDamage;
        }

        return _totalValue;
    }

    public int GetItemDefense()
    {
        return _totalValue = Random.Range(_itemDefense.value.x, _itemDefense.value.y);
    }

    public int GetItemHeal()
    {
        return _totalValue = Random.Range(_itemHeal.value.x, _itemHeal.value.y);
    }

    public bool GetProbability()
    {
        ProportionValue<bool>[] tempProb = new ProportionValue<bool>[2];

        tempProb[0] = ProportionValue.Create(_itemAttack.probability, true);
        tempProb[1] = ProportionValue.Create(1 - _itemAttack.probability, false);

        return tempProb.ChooseByRandom();
    }

    #region Shader

    public void ShowUI(bool show)
    {
        MaterialShow(show);
    }

    protected void MaterialShow(bool show)
    {
        _material.SetFloat(hash_IsDamaged, 0);
        _material.SetFloat(hash_IsHealing, 0);
        _material.SetFloat(hash_Glow, show ? 1 : 0);
    }

    protected void MaterialDamage()
    {
        _material.SetFloat(hash_IsDamaged, 1);
        _material.SetFloat(hash_IsHealing, 0);
        StartCoroutine(AnimateGlow());
    }

    protected void MaterialHeal()
    {
        _material.SetFloat(hash_IsDamaged, 0);
        _material.SetFloat(hash_IsHealing, 1);
        StartCoroutine(AnimateGlow());
    }

    private IEnumerator AnimateGlow()
    {
        _varShader = 0;

        while (_varShader < 1)
        {
            _varShader += _matGlowSpeed * Time.deltaTime;
            _material.SetFloat(hash_Glow, _varShader);
            yield return null;
        }

        while (_varShader > 0)
        {
            _varShader -= _matGlowSpeed * Time.deltaTime;
            _material.SetFloat(hash_Glow, _varShader);
            yield return null;
        }

        yield return null;
    }

    #endregion

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
        _coroutineGettingAhead = StartCoroutine(GettingAhead());
    }

    public void StopGettingAhead()
    {
        StopCoroutine(_coroutineGettingAhead);
    }

    /// <summary>
    /// Luego de un lapso de tiempo, se mueve hacia la punta de la lista de Characters
    /// </summary>
    private IEnumerator GettingAhead()
    {
        float elapsedTime = 0;

        float timeThreshold = (10 / _statsReaction) * _combatConfig.actionTimeThresholdMultiplier;

        while (elapsedTime <= timeThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        EventController.TriggerEvent(_combatCharacterGoAheadEvent);

        // GameManager.Instance.combatManager.CharacterIsReadyToGoAhead(this);
    }

    #endregion 

}