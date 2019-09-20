using UnityEngine;

public class Character : MonoBehaviour
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

    public virtual void ActionAttack()
    {

    }

    public virtual void ActionReceiveDamage(float damageReceived)
    {
        if (healthActual == 0)
            return;

        healthActual -= damageReceived;

    }

}