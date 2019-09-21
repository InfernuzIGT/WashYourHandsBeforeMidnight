public interface IAttackable
{
    void ActionAttack();
}

public interface IHealeable<T>
{
    void ActionHeal(T amountHeal);
}

public interface IDamageable<T>
{
    void ActionReceiveDamage(T damageReceived);
}