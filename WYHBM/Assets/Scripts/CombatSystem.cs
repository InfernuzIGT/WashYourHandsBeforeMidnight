using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class CombatSystem : MonoBehaviour
{
    public CombatState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform  playerStation;
    public Transform  playerStation1;
    public Transform  enemyStation;
    public Transform  enemyStation1;

    public Unit playerUnit;
    public Unit enemyUnit;

    void Start()
    {
        playerUnit.Stats();
        state = CombatState.START;
        
        StartCoroutine(SetupBattle());
    }
    private IEnumerator SetupBattle()
    {
        // Instantiate(playerPrefab, playerStation);

        // Instantiate(enemyPrefab, enemyStation);
        Debug.Log ($"<b> The combat is just started..  </b>");

        yield return new WaitForSeconds(2f);

        state = CombatState.PLAYERTURN;
        PlayerTurn();
    }
    public void EndCombat()
    {
        if (state == CombatState.WON)
        {
            Debug.Log ($"<b> You won the combat </b>");
            
        }
        else if (state == CombatState.LOST)
        {
            Debug.Log ($"<b> You lost the combat </b>");
        }
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damageMelee);

        Debug.Log ($"<b> Enemy Health: </b>" + enemyUnit.currentHP);

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = CombatState.WON;
            EndCombat();
        }
        else
        {
            state = CombatState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyTurn()
    {
        Debug.Log ($"<b> Enemy turn! </b>");

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damageMelee);

        Debug.Log ($"<b> Health player: </b>" + playerUnit.currentHP);
        
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = CombatState.LOST;
            EndCombat();
        }
        else 
        {
            state = CombatState.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void PlayerTurn()
    {
        Debug.Log ($"<b> Choose an action... </b>");
        
    }
    public void OnAttackButton()
    {
        if(state != CombatState.PLAYERTURN)
        return;

        StartCoroutine(PlayerAttack());
        
    }
}
