using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


    private float _playerSpeed;
    private float _enemySpeed;

    public GameObject Character1, Character2, Character3;
    private List<GameObject> characters = new List<GameObject>();
    private Queue<int> _turner  = new Queue<int>();

    [Header ("GameObjects")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;

    [Header ("Scripts")]
    public Unit playerUnit;
    public Unit enemyUnit;
    
    [Header("Materials")]
    public Material red;
    public Material blue;
    public Material playerColor;
    public Material enemyColor;

    private bool _turnedPass;
    private int _characterIndex;

    void Awake()
    {
        AddCharactersToList();

        playerUnit = playerPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();

        playerUnit.Stats();
    }

    void Start()
    {
        QueueTurner();
        state = CombatState.START;
        
    }   

    public void QueueTurner()
    {
        characters = characters.OrderBy(GameObject => characters).ToList();

        // Recorre el componente Unit en toda la lista de personajes
        foreach (var unit in characters)
        {
            // Se mete en cola el GameObject con mas agility(stat en el componente unit)
            if (playerUnit.agility > enemyUnit.agility )
            {
                _turner.Enqueue(1);
                _turner.Enqueue(2);
                _turner.Enqueue(3);
            }
            else
            {
                _turner.Enqueue(3);
                _turner.Enqueue(2);
                _turner.Enqueue(1);

            }
        }

        DequeueTurner();

    }

    /*Devuelve al jugador que primero fue insertado en la cola, es decir, el que tiene mas velocidad*/
    public void DequeueTurner()
    {
        _turner.Dequeue();
    }

    private void QueueAux()
    {
        if (_turnedPass)
        {
            _turner.Dequeue();
        }
    }

    /**/
    private void AddCharactersToList()
    {
        characters.Add(playerPrefab);
        characters.Add(enemyPrefab);
        characters.Add(enemyPrefab2);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(PlayerAttack());
        }

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

    /*Da comienzo al combate*/
    private IEnumerator SetupBattle()
    {
        Debug.Log ($"<b> The combat is just started..  </b>");

        yield return new WaitForSeconds(2f);

        state = CombatState.PLAYERTURN;
        PlayerTurn();
    }

    /*Ejecuta la accion del jugador atacando*/
    IEnumerator PlayerAttack()
    {
        enemyUnit.TakeDamage(playerUnit);
        
        enemyPrefab.GetComponent<MeshRenderer>().material = red;

        Debug.Log ($"<b> Enemy Health: </b>" + enemyUnit.currentHP);

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        enemyPrefab.GetComponent<MeshRenderer>().material = enemyColor;

        if (enemyUnit.isAlive)
        {
            state = CombatState.ENEMYTURN;
            // menu.SetActive(false);
            StartCoroutine(EnemyTurn());
        }
        else
        {
            state = CombatState.WON;
            // menu.SetActive(false);
            EndCombat();
        }
    }

    /**/
    IEnumerator PlayerDefense()
    {
        playerPrefab.GetComponent<MeshRenderer>().material = blue;

        playerUnit.Defense();

        Debug.Log ($"<b> Defense increase. </b>");

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        playerPrefab.GetComponent<MeshRenderer>().material = playerColor;

        state = CombatState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    /**/
    IEnumerator PlayerItem()
    {
        // Aplicar distintivo de player usando item

        playerUnit.UseItem();

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    /**/
    IEnumerator PlayerEscape()
    {
        // Mover al personaje en direccion a la salida

        playerUnit.Escape();

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        EndCombat();
    }
    
    /*Ejecuta la corrutina del enemigo atacando*/
    IEnumerator EnemyTurn()
    {
        Debug.Log ($"<b> Enemy turn! </b>");

        yield return new WaitForSeconds(2f);

        playerUnit.TakeDamage(enemyUnit);

        playerPrefab.GetComponent<MeshRenderer>().material = red;

        Debug.Log ($"<b> Health player: </b>" + playerUnit.currentHP);
        
        yield return new WaitForSeconds(1f);
        
        playerPrefab.GetComponent<MeshRenderer>().material = playerColor;

        if (playerUnit.isAlive)
        {
            state = CombatState.PLAYERTURN;
            // menu.SetActive(true);
            PlayerTurn();
        }
        else 
        {
            state = CombatState.LOST;
            // menu.SetActive(false);
            EndCombat();
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