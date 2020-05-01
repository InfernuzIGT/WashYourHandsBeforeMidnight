using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using GameMode.Combat;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Mariano")]
    public CombatArea[] combatAreas;

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
    private WaitForSeconds combatTransition;
    private WaitForSeconds combatWaitTime;
    private WaitForSeconds evaluationDuration;

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

    public void SetCombat()
    {
        Debug.Log($"<b> SPAWN ENEMIES! </b>");
        
        // TODO Mariano: Spawn Enemies usign _currentNPC
        // TODO Mariano: Wait X seconds, and StartCombat!
    }

    // Start Combat
    public void InitiateTurn()
    {
        AddToWaiting(InitialSort());
        SetInitialCharactersTurn();

        StartCoroutine(TurnsLoop());
        UnleashRace();
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

    public List<CombatCharacter> InitialSort()
    {
        List<CombatCharacter> sortedCharacters = new List<CombatCharacter>();
        sortedCharacters.AddRange(listCharacter);

        // CombatCharacter[] sortedCharacters = new CombatCharacter[characters.Length];
        // sortedCharacters = characters;

        CombatCharacter fastestCharacter;
        fastestCharacter = sortedCharacters[0];

        for (int i = 0; i < sortedCharacters.Count; i++) // ONE MINUS
        {
            for (int j = 0; j < sortedCharacters.Count; j++) // ONE MINUS
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
            waitingForAction.Add(charactersToAdd[i]);
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

    //-----------------------------------------------------------
    //-----------------------------------------------------------
    //-----------------------------------------------------------

    private void Start()
    {
        GetCharacters();

        combatTransition = new WaitForSeconds(GameData.Instance.combatConfig.transitionDuration);
        combatWaitTime = new WaitForSeconds(GameData.Instance.combatConfig.waitCombatDuration);
        evaluationDuration = new WaitForSeconds(GameData.Instance.combatConfig.evaluationDuration);
    }

    private void GetCharacters()
    {
        // TODO Mariano: Spawn Players and Enemies
    }

    public void StartCombat()
    {
        _isActionEnable = isTurnPlayer;

        if (!_isActionEnable)
            return;

        isTurnPlayer = false;

        StartCoroutine(Combat(listPlayers));
    }

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