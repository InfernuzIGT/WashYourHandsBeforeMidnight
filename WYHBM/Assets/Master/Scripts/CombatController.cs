using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Events;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private CombatConfig _combatConfig = null;
    [SerializeField, ReadOnly] private CombatArea _currentCombatArea = null;
    [SerializeField] private List<Player> _combatPlayers = null;

    [Header("Character - Players")]
    [SerializeField] private List<Player> listPlayers;

    [Header("Character - Enemies")]
    public List<Enemy> listEnemies;

    private List<CombatCharacter> _listAllCharacters;
    private List<CombatCharacter> _listSelectionCharacters;
    private CombatCharacter _currentCharacter;
    private COMBAT_STATE _combatState;
    private ANIM_STATE _animState;
    private ItemSO _currentItem;
    private bool _isEndOfCombat;

    // Input
    private Vector2 _inputMovement;
    private int _indexCharacter;
    private int _indexLastCharacter;

    private Coroutine _coroutineTurnsLoop;
    private WaitForSeconds _waitBetweenTurns;

    // Events
    private CombatEvent _combatEvent;
    private CombatCreateActionsEvent _combatCreateActionsEvent;
    private CombatHideActionsEvent _combatHideActionsEvent;

    // Properties
    private List<CombatCharacter> _listWaitingCharacters;
    public List<CombatCharacter> ListWaitingCharacters { get { return _listWaitingCharacters; } }

    public List<Player> ListPlayers { get { return listPlayers; } }

    private void Start()
    {
        listPlayers = new List<Player>();
        listEnemies = new List<Enemy>();
        _listAllCharacters = new List<CombatCharacter>();
        _listWaitingCharacters = new List<CombatCharacter>();
        _listSelectionCharacters = new List<CombatCharacter>();

        _waitBetweenTurns = new WaitForSeconds(_combatConfig.waitTimeBetweenTurns);

        _combatEvent = new CombatEvent();
        _combatEvent.isEnter = false;

        _combatCreateActionsEvent = new CombatCreateActionsEvent();
        _combatHideActionsEvent = new CombatHideActionsEvent();

        EventSystemUtility.Instance.InputUIModule.move.action.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        EventSystemUtility.Instance.InputUIModule.move.action.performed += ctx => NavigateCharacter();
        EventSystemUtility.Instance.InputUIModule.submit.action.performed += ctx => SelectCharacter(true);
        EventSystemUtility.Instance.InputUIModule.cancel.action.performed += ctx => SelectCharacter(false);
    }

    private void OnEnable()
    {
        EventController.AddListener<CombatEvent>(OnCombatEvent);
        EventController.AddListener<CombatActionEvent>(OnCombatAction);
        EventController.AddListener<CombatRemoveCharacterEvent>(OnCombatRemoveCharacter);
        EventController.AddListener<CombatPlayerEvent>(OnCombatPlayer);
        EventController.AddListener<CombatCharacterGoAheadEvent>(OnCombatCharacterGoAhead);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<CombatEvent>(OnCombatEvent);
        EventController.RemoveListener<CombatActionEvent>(OnCombatAction);
        EventController.RemoveListener<CombatRemoveCharacterEvent>(OnCombatRemoveCharacter);
        EventController.RemoveListener<CombatPlayerEvent>(OnCombatPlayer);
        EventController.RemoveListener<CombatCharacterGoAheadEvent>(OnCombatCharacterGoAhead);
    }

    public CinemachineVirtualCamera GetCombatAreaCamera()
    {
        return _currentCombatArea.virtualCamera;
    }

    private void OnCombatEvent(CombatEvent evt)
    {
        if (evt.isEnter)
        {
            _currentCombatArea = GameData.Instance.SpawnPoint.GetCombatArea();
            SetData(_currentCombatArea, _combatPlayers, evt.combatEnemies);
        }
        else
        {

        }
    }

    private void OnCombatAction(CombatActionEvent evt)
    {
        _combatState = COMBAT_STATE.SelectCharacter;

        if (evt.item == null)
        {
            _animState = ANIM_STATE.Attack_1;
            _currentItem = null;
            HighlightCharacters(true, false);
            return;
        }

        switch (evt.item.type)
        {
            case ITEM_TYPE.WeaponMelee:
                _animState = ANIM_STATE.Attack_1;
                HighlightCharacters(true, false);
                break;

            case ITEM_TYPE.WeaponOneHand:
            case ITEM_TYPE.WeaponTwoHands:
                _animState = ANIM_STATE.Attack_2;
                HighlightCharacters(true, false);
                break;

            case ITEM_TYPE.ItemHeal:
            case ITEM_TYPE.ItemGrenade:
            case ITEM_TYPE.ItemDefense:
                _animState = ANIM_STATE.Item;
                HighlightCharacters(true, true);
                break;

            default:
                _combatState = COMBAT_STATE.None;
                HighlightCharacters(false, true);
                HighlightCharacters(false, false);
                break;
        }

        _currentItem = evt.item;
    }

    private void OnCombatPlayer(CombatPlayerEvent evt)
    {
        ChangeCombatState(evt.canSelect);
    }

    private void CheckGame()
    {
        if (listPlayers.Count == 0)
        {
            StopCoroutine(TurnsLoop());
            FinishGame(false);
        }
        else if (listEnemies.Count == 0)
        {
            StopCoroutine(TurnsLoop());
            FinishGame(true);
        }
    }

    private void OnCombatCharacterGoAhead(CombatCharacterGoAheadEvent evt)
    {
        int index;
        CombatCharacter auxCharacter;

        index = _listWaitingCharacters.IndexOf(evt.character);

        if (index < 0)return;

        if (index <= 1)
        {
            _listWaitingCharacters[index].StartGettingAhead();
            return;
        }

        auxCharacter = _listWaitingCharacters[index - 1];

        _listWaitingCharacters[index - 1] = evt.character;
        _listWaitingCharacters[index] = auxCharacter;

        if ((index - 1) > 1)
        {
            evt.character.StartGettingAhead();
        }

        _listWaitingCharacters[index].StartGettingAhead();
    }

    private void SetData(CombatArea combatArea, List<Player> combatPlayers, List<Enemy> combatEnemies)
    {
        int indexCombat = 0;

        for (int i = 0; i < combatPlayers.Count; i++)
        {
            Player player = Instantiate(
                combatPlayers[i],
                _currentCombatArea.playerPosition[i].position,
                Quaternion.identity,
                _currentCombatArea.transform);

            player.SetCharacter(indexCombat);
            player.gameObject.SetActive(false);
            indexCombat++;

            _combatCreateActionsEvent.equipment = combatPlayers[i].Data.Equipment;
            EventController.TriggerEvent(_combatCreateActionsEvent);

            listPlayers.Add(player);
            _listAllCharacters.Add(player);
        }

        for (int i = 0; i < combatEnemies.Count; i++)
        {
            Enemy enemy = Instantiate(
                combatEnemies[i],
                _currentCombatArea.enemyPosition[i].position,
                Quaternion.identity,
                _currentCombatArea.transform);

            enemy.SetCharacter(indexCombat);
            enemy.gameObject.SetActive(false);
            indexCombat++;

            listEnemies.Add(enemy);
            _listAllCharacters.Add(enemy);
        }
    }

    private void HighlightCharacters(bool canHighlight, bool isPlayer)
    {
        _combatHideActionsEvent.canHighlight = canHighlight;
        EventController.TriggerEvent(_combatHideActionsEvent);

        if (isPlayer)
        {
            for (int i = 0; i < listPlayers.Count; i++)
            {
                listPlayers[i].CanHighlight = canHighlight;
            }
            if (canHighlight)
            {
                _listSelectionCharacters.Clear();
                _listSelectionCharacters.AddRange(listPlayers);

                _indexLastCharacter = 0;
                _indexCharacter = 0;
                _listSelectionCharacters[_indexLastCharacter].ShowUI(false);
                _listSelectionCharacters[_indexCharacter].ShowUI(true);
            }
            else
            {
                _listSelectionCharacters.Clear();
            }
        }
        else
        {
            for (int i = 0; i < listEnemies.Count; i++)
            {
                listEnemies[i].CanHighlight = canHighlight;
            }
            if (canHighlight)
            {
                _listSelectionCharacters.Clear();
                _listSelectionCharacters.AddRange(listEnemies);

                _indexLastCharacter = 0;
                _indexCharacter = 0;
                _listSelectionCharacters[_indexLastCharacter].ShowUI(false);
                _listSelectionCharacters[_indexCharacter].ShowUI(true);
            }
            else
            {
                _listSelectionCharacters.Clear();
            }
        }
    }

    private void NavigateCharacter()
    {
        if (_combatState != COMBAT_STATE.SelectCharacter)return;

        if (!Mathf.Approximately(_inputMovement.x, 0f) && _inputMovement.sqrMagnitude > 0)
        {
            if (_inputMovement.x > 0)
            {
                if (_indexCharacter < _listSelectionCharacters.Count - 1)
                {
                    _indexLastCharacter = _indexCharacter;
                    _indexCharacter++;
                }
            }
            else
            {
                if (_indexCharacter > 0)
                {
                    _indexLastCharacter = _indexCharacter;
                    _indexCharacter--;
                }
            }

            _listSelectionCharacters[_indexLastCharacter].ShowUI(false);
            _listSelectionCharacters[_indexCharacter].ShowUI(true);
        }
    }

    private void SelectCharacter(bool isSelection)
    {
        if (_combatState != COMBAT_STATE.SelectCharacter)return;

        if (isSelection)
        {
            _combatState = COMBAT_STATE.MakeAction;

            _listSelectionCharacters[_indexCharacter].Select(_currentItem);

            _currentCharacter.AnimationAction(_animState);
            _currentCharacter.DoAction();
        }
        else
        {
            _combatState = COMBAT_STATE.SelectAction;
            _listSelectionCharacters[_indexCharacter].ShowUI(false);
            HighlightCharacters(false, false);
        }
    }

    public void ChangeCombatState(bool canSelect)
    {
        _combatState = canSelect ? COMBAT_STATE.SelectAction : COMBAT_STATE.Wait;
    }

    private void OnCombatRemoveCharacter(CombatRemoveCharacterEvent evt)
    {
        evt.character.StopGettingAhead();

        _listAllCharacters.Remove(evt.character);
        _listWaitingCharacters.Remove(evt.character);

        if (evt.isPlayer)
        {
            Player player = evt.character as Player;
            listPlayers.Remove(player);
        }
        else
        {
            Enemy enemy = evt.character as Enemy;
            listEnemies.Remove(enemy);
        }
    }

    private void FinishGame(bool isWin)
    {
        Debug.Log($"<b> WIN: {isWin} </b>");

        _isEndOfCombat = true;
        
        _combatEvent.isWin = isWin;
        EventController.TriggerEvent(_combatEvent);
    }

    public void SetCombatArea(bool isEnabled)
    {
        if (isEnabled)
        {
            for (int i = 0; i < listPlayers.Count; i++)
            {
                listPlayers[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < listEnemies.Count; i++)
            {
                listEnemies[i].gameObject.SetActive(true);
            }
        }
        else
        {
            _currentCharacter = null;

            for (int i = 0; i < _listAllCharacters.Count; i++)
            {
                Destroy(_listAllCharacters[i].gameObject);
            }

            listPlayers.Clear();
            listEnemies.Clear();
            _listAllCharacters.Clear();
            _listWaitingCharacters.Clear();
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

        _coroutineTurnsLoop = StartCoroutine(TurnsLoop());
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
                if (sortedCharacters[j].Data.StatsReaction < sortedCharacters[j + 1].Data.StatsReaction)
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
        // yield return _waitStart;

        while (!_isEndOfCombat)
        {
            yield return _currentCharacter.StartWaitingForAction();

            CheckGame();

            SendBottom();

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

    #endregion
}