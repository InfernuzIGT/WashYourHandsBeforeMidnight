using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using GameMode.Combat;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Mariano")]
    public LayerMask currentLayer;
    public COMBAT_STATE combatState;
    public CombatArea[] combatAreas;
    public int currentArea;
    public bool canSelect;

    [Header("Controllers")]
    public UIController uIController;
    public ActionController actionController;

    [Header("General")]
    public bool isTurnPlayer = true;

    [Header("Characters")]
    public Transform groupPlayers;
    public Transform groupEnemies;
    [Space]
    public List<Player> listPlayers = new List<Player>();
    [Space]
    public List<Enemy> listEnemies = new List<Enemy>();

    private bool _isActionEnable;
    private int _combatIndex;
    private WaitForSeconds combatTransition;
    private WaitForSeconds combatWaitTime;
    private WaitForSeconds evaluationDuration;

    private RaycastHit _hit;
    private Ray _ray;

    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------

    [Header("NEW COMBAT")]
    public CombatCharacter currentCharacter;
    [Space]
    public List<CombatCharacter> listCharacter;
    public int turnCount;

    private List<CombatCharacter> waitingForAction;
    private bool endOfCombat = false;

    private void Start()
    {
        combatTransition = new WaitForSeconds(GameData.Instance.combatConfig.transitionDuration);
        combatWaitTime = new WaitForSeconds(GameData.Instance.combatConfig.waitCombatDuration);
        evaluationDuration = new WaitForSeconds(GameData.Instance.combatConfig.evaluationDuration);

        listCharacter = new List<CombatCharacter>();
        waitingForAction = new List<CombatCharacter>();
    }

    public void SetData(List<Player> players, List<Enemy> enemies)
    {
        listPlayers.Clear();
        listEnemies.Clear();
        listCharacter.Clear();
        _combatIndex = 0;
        canSelect = false;

        currentArea = Random.Range(0, combatAreas.Length);

        for (int i = 0; i < players.Count; i++)
        {
            Player player = Instantiate(
                players[i],
                combatAreas[currentArea].playerPosition[i].position + GameData.Instance.gameConfig.playerBaseOffset,
                Quaternion.identity);

            player.gameObject.SetActive(false);

            player.CombatIndex = _combatIndex;
            _combatIndex++;

            listPlayers.Add(player);
            listCharacter.Add(player);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = Instantiate(
                enemies[i],
                combatAreas[currentArea].enemyPosition[i].position + GameData.Instance.gameConfig.playerBaseOffset,
                Quaternion.identity);

            enemy.gameObject.SetActive(false);

            enemy.CombatIndex = _combatIndex;
            _combatIndex++;

            listEnemies.Add(enemy);
            listCharacter.Add(enemy);
        }
    }

    public CinemachineVirtualCamera SetCamera()
    {
        for (int i = 0; i < listPlayers.Count; i++)
        {
            listPlayers[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < listEnemies.Count; i++)
        {
            listEnemies[i].gameObject.SetActive(true);
        }

        return combatAreas[currentArea].virtualCamera;
    }

    #region Turn System

    // Start Combat
    public void InitiateTurn()
    {
        AddToWaiting(InitialSort());
        SetInitialCharactersTurn();

        StartCoroutine(TurnsLoop());
        UnleashRace();
    }

    public List<CombatCharacter> InitialSort()
    {
        List<CombatCharacter> sortedCharacters = new List<CombatCharacter>();
        sortedCharacters.AddRange(listCharacter);

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

    public void AddToWaiting(List<CombatCharacter> charactersToAdd)
    {
        for (int i = 0; i < charactersToAdd.Count; i++)
        {
            waitingForAction.Add(charactersToAdd[i]);
        }
    }

    private IEnumerator TurnsLoop()
    {
        while (!endOfCombat)
        {
            // Waiting for the current character to do his action.
            yield return currentCharacter.StartWaitingForAction();

            SendBottom();

            turnCount++;

            // The Action Was done. Now should be the Next Characters Action.

            yield return null;
        }
    }

    public void SetInitialCharactersTurn()
    {
        currentCharacter = waitingForAction[0];
        currentCharacter.IsMyTurn = true;
    }

    public void SendBottom()
    {
        currentCharacter.StartGettingAhead();

        waitingForAction.Remove(currentCharacter);
        waitingForAction.Add(currentCharacter);
        currentCharacter = waitingForAction[0];

        currentCharacter.IsMyTurn = true;
    }

    public void UnleashRace()
    {
        for (int i = 2; i < waitingForAction.Count; i++)
        {
            waitingForAction[i].StartGettingAhead();
        }
    }

    public void CharacterIsReadyToGoAhead(CombatCharacter characterGoingAhead)
    {
        int index;
        CombatCharacter auxCharacter;

        index = waitingForAction.IndexOf(characterGoingAhead);

        if (index <= 1)
        {
            waitingForAction[index].StartGettingAhead();
            return;
        }

        auxCharacter = waitingForAction[index - 1];

        waitingForAction[index - 1] = characterGoingAhead;
        waitingForAction[index] = auxCharacter;

        if ((index - 1) > 1)
        {
            characterGoingAhead.StartGettingAhead();
        }

        waitingForAction[index].StartGettingAhead();
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
                currentLayer = GameData.Instance.gameConfig.layerEnemy;
                GameManager.Instance.combatUI.actionTxt.text = "Select enemy";
                break;

            case COMBAT_STATE.Defense:
                currentLayer = GameData.Instance.gameConfig.layerPlayer;
                GameManager.Instance.combatUI.actionTxt.text = "Select player";
                break;

            default:
                currentLayer = GameData.Instance.gameConfig.layerNone;
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
                    _hit.collider.gameObject.GetComponent<CombatCharacter>().Select(combatState, currentCharacter);
                    canSelect = false;
                }
            }
        }
    }

    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------

    private IEnumerator Combat(List<Player> players)
    {
        if (listPlayers[0].IsAlive) // TODO Mariano: Si hay algun PLAYER vivo
        {
            // TODO Mariano: Fade IN + BLUR

            // TODO Mariano: Fade de los personajes involucrados
            listPlayers[0].ActionStartCombat();
            listEnemies[0].ActionStartCombat();

            yield return combatTransition;

            actionController.PlayAction(players);
            uIController.ChangeUI(!listEnemies[0].IsAlive); // Need FALSE

            yield return combatWaitTime;

            // TODO Mariano: Fade OUT + BLUR

            // TODO Mariano: Fade de los personajes involucrados
            listPlayers[0].ActionStopCombat();
            listEnemies[0].ActionStopCombat();

            yield return combatTransition;

            StartCoroutine(Combat(listEnemies));
        }
        else
        {
            yield return evaluationDuration;

            EndGame(false);
        }

    }

    private IEnumerator Combat(List<Enemy> enemies)
    {
        if (listEnemies[0].IsAlive) // TODO Mariano: Si hay algun ENEMY vivo
        {
            yield return evaluationDuration;

            // TODO Mariano: Fade IN + BLUR

            // TODO Mariano: Fade de los personajes involucrados
            listPlayers[0].ActionStartCombat();
            listEnemies[0].ActionStartCombat();

            yield return combatTransition;

            actionController.PlayAction(enemies);
            uIController.ChangeUI(listPlayers[0].IsAlive); // Need TRUE

            yield return combatWaitTime;

            // TODO Mariano: Fade OUT + BLUR

            // TODO Mariano: Fade de los personajes involucrados
            listPlayers[0].ActionStopCombat();
            listEnemies[0].ActionStopCombat();

            yield return combatTransition;

            if (listPlayers[0].IsAlive) // TODO Mariano: Si hay algun PLAYER vivo
            {
                isTurnPlayer = true;
                listPlayers[0].SetDefense(0); // TODO Mariano: DELETE

            }
            else
            {
                yield return evaluationDuration;

                EndGame(false);
            }

        }
        else
        {
            yield return evaluationDuration;

            EndGame(true);
        }
    }

    //------------------------------------

    public void EndGame(bool isWin)
    {
        // TODO Mariano: FADE

        uIController.endTxt.text = isWin ? GameData.Instance.textConfig.gameWinTxt : GameData.Instance.textConfig.gameLoseTxt;

        uIController.endTxt.gameObject.SetActive(true);

        uIController.endTxt.transform.
        DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 1, 5, .5f).
        SetEase(Ease.OutQuad);
    }
}