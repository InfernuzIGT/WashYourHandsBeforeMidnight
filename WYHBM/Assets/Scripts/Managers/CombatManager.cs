using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Events;
using GameMode.Combat; // TODO Mariano: REVIEW
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("General")]
    public bool canSelect;
    public COMBAT_STATE combatState;
    public LayerMask currentLayer;

    [Header("Character - Players")]
    public List<Player> listPlayers;

    [Header("Character - Enemies")]
    public List<Enemy> listEnemies;

    private List<CombatCharacter> _listAllCharacters;
    private List<CombatCharacter> _listWaitingCharacters;
    private CombatCharacter _currentCharacter;
    private RaycastHit _hit;
    private Ray _ray;
    private GameObject _combatAreaContainer;
    private bool _isEndOfCombat;
    private int _turnCount;

    private WaitForSeconds combatTransition;
    private WaitForSeconds combatWaitTime;
    private WaitForSeconds evaluationDuration;

    private ExitCombatEvent _interactionCombatEvent;

    private void Start()
    {
        combatTransition = new WaitForSeconds(GameData.Instance.combatConfig.transitionDuration);
        combatWaitTime = new WaitForSeconds(GameData.Instance.combatConfig.waitCombatDuration);
        evaluationDuration = new WaitForSeconds(GameData.Instance.combatConfig.evaluationDuration);

        listPlayers = new List<Player>();
        listEnemies = new List<Enemy>();
        _listAllCharacters = new List<CombatCharacter>();
        _listWaitingCharacters = new List<CombatCharacter>();

        _interactionCombatEvent = new ExitCombatEvent();
    }

    public void SetData(CombatArea combatArea, List<Player> players, List<Enemy> enemies)
    {
        int indexCombat = 0;
        canSelect = false;

        _combatAreaContainer = Instantiate(
            GameData.Instance.gameConfig.emptyObject,
            combatArea.transform.position,
            combatArea.transform.rotation);

        for (int i = 0; i < players.Count; i++)
        {
            Player player = Instantiate(
                players[i],
                combatArea.playerPosition[i].position + GameData.Instance.gameConfig.playerBaseOffset,
                Quaternion.identity,
                _combatAreaContainer.transform);

            player.gameObject.SetActive(false);

            player.CombatIndex = indexCombat;
            indexCombat++;

            listPlayers.Add(player);
            _listAllCharacters.Add(player);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = Instantiate(
                enemies[i],
                combatArea.enemyPosition[i].position + GameData.Instance.gameConfig.playerBaseOffset,
                Quaternion.identity,
                _combatAreaContainer.transform);

            enemy.gameObject.SetActive(false);

            enemy.CombatIndex = indexCombat;
            indexCombat++;

            listEnemies.Add(enemy);
            _listAllCharacters.Add(enemy);
        }
    }

    #region Turn System

    /// <summary>
    /// Comienza el combate.
    /// </summary>
    public void InitiateTurn()
    {
        for (int i = 0; i < listPlayers.Count; i++)
        {
            listPlayers[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < listEnemies.Count; i++)
        {
            listEnemies[i].gameObject.SetActive(true);
        }

        _isEndOfCombat = false;

        AddToWaiting(InitialSort());
        SetInitialCharactersTurn();

        StartCoroutine(TurnsLoop());
        UnleashRace();
    }

    /// <summary>
    /// Reordena la lista de Characters.
    /// </summary>
    public List<CombatCharacter> InitialSort()
    {
        List<CombatCharacter> sortedCharacters = new List<CombatCharacter>();
        sortedCharacters.AddRange(_listAllCharacters);

        CombatCharacter fastestCharacter;
        fastestCharacter = sortedCharacters[0];

        for (int i = 0; i < sortedCharacters.Count - 1; i++) // ONE MINUS
        {
            for (int j = 0; j < sortedCharacters.Count - 1; j++) // ONE MINUS
            {
                if (sortedCharacters[j].StatsReaction < sortedCharacters[j + 1].StatsReaction)
                {
                    // Saving the Fastest one.
                    fastestCharacter = sortedCharacters[j + 1];

                    // Swaping the characters.
                    sortedCharacters[j + 1] = sortedCharacters[j];
                    sortedCharacters[j] = fastestCharacter;
                }
            }
        }

        return sortedCharacters;
    }

    /// <summary>
    /// Agrega a una lista de espera todos los Characters.
    /// </summary>
    public void AddToWaiting(List<CombatCharacter> charactersToAdd)
    {
        for (int i = 0; i < charactersToAdd.Count; i++)
        {
            _listWaitingCharacters.Add(charactersToAdd[i]);
        }
    }

    /// <summary>
    /// Espera la accion de un Character, y una vez cumplida lo envia al fondo y avanza al siguiente turno.
    /// </summary>
    private IEnumerator TurnsLoop()
    {
        while (!_isEndOfCombat)
        {
            // Waiting for the current character to do his action.
            yield return _currentCharacter.StartWaitingForAction();

            SendBottom();

            _turnCount++;

            // The Action Was done. Now should be the Next Characters Action.

            yield return null;
        }
    }

    /// <summary>
    /// Setea el turno al primer Character de la lista
    /// </summary>
    public void SetInitialCharactersTurn()
    {
        _currentCharacter = _listWaitingCharacters[0];
        _currentCharacter.IsMyTurn = true;

        Debug.Log($"<b> First Turn: </b> {_currentCharacter.name}");
    }

    /// <summary>
    /// Envia al Character al final de la lista
    /// </summary>
    public void SendBottom()
    {
        _currentCharacter.StartGettingAhead();

        _listWaitingCharacters.Remove(_currentCharacter);
        _listWaitingCharacters.Add(_currentCharacter);
        _currentCharacter = _listWaitingCharacters[0];

        Debug.Log($"<b> New Turn:  </b> {_currentCharacter.name}");

        _currentCharacter.IsMyTurn = true;
    }

    /// <summary>
    /// Reordena las posiciones de los Characters en la lista
    /// </summary>
    public void UnleashRace()
    {
        for (int i = 2; i < _listWaitingCharacters.Count; i++)
        {
            _listWaitingCharacters[i].StartGettingAhead();
        }
    }

    /// <summary>
    /// Coloca el Character por encima de la lista
    /// </summary>
    public void CharacterIsReadyToGoAhead(CombatCharacter characterGoingAhead)
    {
        int index;
        CombatCharacter auxCharacter;

        index = _listWaitingCharacters.IndexOf(characterGoingAhead);

        if (index <= 1)
        {
            _listWaitingCharacters[index].StartGettingAhead();
            return;
        }

        auxCharacter = _listWaitingCharacters[index - 1];

        _listWaitingCharacters[index - 1] = characterGoingAhead;
        _listWaitingCharacters[index] = auxCharacter;

        if ((index - 1) > 1)
        {
            characterGoingAhead.StartGettingAhead();
        }

        _listWaitingCharacters[index].StartGettingAhead();
    }

    #endregion

    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------

    public void ActionAttack()
    {
        combatState = COMBAT_STATE.Attack;
        EnableAction();
    }

    public void ActionDefense()
    {
        combatState = COMBAT_STATE.Defense;
        EnableAction();
    }

    public void ActionItem()
    {
        combatState = COMBAT_STATE.Item;
        EnableAction();
    }

    private void EnableAction()
    {
        switch (combatState)
        {
            case COMBAT_STATE.Attack:
            case COMBAT_STATE.Item:
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.actionTxt.text = "Select enemy";
                break;

            case COMBAT_STATE.Defense:
                currentLayer = GameData.Instance.combatConfig.layerPlayer;
                GameManager.Instance.combatUI.actionTxt.text = "Select player";
                break;

            default:
                currentLayer = GameData.Instance.combatConfig.layerNone;
                GameManager.Instance.combatUI.actionTxt.text = "";
                break;
        }

        canSelect = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSelect)
        {
            _ray = GameManager.Instance.GetRayMouse();

            if (Physics.Raycast(_ray, out _hit, 2000, currentLayer))
            {
                if (_hit.collider != null)
                {
                    _hit.collider.gameObject.GetComponent<Enemy>().Select(combatState, _currentCharacter);
                    canSelect = false;
                }
            }
        }
    }

    //------------------------------------

    public void CheckGame(Player character)
    {
        // TODO Mariano: Remover de todas las listas

        listPlayers.Remove(character);

        if (listPlayers.Count == 0)
        {
            EndGame(false);
        }
    }

    public void CheckGame(Enemy character)
    {
        // TODO Mariano: Remover de todas las listas

        listEnemies.Remove(character);

        if (listEnemies.Count == 0)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool isWin)
    {
        _isEndOfCombat = true;

        Debug.Log($"<b> GAME STATE: </b> {isWin}");

        // uIController.endTxt.text = isWin ? GameData.Instance.textConfig.gameWinTxt : GameData.Instance.textConfig.gameLoseTxt;

        // uIController.endTxt.gameObject.SetActive(true);

        // uIController.endTxt.transform.
        // DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 1, 5, .5f).
        // SetEase(Ease.OutQuad);

        _interactionCombatEvent.isWin = isWin;
        EventController.TriggerEvent(_interactionCombatEvent);

    }

    public void CloseCombatArea()
    {
        Destroy(_combatAreaContainer.gameObject);

        _combatAreaContainer = null;
        _currentCharacter = null;

        listPlayers.Clear();
        listEnemies.Clear();
        _listAllCharacters.Clear();
        _listWaitingCharacters.Clear();
    }
}