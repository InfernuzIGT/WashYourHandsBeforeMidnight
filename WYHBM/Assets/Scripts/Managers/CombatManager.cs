using System.Collections.Generic;
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

    private FadeInEvent fadeInEvent = new FadeInEvent();
    private FadeOutEvent fadeOutEvent = new FadeOutEvent();
    private FadeInCanvasEvent fadeInCanvasEvent = new FadeInCanvasEvent();
    private FadeOutCanvasEvent fadeOutCanvasEvent = new FadeOutCanvasEvent();

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

        fadeInEvent.duration = 3;
        fadeOutEvent.duration = 3;
        fadeInEvent.text = GameData.Instance.textConfig.fadeInText;
        fadeOutEvent.text = GameData.Instance.textConfig.fadeOutText;

        fadeInCanvasEvent.duration = GameData.Instance.combatConfig.transitionDuration;
        fadeOutCanvasEvent.duration = GameData.Instance.combatConfig.transitionDuration;

        FadeOut();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void GetCharacters()
    {
        // TODO Mariano: REDO THIS!

        for (int i = 0; i < groupPlayers.childCount; i++)
            listPlayers.Add(groupPlayers.GetChild(i).GetComponent<Player>());

        for (int i = 0; i < groupEnemies.childCount; i++)
            listEnemies.Add(groupEnemies.GetChild(i).GetComponent<Enemy>());
    }

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

}