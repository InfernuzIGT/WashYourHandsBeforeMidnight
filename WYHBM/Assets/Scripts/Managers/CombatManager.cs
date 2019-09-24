using UnityEngine;
using GameMode.Combat;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Controllers")]
    public UIController uiController;
    public ActionController actionController;

    [Header("General")]
    public bool isPaused = false; 
    
    [Header ("Turn")]
    public bool isTurnPlayer = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}