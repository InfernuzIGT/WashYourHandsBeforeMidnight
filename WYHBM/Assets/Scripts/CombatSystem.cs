using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CombatState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

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
    private Queue<int> _turner = new Queue<int>();

    [Header("GameObjects")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;

    [Header("Characters")]
    public List<CombatCharacter> listCharacters;
    [Space]
    public CombatCharacter playerUnit; // TODO Mariano: DELETE
    public CombatCharacter enemyUnit; // TODO Mariano: DELETE

    private bool _turnedPass; // TODO Mariano: NO SE USA
    private int _characterIndex;

    private void StartCombat()
    {
        AddCharactersToList();
        // TODO Mariano: Setear las stats
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
            if (playerUnit.StatsAgility > enemyUnit.StatsAgility)
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

    // TODO Mariano: Player Ataca
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         StartCoroutine(PlayerAttack());
    //     }

    // }

    public void EndCombat()
    {
        if (state == CombatState.WON)
        {
            Debug.Log($"<b> You won the combat </b>");
        }
        else if (state == CombatState.LOST)
        {
            Debug.Log($"<b> You lost the combat </b>");
        }
    }

    public void PlayerTurn()
    {
        Debug.Log($"<b> Choose an action... </b>");
    }

    #region Enumerators

    /*Da comienzo al combate*/
    private IEnumerator SetupBattle()
    {
        Debug.Log($"<b> The combat is just started..  </b>");

        yield return new WaitForSeconds(2f);

        state = CombatState.PLAYERTURN;
        PlayerTurn();
    }

    /*Ejecuta la accion del jugador atacando*/
    IEnumerator PlayerAttack()
    {
        // enemyUnit.TakeDamage(playerUnit);
        // TODO Mariano: Dañar ENEMIGO

        // Debug.Log($"<b> Enemy Health: </b>" + enemyUnit.currentHP);

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        if (enemyUnit.IsAlive)
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
        // playerUnit.Defense();
        // TODO Mariano: Jugador se DEFIENDE

        Debug.Log($"<b> Defense increase. </b>");

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    /**/
    IEnumerator PlayerItem()
    {
        // Aplicar distintivo de player usando item

        // playerUnit.UseItem();
        // TODO Mariano: Jugador USA ITEM

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        StartCoroutine(EnemyTurn());
    }

    /**/
    IEnumerator PlayerEscape()
    {
        // Mover al personaje en direccion a la salida

        // playerUnit.Escape();
        // TODO Mariano: Jugador ESCAPA

        // menu.SetActive(false);

        yield return new WaitForSeconds(2f);

        state = CombatState.ENEMYTURN;

        EndCombat();
    }

    /*Ejecuta la corrutina del enemigo atacando*/
    IEnumerator EnemyTurn()
    {
        Debug.Log($"<b> Enemy turn! </b>");

        yield return new WaitForSeconds(2f);

        // playerUnit.TakeDamage(enemyUnit);
        // TODO Mariano: Daña al JUGADOR

        // Debug.Log($"<b> Health player: </b>" + playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (playerUnit.IsAlive)
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
        if (state != CombatState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnDefenseButton()
    {
        if (state != CombatState.PLAYERTURN)
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