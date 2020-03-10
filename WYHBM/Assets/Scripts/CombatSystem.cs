using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatStates
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

public class CombatSystem : MonoBehaviour
{
    public CombatStates state;
    public UnitRefactor unitRefactor;


    void Start()
    {
        state = CombatStates.START;
        StartCombat();
    }

    public void EndCombat()
    {
        if (state == CombatStates.WON)
        {
            Debug.Log ($"<b> You won the prototype combat! </b>");
        }
        else if (state == CombatStates.LOST)
        {
            Debug.Log ($"<b> You lost the prototype combat! </b>");
        }
        
    }

    public IEnumerator StartCombat()
    {
        Debug.Log ($"<b> La batalla acaba de comenzar </b>");

        yield return new WaitForSeconds(2f);

        state = CombatStates.PLAYERTURN;
    }

    public IEnumerator EnemyTurn ()
    {
        bool isDead = unitRefactor.TakeDamage(unitRefactor.damageBase);

        yield return new WaitForSeconds (2f);

        if (isDead)
        {
            state = CombatStates.LOST;
            EndCombat();
        }
        else
        {
            state = CombatStates.PLAYERTURN;
        }
    }

    public IEnumerator PlayerAttack ()
    {
        bool isDead = unitRefactor.TakeDamage(unitRefactor.damageBase);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = CombatStates.WON;
            EndCombat();
        }
        else
        {
            state = CombatStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnAttackButton()
    {
        if (state != CombatStates.PLAYERTURN)
            return;
    
        StartCoroutine(PlayerAttack());
    }
}
