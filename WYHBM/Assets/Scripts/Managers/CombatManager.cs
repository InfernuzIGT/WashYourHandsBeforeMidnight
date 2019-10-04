using System.Collections.Generic;
using Events;
using GameMode.Combat;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Controllers")]
    public UIController uiController;
    public ActionController actionController;

    [Header("General")]
    public bool isPaused = false;
    public bool isTurnPlayer = true;
    
    [Header ("Fade")]
    public float fadeDuration = 3;
    public string fadeInText = "COMBAT!";
    public string fadeOutText = "FINISH";

    [Header("Characters")]
    public Transform groupPlayers;
    public Transform groupEnemies;
    [Space]
    public List<Player> listPlayers = new List<Player>();
    [Space]
    public List<Enemy> listEnemies = new List<Enemy>();

    private FadeInEvent fadeInEvent = new FadeInEvent();
    private FadeOutEvent fadeOutEvent = new FadeOutEvent();

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
        
        fadeOutEvent.duration = 3;
        fadeInEvent.text = fadeInText;        
        fadeOutEvent.text = fadeOutText;        
        
        FadeOut();
    }

    private void GetCharacters()
    {
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
    
}