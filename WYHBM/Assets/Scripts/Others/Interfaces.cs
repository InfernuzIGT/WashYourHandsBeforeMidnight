public interface ICombatable
{
    void ActionStartCombat();
    void ActionStopCombat();
}

public interface IHealeable<T>
{
    void ActionHeal(T amountHeal);
}

public interface IDamageable<T>
{
    void ActionReceiveDamage(T damageReceived);
}