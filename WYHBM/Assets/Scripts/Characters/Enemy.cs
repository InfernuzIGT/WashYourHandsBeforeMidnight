using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : Character
{

    private void Start()
    {
        healthActual = healthMax;
    }

    public override void ActionReceiveDamage(float damageReceived)
    {
        base.ActionReceiveDamage(damageReceived);

        if (healthActual <= 0)
        {
            Debug.Log ($"<color=red> Dead: {name} </color>");
            healthActual = 0;
        }
    }

}