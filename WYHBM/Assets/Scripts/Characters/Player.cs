using UnityEngine;

public class Player : Character
{
    [Header("Custom")]
    public GameObject _enemy;
    public int healPotion;

    public override void ActionReceiveDamage(float damageReceived)
    {
        base.ActionReceiveDamage(damageReceived);
       
    }

    public override void ActionHeal(float amountHeal)
    {
        base.ActionHeal(amountHeal);
        
    }

}