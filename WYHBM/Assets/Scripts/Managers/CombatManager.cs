using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using GameMode.Combat;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Controllers")]
    public UIController uIController;
    public ActionController actionController;

    [Header("General")]
    public bool isPaused = false;
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

    private FadeInEvent fadeInEvent = new FadeInEvent();
    private FadeOutEvent fadeOutEvent = new FadeOutEvent();
    private FadeInCanvasEvent fadeInCanvasEvent = new FadeInCanvasEvent();
    private FadeOutCanvasEvent fadeOutCanvasEvent = new FadeOutCanvasEvent();
    private FadeOutCanvasEvent fadeOutEndEvent = new FadeOutCanvasEvent();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        GetCharacters();

        combatTransition = new WaitForSeconds(GameData.Instance.combatConfig.transitionDuration);
        combatWaitTime = new WaitForSeconds(GameData.Instance.combatConfig.waitCombatDuration);
        evaluationDuration = new WaitForSeconds(GameData.Instance.combatConfig.evaluationDuration);

        fadeInEvent.duration = 3;
        fadeOutEvent.duration = 3;
        fadeInEvent.text = GameData.Instance.textConfig.fadeInText;
        fadeOutEvent.text = GameData.Instance.textConfig.fadeOutText;

        fadeInCanvasEvent.duration = GameData.Instance.combatConfig.transitionDuration;
        fadeOutCanvasEvent.duration = GameData.Instance.combatConfig.transitionDuration;

        fadeOutEndEvent.duration = GameData.Instance.combatConfig.fadeOutEndDuration;

        FadeOut();
    }

    private void GetCharacters()
    {
        // TODO Mariano: REDO THIS!

        for (int i = 0; i < groupPlayers.childCount; i++)
            listPlayers.Add(groupPlayers.GetChild(i).GetComponent<Player>());

        for (int i = 0; i < groupEnemies.childCount; i++)
            listEnemies.Add(groupEnemies.GetChild(i).GetComponent<Enemy>());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            uIController.Menu(isPaused);
        }

    }

    public void StartCombat()
    {
        _isActionEnable = !isPaused && isTurnPlayer;

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
            FadeOutCanvas();

            // TODO Mariano: Fade de los personajes involucrados
            listPlayers[0].ActionStartCombat();
            listEnemies[0].ActionStartCombat();

            yield return combatTransition;

            actionController.PlayAction(players);
            uIController.ChangeUI(!listEnemies[0].IsAlive); // Need FALSE

            yield return combatWaitTime;

            // TODO Mariano: Fade OUT + BLUR
            FadeInCanvas();

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
            FadeOutCanvas();

            // TODO Mariano: Fade de los personajes involucrados
            listPlayers[0].ActionStartCombat();
            listEnemies[0].ActionStartCombat();

            yield return combatTransition;

            actionController.PlayAction(enemies);
            uIController.ChangeUI(listPlayers[0].IsAlive); // Need TRUE

            yield return combatWaitTime;

            // TODO Mariano: Fade OUT + BLUR
            FadeInCanvas();

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
        EventController.TriggerEvent(fadeOutEndEvent);
        EventController.TriggerEvent(fadeInEvent);

        uIController.endTxt.text = isWin ? GameData.Instance.textConfig.gameWinTxt : GameData.Instance.textConfig.gameLoseTxt;

        uIController.endTxt.gameObject.SetActive(true);

        uIController.endTxt.transform.
        DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 1, 5, .5f).
        SetEase(Ease.OutQuad);
    }

    #region Fade

    public void FadeIn()
    {
        EventController.TriggerEvent(fadeInEvent);
    }

    public void FadeOut()
    {
        EventController.TriggerEvent(fadeOutEvent);
    }

    public void FadeInCanvas()
    {
        EventController.TriggerEvent(fadeInCanvasEvent);
    }

    public void FadeOutCanvas()
    {
        EventController.TriggerEvent(fadeOutCanvasEvent);
    }

    #endregion

}