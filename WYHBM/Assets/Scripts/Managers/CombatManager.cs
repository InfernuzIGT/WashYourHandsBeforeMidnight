using System.Collections.Generic;
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

    [Header("Characters")]
    public Transform groupPlayers;
    public Transform groupEnemies;
    [Space]
    public List<Player> listPlayers = new List<Player>();
    [Space]
    public List<Enemy> listEnemies = new List<Enemy>();

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
    }

    private void GetCharacters()
    {
        for (int i = 0; i < groupPlayers.childCount; i++)
            listPlayers.Add(groupPlayers.GetChild(i).GetComponent<Player>());
        
        for (int i = 0; i < groupEnemies.childCount; i++)
            listEnemies.Add(groupEnemies.GetChild(i).GetComponent<Enemy>());
    }
}