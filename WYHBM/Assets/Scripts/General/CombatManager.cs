using System.Collections;
using System.Collections.Generic;
using Events;
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

    private WaitForSeconds _waitStart;
    private WaitForSeconds _waitBetweenTurns;

    private ExitCombatEvent _interactionCombatEvent;

    private void Start()
    {
        listPlayers = new List<Player>();
        listEnemies = new List<Enemy>();
        _listAllCharacters = new List<CombatCharacter>();
        _listWaitingCharacters = new List<CombatCharacter>();

        _waitStart = new WaitForSeconds(GameData.Instance.combatConfig.waitTimeToStart);
        _waitBetweenTurns = new WaitForSeconds(GameData.Instance.combatConfig.waitTimeBetweenTurns);

        _interactionCombatEvent = new ExitCombatEvent();
    }

    private void Update()
    {
        SelectEnemy();
    }

    private void SelectEnemy()
    {
        if (Input.GetMouseButtonDown(0) && canSelect)
        {
            _ray = GameManager.Instance.GetRayMouse();

            if (Physics.Raycast(_ray, out _hit, 100, currentLayer))
            {
                if (_hit.collider != null)
                {
                    _hit.collider.gameObject.GetComponent<CombatCharacter>().Select(combatState, _currentCharacter);
                    canSelect = false;
                }
            }
        }
    }

    public void SetData(CombatArea combatArea, List<CombatPlayer> combatPlayers, List<CombatEnemy> combatEnemies)
    {
        int indexCombat = 0;
        canSelect = false;

        _combatAreaContainer = Instantiate(
            GameData.Instance.gameConfig.emptyObject,
            combatArea.transform.position,
            combatArea.transform.rotation);

        for (int i = 0; i < combatPlayers.Count; i++)
        {
            Player player = Instantiate(
                combatPlayers[i].character,
                combatArea.playerPosition[i].position + GameData.Instance.gameConfig.playerBaseOffset,
                Quaternion.identity,
                _combatAreaContainer.transform);

            player.SetCharacter(indexCombat, combatPlayers[i].inventory);
            indexCombat++;

            listPlayers.Add(player);
            _listAllCharacters.Add(player);
        }

        for (int i = 0; i < combatEnemies.Count; i++)
        {
            Enemy enemy = Instantiate(
                combatEnemies[i].character,
                combatArea.enemyPosition[i].position + GameData.Instance.gameConfig.playerBaseOffset,
                Quaternion.identity,
                _combatAreaContainer.transform);

            enemy.SetCharacter(indexCombat, combatEnemies[i].inventory);
            indexCombat++;

            listEnemies.Add(enemy);
            _listAllCharacters.Add(enemy);
        }
    }

    public void DoAction(COMBAT_STATE combatState)
    {
        switch (combatState)
        {
            case COMBAT_STATE.Attack:
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                break;

            case COMBAT_STATE.Defense:
                currentLayer = GameData.Instance.combatConfig.layerPlayer;
                GameManager.Instance.combatUI.messageTxt.text = "Select player";
                break;

            case COMBAT_STATE.Item:
                if (_currentCharacter.InventoryCombat.item != null)
                {
                    ReadItem();
                }
                else
                {
                    Debug.Log($"No items!");
                    return;
                }
                break;

            default:
                currentLayer = GameData.Instance.combatConfig.layerNone;
                GameManager.Instance.combatUI.messageTxt.text = "";
                break;
        }

        this.combatState = combatState;
        canSelect = true;
    }

    private void ReadItem()
    {
        switch (_currentCharacter.InventoryCombat.item.type)
        {
            case ITEM_TYPE.Damage:
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                break;

            case ITEM_TYPE.Heal:
                currentLayer = GameData.Instance.combatConfig.layerPlayer;
                GameManager.Instance.combatUI.messageTxt.text = "Select Player";
                break;

            default:
                break;
        }
    }

    public void CheckGame(Player character)
    {
        _listAllCharacters.Remove(character);
        _listWaitingCharacters.Remove(character);

        listPlayers.Remove(character);

        if (listPlayers.Count == 0)
        {
            FinishGame(false);
        }
    }

    public void CheckGame(Enemy character)
    {
        _listAllCharacters.Remove(character);
        _listWaitingCharacters.Remove(character);

        listEnemies.Remove(character);

        if (listEnemies.Count == 0)
        {
            FinishGame(true);
        }
    }

    private void FinishGame(bool isWin)
    {
        _isEndOfCombat = true;

        Debug.Log($"<color=blue><b> [COMBAT] </b></color> Win: {isWin}");

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

    #region Turn System

    /// <summary>
    /// Comienza el combate.
    /// </summary>
    public void InitiateTurn()
    {
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

        for (int i = 0; i < sortedCharacters.Count - 1; i++)
        {
            for (int j = 0; j < sortedCharacters.Count - 1; j++)
            {
                if (sortedCharacters[j].StatsReaction < sortedCharacters[j + 1].StatsReaction)
                {
                    fastestCharacter = sortedCharacters[j + 1];

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
        yield return _waitStart;

        while (!_isEndOfCombat)
        {
            yield return _currentCharacter.StartWaitingForAction();

            SendBottom();

            _turnCount++;

            yield return _waitBetweenTurns;
        }
    }

    /// <summary>
    /// Setea el turno al primer Character de la lista
    /// </summary>
    public void SetInitialCharactersTurn()
    {
        _currentCharacter = _listWaitingCharacters[0];
        _currentCharacter.IsMyTurn = true;
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
}