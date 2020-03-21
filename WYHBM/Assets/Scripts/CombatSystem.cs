using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

[System.Serializable]
public class Ch
{
    public string name;
    public int speed;
}

public class CombatSystem : MonoBehaviour
{
    public CombatState state;
    public Queue turner;

    [Header ("GameObjects")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject menu;

    [Header ("Transform")]
    public Transform  playerStation;
    public Transform  playerStation1;
    public Transform  enemyStation;
    public Transform  enemyStation1;

    [Header ("Scripts")]
    public Unit playerUnit;
    public Unit enemyUnit;

    [Header("Materials")]
    public Material red;
    public Material blue;
    public Material playerColor;
    public Material enemyColor;

    void Start()
    {
        playerUnit.Stats();
        state = CombatState.START;
        
        StartCoroutine(SetupBattle());
        // turner = new Queue();
        // QueueTurner();
    }   

    // public void QueueTurner()
    // {
    //     turner.Enqueue(characters);

    //     if (characters.speed == 1)
    //     {
    //         turner.Dequeue();
    //     }
    // }

    void Update()
    {
        playerUnit.healthBar.value = playerUnit.currentHP;
        enemyUnit.healthBar.value = enemyUnit.currentHP;
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

    public void PlayerTurn()
    {
        Debug.Log ($"<b> Choose an action... </b>");
    }

    #region Enumerators
    private IEnumerator SetupBattle()
    {
        Debug.Log ($"<b> The combat is just started..  </b>");

        yield return new WaitForSeconds(2f);

        state = CombatState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damageMelee);
        
        enemyPrefab.GetComponent<MeshRenderer>().material = red;

        Debug.Log ($"<b> Enemy Health: </b>" + enemyUnit.currentHP);

        menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        enemyPrefab.GetComponent<MeshRenderer>().material = enemyColor;

        if (isDead)
        {
            state = CombatState.WON;
            menu.SetActive(false);
            EndCombat();
        }
        else
        {
            state = CombatState.ENEMYTURN;
            menu.SetActive(false);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerDefense()
    {
        playerPrefab.GetComponent<MeshRenderer>().material = blue;

        playerUnit.Defense();

        Debug.Log ($"<b> Defense increase. </b>");

        menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        playerPrefab.GetComponent<MeshRenderer>().material = playerColor;

        state = CombatState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerItem()
    {
        // Aplicar distintivo de player usando item

        playerUnit.UseItem();

        menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerEscape()
    {
        // Mover al personaje en direccion a la salida

        playerUnit.Escape();

        menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        EndCombat();
    }
    
    IEnumerator EnemyTurn()
    {
        Debug.Log ($"<b> Enemy turn! </b>");

        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damageMelee);

        playerPrefab.GetComponent<MeshRenderer>().material = red;

        Debug.Log ($"<b> Health player: </b>" + playerUnit.currentHP);
        
        yield return new WaitForSeconds(1f);
        
        playerPrefab.GetComponent<MeshRenderer>().material = playerColor;

        if (isDead)
        {
            state = CombatState.LOST;
            menu.SetActive(false);
            EndCombat();
        }
        else 
        {
            state = CombatState.PLAYERTURN;
            menu.SetActive(true);
            PlayerTurn();
        }
    }

    #endregion

    #region Buttons
    public void OnAttackButton()
    {
        if(state != CombatState.PLAYERTURN)
        return;

        StartCoroutine(PlayerAttack());
    }

    public void OnDefenseButton()
    {
        if(state != CombatState.PLAYERTURN)
        return;

        StartCoroutine(PlayerDefense());
    }
    
    public void OnItemButton()
    {
        if (state != CombatState.PLAYERTURN)
        return;   
        
        StartCoroutine(PlayerItem());
    }

    public void OnEscapeButton()
    {
        if (state != CombatState.PLAYERTURN)
        return;   
        
        StartCoroutine(PlayerEscape());
    } 
    #endregion
    
}