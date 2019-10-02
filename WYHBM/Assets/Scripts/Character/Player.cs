using UnityEngine;

public class Player : Character
{

    public override void ActionReceiveDamage(float damageReceived)
    {
        base.ActionReceiveDamage(damageReceived);
       
    }

    public override void ActionHeal(float amountHeal)
    {
        base.ActionHeal(amountHeal);
    }
    
    

}