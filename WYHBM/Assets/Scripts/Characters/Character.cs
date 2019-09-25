using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour, IAttackable, IHealeable<float>, IDamageable<float>
{
    [Header("General")]
    public new string name;
    public CHARACTER_TYPE characterType = CHARACTER_TYPE.none;
    public CHARACTER_BEHAVIOUR characterBehaviour = CHARACTER_BEHAVIOUR.none;

    [Header("Stats")]
    public float healthMax;
    public float damage;
    public float defense;

    [Header("Interface")]
    public Image healthBar;

    [Header("Equipment")]
    public List<WeaponSO> equipmentWeapon;
    [Space]
    public List<ItemSO> equipmentItem;
    [Space]
    public List<ArmorSO> equipmentArmor;
    
    private float healthActual;

    private BoxCollider2D _boxCollider;
    public BoxCollider2D BoxCollider { get { return _boxCollider; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        healthActual = healthMax;
        healthBar.fillAmount = healthActual / healthMax;
    }

    public virtual void ActionAttack()
    {

    }

    public virtual void ActionHeal(float amountHeal)
    {
        healthActual += amountHeal;
        healthBar.fillAmount = healthActual / healthMax;
    }

    public virtual void ActionReceiveDamage(float damageReceived)
    {
        if (healthActual == 0)
            return;

        healthActual -= damageReceived;
        healthBar.fillAmount = healthActual / healthMax;

        if (healthActual <= 0)
        {
            healthActual = 0;
            healthBar.fillAmount = 0;
        }
    }

}