using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Character - Players")]
    public List<Player> listPlayers;

    [Header("Character - Enemies")]
    public List<Enemy> listEnemies;

    private List<CombatCharacter> _listAllCharacters;
    private List<CombatCharacter> _listSelectionCharacters;
    private CombatCharacter _currentCharacter;
    private COMBAT_STATE _combatState;
    private ANIM_STATE _animState;
    private ItemSO _currentItem;
    private GameObject _combatAreaContainer;
    private bool _isEndOfCombat;

    // Input
    private Vector2 _inputMovement;
    private int _indexCharacter;
    private int _indexLastCharacter;

    private Coroutine _coroutineTurnsLoop;
    // private WaitForSeconds _waitStart;
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

        InputActions.UI.Navigate.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        InputActions.UI.Navigate.performed += ctx => NavigateCharacter();
        InputActions.UI.Submit.performed += ctx => SelectCharacter(true);
        InputActions.UI.Cancel.performed += ctx => SelectCharacter(false);
    }

    private void Start()
    {
        listPlayers = new List<Player>();
        listEnemies = new List<Enemy>();
        _listAllCharacters = new List<CombatCharacter>();
        _listWaitingCharacters = new List<CombatCharacter>();
        _listSelectionCharacters = new List<CombatCharacter>();

        // _waitStart = new WaitForSeconds(GameData.Instance.combatConfig.waitTimeToStart);
        _waitBetweenTurns = new WaitForSeconds(GameData.Instance.combatConfig.waitTimeBetweenTurns);

        _interactionCombatEvent = new ExitCombatEvent();
    }

    public void SetData(CombatArea combatArea, List<CombatPlayer> combatPlayers, List<CombatEnemy> combatEnemies)
    {
        int indexCombat = 0;

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
            player.gameObject.SetActive(false);
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
            enemy.gameObject.SetActive(false);
            indexCombat++;

            listEnemies.Add(enemy);
            _listAllCharacters.Add(enemy);
        }

        GameManager.Instance.combatUI.CreateTurn(_listAllCharacters);
    }

    public void ToggleInputCombat(bool isEnabled)
    {
        if (isEnabled)
        {
            InputActions.Enable();
        }
        else
        {
            InputActions.Disable();
        }
    }

    public void SelectAction(ItemSO item)
    {
        _combatState = COMBAT_STATE.SelectCharacter;

        if (item == null)
        {
            _animState = ANIM_STATE.AttackMelee;
            GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
            _currentItem = null;
            HighlightCharacters(true, false);
            return;
        }

        switch (item.type)
        {
            case ITEM_TYPE.WeaponMelee:
                _animState = ANIM_STATE.AttackMelee;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                HighlightCharacters(true, false);
                break;

            case ITEM_TYPE.WeaponOneHand:
                _animState = ANIM_STATE.AttackOneHand;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                HighlightCharacters(true, false);
                break;

            case ITEM_TYPE.WeaponTwoHands:
                _animState = ANIM_STATE.AttackTwoHands;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                HighlightCharacters(true, false);
                break;

            case ITEM_TYPE.ItemHeal:
                _animState = ANIM_STATE.ItemHeal;
                GameManager.Instance.combatUI.messageTxt.text = "Select player";
                HighlightCharacters(true, true);
                break;

            case ITEM_TYPE.ItemGrenade:
                _animState = ANIM_STATE.ItemGrenade;
                GameManager.Instance.combatUI.messageTxt.text = "Select enemy";
                HighlightCharacters(true, false);
                break;

            case ITEM_TYPE.ItemDefense:
                _animState = ANIM_STATE.ItemDefense;
                GameManager.Instance.combatUI.messageTxt.text = "Select player";
                HighlightCharacters(true, true);
                break;

            default:
                Debug.Log($"<b> NONE </b>");
                _combatState = COMBAT_STATE.None;
                GameManager.Instance.combatUI.messageTxt.text = "";
                HighlightCharacters(false, true);
                HighlightCharacters(false, false);
                break;
        }

        _currentItem = item;
    }

    private void HighlightCharacters(bool canHighlight, bool isPlayer)
    {
        GameManager.Instance.combatUI.HideActions(canHighlight);

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

    public void CheckGame()
    {
        if (listPlayers.Count == 0)
        {
            FinishGame(false);
        }
        else if (listEnemies.Count == 0)
        {
            FinishGame(true);
        }
    }

    public void RemoveCharacter(Player character)
    {
        character.StopGettingAhead();

        _listAllCharacters.Remove(character);
        _listWaitingCharacters.Remove(character);
        listPlayers.Remove(character);

        GameManager.Instance.ReorderTurn();

        // if (listPlayers.Count == 0)
        // {
        //     FinishGame(false);
        // }
    }

    public void RemoveCharacter(Enemy character)
    {
        character.StopGettingAhead();

        _listAllCharacters.Remove(character);
        _listWaitingCharacters.Remove(character);
        listEnemies.Remove(character);

        GameManager.Instance.ReorderTurn();

        // if (listEnemies.Count == 0)
        // {
        //     FinishGame(true);
        // }
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
            Destroy(_combatAreaContainer.gameObject);

            _combatAreaContainer = null;
            _currentCharacter = null;

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
        // yield return _waitStart;

        while (!_isEndOfCombat)
        {
            yield return _currentCharacter.StartWaitingForAction();

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