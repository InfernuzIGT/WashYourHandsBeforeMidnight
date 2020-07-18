using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("General")]
    public bool canSelect;
    public ANIM_STATE animState;
    public ItemSO currentItem;
    public LayerMask currentLayer;

    [Header("Character - Players")]
    public List<Player> listPlayers;

    [Header("Character - Enemies")]
    public List<Enemy> listEnemies;

    private List<CombatCharacter> _listAllCharacters;
    private CombatCharacter _currentCharacter;
    private RaycastHit _hit;
    private Ray _ray;
    private GameObject _combatAreaContainer;
    private bool _isEndOfCombat;
    private int _turnCount;

    private WaitForSeconds _waitStart;
    private WaitForSeconds _waitBetweenTurns;

    private ExitCombatEvent _interactionCombatEvent;

    // Properties
    private InputActions _inputActions;
    public InputActions InputActions { get { return _inputActions; } set { _inputActions = value; } }

    private List<CombatCharacter> _listWaitingCharacters;
    public List<CombatCharacter> ListWaitingCharacters { get { return _listWaitingCharacters; } }

    public void Awake()
    {
        CreateInput();
    }

    private void CreateInput()
    {
        InputActions = new InputActions();

        InputActions.ActionPlayer.Click.performed += ctx => SelectEnemy();
    }

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

    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }

    // private void Update()
    // {
    //     SelectEnemy();
    // }

    private void SelectEnemy()
    {
        if (canSelect)
        {
            _ray = GameManager.Instance.GetRayMouse();

            if (Physics.Raycast(_ray, out _hit, 100, currentLayer))
            {
                if (_hit.collider != null)
                {
                    _hit.collider.gameObject.GetComponent<CombatCharacter>().Select(currentItem);

                    _currentCharacter.AnimationAction(animState);
                    _currentCharacter.DoAction();

                    canSelect = false;

                    // GameManager.Instance.combatUI.EnableActions(false);
                }
            }
        }
    }

    public void SetData(CombatArea combatArea, List<CombatPlayer> combatPlayers, List<CombatEnemy> combatEnemies)
    {
        int indexCombat = 0;
        canSelect = false;

        _combatAreaContainer = Instantiate(
            GameData.Instance.worldConfig.emptyObject,
            combatArea.transform.position,
            combatArea.transform.rotation);

        for (int i = 0; i < combatPlayers.Count; i++)
        {
            Player player = Instantiate(
                combatPlayers[i].character,
                combatArea.playerPosition[i].position + GameData.Instance.worldConfig.playerBaseOffset,
                Quaternion.identity,
                _combatAreaContainer.transform);

            player.SetCharacter(indexCombat, combatPlayers[i].equipment);
            indexCombat++;

            GameManager.Instance.combatUI.CreateActions(combatPlayers[i].equipment);

            listPlayers.Add(player);
            _listAllCharacters.Add(player);
        }

        for (int i = 0; i < combatEnemies.Count; i++)
        {
            Enemy enemy = Instantiate(
                combatEnemies[i].character,
                combatArea.enemyPosition[i].position + GameData.Instance.worldConfig.playerBaseOffset,
                Quaternion.identity,
                _combatAreaContainer.transform);

            enemy.SetCharacter(indexCombat, combatEnemies[i].equipment);
            indexCombat++;

            listEnemies.Add(enemy);
            _listAllCharacters.Add(enemy);
        }

        GameManager.Instance.combatUI.CreateTurn(_listAllCharacters);
    }

    public void DoAction(ItemSO item)
    {
        // if (canSelect)return;

        if (item == null)
        {
            animState = ANIM_STATE.AttackMelee;
            currentLayer = GameData.Instance.combatConfig.layerEnemy;
            GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
            currentItem = null;
            canSelect = true;
            SetHighlight(true, false);
            return;
        }

        switch (item.type)
        {
            case ITEM_TYPE.WeaponMelee:
                animState = ANIM_STATE.AttackMelee;
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                SetHighlight(true, false);
                break;

            case ITEM_TYPE.WeaponOneHand:
                animState = ANIM_STATE.AttackOneHand;
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                SetHighlight(true, false);
                break;

            case ITEM_TYPE.WeaponTwoHands:
                animState = ANIM_STATE.AttackTwoHands;
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                SetHighlight(true, false);
                break;

            case ITEM_TYPE.ItemHeal:
                animState = ANIM_STATE.ItemHeal;
                currentLayer = GameData.Instance.combatConfig.layerPlayer;
                GameManager.Instance.combatUI.messageTxt.text = "Select player";
                SetHighlight(true, true);
                break;

            case ITEM_TYPE.ItemGrenade:
                animState = ANIM_STATE.ItemGrenade;
                currentLayer = GameData.Instance.combatConfig.layerEnemy;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                SetHighlight(true, false);
                break;

            case ITEM_TYPE.ItemDefense:
                animState = ANIM_STATE.ItemDefense;
                currentLayer = GameData.Instance.combatConfig.layerPlayer;
                GameManager.Instance.combatUI.messageTxt.text = "Select player";
                SetHighlight(true, true);
                break;

            default:
                currentLayer = GameData.Instance.combatConfig.layerNone;
                GameManager.Instance.combatUI.messageTxt.text = "";
                SetHighlight(false, true);
                SetHighlight(false, false);
                break;
        }

        currentItem = item;
        canSelect = true;
    }

    public void CheckGame(Player character)
    {
        _listAllCharacters.Remove(character);
        _listWaitingCharacters.Remove(character);
        
        Debug.Log ($"<b> REMOVED PLAYER </b>");

        listPlayers.Remove(character);

        GameManager.Instance.ReorderTurn();

        if (listPlayers.Count == 0)
        {
            FinishGame(false);
        }
    }

    public void CheckGame(Enemy character)
    {
        _listAllCharacters.Remove(character);
        _listWaitingCharacters.Remove(character);

        Debug.Log ($"<b> REMOVED ENEMY </b>");
        
        listEnemies.Remove(character);

        GameManager.Instance.ReorderTurn();

        if (listEnemies.Count == 0)
        {
            FinishGame(true);
        }
    }

    private void FinishGame(bool isWin)
    {
        _isEndOfCombat = true;

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

    public void SetHighlight(bool canHighlight, bool selectPlayers)
    {
        if (selectPlayers)
        {
            for (int i = 0; i < listPlayers.Count; i++)
            {
                listPlayers[i].CanHighlight = canHighlight;
            }
        }
        else
        {
            for (int i = 0; i < listEnemies.Count; i++)
            {
                listEnemies[i].CanHighlight = canHighlight;
            }
        }
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

        GameManager.Instance.ReorderTurn();
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