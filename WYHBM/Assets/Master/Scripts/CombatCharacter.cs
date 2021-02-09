using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;
using FMODUnity;

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
    [SerializeField] protected CombatCharacterSO _data = null;

    [Header("FMOD")]
    public StudioEventEmitter hurtSound;
    public StudioEventEmitter attackSound;
    public StudioEventEmitter dodgeSound;
    public StudioEventEmitter deathSound;
    public StudioEventEmitter itemSound;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] protected CombatConfig _combatConfig = null;
    [SerializeField, ConditionalHide] protected SpriteRenderer _spriteRenderer;
    [SerializeField, ConditionalHide] private HealthBar _healthBar;
    [SerializeField, ConditionalHide] private CombatAnimator _combatAnimator;
    [SerializeField, ConditionalHide] private GameObject _shadow;

    // Protected
    protected bool _isActionDone;
    protected bool _isAlive = true;
    protected Material _material;
    protected WaitForSeconds _waitPerAction;

    // private Vector2 _infoTextPosition;
    private bool _inDefense;

    // Events
    // private InfoTextEvent infoTextEvent;
    private CombatCharacterGoAheadEvent _combatCharacterGoAheadEvent;
    protected ShakeEvent _shakeEvent;
    protected VibrationEvent _vibrationEvent;
    protected CombatRemoveCharacterEvent _combatRemoveCharacterEvent;

    private Coroutine _coroutineGettingAhead;

    // Shader
    private int hash_IsDamaged = Shader.PropertyToID("_IsDamaged");
    private int hash_IsHealing = Shader.PropertyToID("_IsHealing");
    private int hash_IsKilled = Shader.PropertyToID("_IsKilled");
    private int hash_Lerp = Shader.PropertyToID("_Lerp");

    // Combat variables
    private float _healthActual;
    private int _totalValue;
    private int _totalDamage;
    private int _totalDefense;
    private ItemSO _itemAttack;
    private ItemSO _itemDefense;
    private ItemSO _itemHeal;

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

    public CombatCharacterSO Data { get { return _data; } }

    public virtual void Start()
    {
        _material = _spriteRenderer.material;

        // infoTextEvent = new InfoTextEvent();
        _shakeEvent = new ShakeEvent();
        _combatRemoveCharacterEvent = new CombatRemoveCharacterEvent();

        _combatCharacterGoAheadEvent = new CombatCharacterGoAheadEvent();
        _combatCharacterGoAheadEvent.character = this;

        _vibrationEvent = new VibrationEvent();

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
        _healthActual = _data.StatsHealthStart;

        // _equipment.AddRange(inventoryCombat);

        _healthBar.UpdateBar(false, _healthActual / _data.StatsHealthMax);
    }

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
        if (_healthActual == 0) return;

        if (!GetProbability())
        {
            AnimationAction(ANIM_STATE.Dodge);
            dodgeSound.Play();
            return;
        }

        _totalDamage = GetItemDamage();

        if (_inDefense)
        {

            _inDefense = false;

            _totalDefense = GetItemDefense();
            if (_totalDefense > _totalDamage) _totalDefense = _totalDamage;

            AnimationAction(ANIM_STATE.Idle);
        }
        else
        {
            MaterialDamage();


            _totalDefense = 0;

            EffectReceiveDamage();
        }

        _healthActual -= (_totalDamage - _totalDefense);


        if (_healthActual <= 0)
        {
            _healthActual = 0;
            _isAlive = false;
            RemoveCharacter();

            AnimationAction(ANIM_STATE.Dead);

            deathSound.Play();

            _healthBar.UpdateBar(true, _healthActual / _data.StatsHealthMax);
            Kill();
        }
        else
        {
            AnimationAction(ANIM_STATE.Hit);

            hurtSound.Play();

            _healthBar.UpdateBar(true, _healthActual / _data.StatsHealthMax, Kill);
        }

        // ShowInfoText(totalDamage, _textConfig.colorMsgDamage);
    }

    public virtual void EffectReceiveDamage() { }

    private void ActionHeal()
    {
        MaterialHeal();

        AnimationAction(ANIM_STATE.Idle);

        _healthActual += GetItemHeal();

        // ShowInfoText(amountHeal, _textConfig.colorMsgHeal);

        if (_healthActual > _data.StatsHealthMax) _healthActual = _data.StatsHealthMax;

        _healthBar.UpdateBar(false, _healthActual / _data.StatsHealthMax);
    }

    #endregion

    #region Animation

    public void AnimationAction(ANIM_STATE combatState)
    {
        _combatAnimator.Action(combatState);

        if (combatState == ANIM_STATE.Attack_1 ||
            combatState == ANIM_STATE.Attack_2 ||
            combatState == ANIM_STATE.Item)
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

        if (_isAlive) AnimationAction(ANIM_STATE.Idle);
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

    private void Kill()
    {
        if (!_isAlive)
        {
            _healthBar.Kill();
            _shadow.gameObject.SetActive(false);
            MaterialKill();
        }
    }

    public void RemoveCharacter()
    {
        EventController.TriggerEvent(_combatRemoveCharacterEvent);
    }

    public int GetItemDamage()
    {
        // if (_itemAttack != null)
        // {
        //     _totalValue = Random.Range(_itemAttack.value.x, _itemAttack.value.y);
        // }
        // else
        // {
        //     _totalValue = _data.StatsBaseDamage;
        // }


        _totalValue = _data.StatsBaseDamage;

        return _totalValue;
    }

    public int GetItemDefense()
    {
        // if (_itemDefense != null)
        // {
        //     _totalValue = Random.Range(_itemDefense.value.x, _itemDefense.value.y);
        // }
        // else
        // {
        //     _totalValue = _data.StatsBaseDefense;
        // }

        _totalValue = _data.StatsBaseDefense;

        return _totalValue;
    }

    public int GetItemHeal()
    {
        if (_itemHeal != null)
        {
            _totalValue = Random.Range(_itemHeal.value.x, _itemHeal.value.y);
        }
        else
        {
            // TODO Mariano: Cambiar por Stat Heal
            _totalValue = _data.StatsBaseDefense;
        }

        return _totalValue;
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

        _material.SetFloat(hash_Lerp, show ? 1 : 0);
    }

    protected void MaterialDamage()
    {
        _material.SetFloat(hash_Lerp, 1);
        _material.SetFloat(hash_IsDamaged, 1);
        _material.SetFloat(hash_IsHealing, 0);

        _material
            .DOFloat(0, hash_Lerp, _combatConfig.waitTimePerAction)
            .SetEase(Ease.InBack);


    }

    protected void MaterialHeal()
    {
        _material.SetFloat(hash_Lerp, 1);
        _material.SetFloat(hash_IsDamaged, 0);
        _material.SetFloat(hash_IsHealing, 1);

        _material
            .DOFloat(0, hash_Lerp, _combatConfig.waitTimePerAction)
            .SetEase(Ease.InBack);
    }

    private void MaterialKill()
    {
        _material.SetFloat(hash_Lerp, 0);
        _material.SetFloat(hash_IsDamaged, 0);
        _material.SetFloat(hash_IsHealing, 0);

        _material
            .DOFloat(0, hash_IsKilled, _combatConfig.waitTimePerAction * 2)
            .SetEase(Ease.OutBack)
            .OnComplete(() => gameObject.SetActive(false));
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

        attackSound.Play();

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

        float timeThreshold = (10 / _data.StatsReaction) * _combatConfig.actionTimeThresholdMultiplier;

        while (elapsedTime <= timeThreshold)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        EventController.TriggerEvent(_combatCharacterGoAheadEvent);
        // GameManager.Instance.combatManager.CharacterIsReadyToGoAhead(this);
    }

    #endregion 

#if UNITY_EDITOR

    public void SetData()
    {
        GetComponent<SpriteRenderer>().sprite = _data.Sprite;
        GetComponent<Animator>().runtimeAnimatorController = _data.AnimatorController;

        // TODO Mariano: Add rest of values

        Debug.Log($"<color=green><b>[Combat {_data.Name}]</b></color> Data loaded successfully!");
    }

#endif

}