using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour, IAttackable, IHealeable<float>, IDamageable<float>
{
    [Header("General")]
    public new string name;
    public CHARACTER_TYPE characterType = CHARACTER_TYPE.none;
    public CHARACTER_BEHAVIOUR characterBehaviour = CHARACTER_BEHAVIOUR.none;

    [Header("Stats")]
    public float healthMax;
    public float healthActual;
    [Space]
    public float damage;
    public float defense;

    private BoxCollider2D _boxCollider;
    public BoxCollider2D BoxCollider { get { return _boxCollider; } }

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        healthActual = healthMax;
    }

    public virtual void ActionAttack()
    {

    }

    public virtual void ActionHeal(float amountHeal)
    {
        healthActual += amountHeal;
    }

    public virtual void ActionReceiveDamage(float damageReceived)
    {
        if (healthActual == 0)
            return;

        healthActual -= damageReceived;

        if (healthActual <= 0)
            healthActual = 0;
    }

}