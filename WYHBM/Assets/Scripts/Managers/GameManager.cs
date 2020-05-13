using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Ambients")]
    public AMBIENT currentAmbient;

    [Header("References")]
    public GlobalController globalController;
    public CombatManager combatManager;
    public Inventory inventoryManager; // TODO Mariano: Add
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;

    [Header("Combat")]
    public CombatArea[] combatAreas;
    [Space]
    public List<Player> combatCharacters;

    public Dictionary<int, QuestSO> dictionaryQuest;
    public Dictionary<int, int> dictionaryProgress;

    // Combat
    private CombatArea _currentCombatArea;
    private NPCController currentNPC;

    private AMBIENT _lastAmbient;

    private WaitForSeconds _waitDeactivateUI;

    // Events
    private FadeEvent _fadeEvent;

    private void Start()
    {
        dictionaryQuest = new Dictionary<int, QuestSO>();
        dictionaryProgress = new Dictionary<int, int>();
        _waitDeactivateUI = new WaitForSeconds(seconds: GameData.Instance.gameConfig.messageLifetime);

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = true;
    }

    private void OnEnable()
    {
        EventController.AddListener<EnterCombatEvent>(OnEnterCombat);
        EventController.AddListener<ExitCombatEvent>(OnExitCombat);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnterCombatEvent>(OnEnterCombat);
        EventController.RemoveListener<ExitCombatEvent>(OnExitCombat);
    }

    private void SwitchAmbient()
    {
        switch (currentAmbient)
        {
            case AMBIENT.World:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                globalController.ChangeCamera(null);
                combatManager.CloseCombatArea();
                break;

                // case AMBIENT.Interior:
                //     worldUI.EnableCanvas(true);
                //     combatUI.EnableCanvas(false);

                //     player.ChangeMovement(true);
                //     break;

                // case AMBIENT.Location:
                //     worldUI.EnableCanvas(true);
                //     combatUI.EnableCanvas(false);

                //     player.ChangeMovement(true);
                //     break;

            case AMBIENT.Combat:
                worldUI.EnableCanvas(false);
                combatUI.EnableCanvas(true);

                globalController.ChangeCamera(_currentCombatArea.virtualCamera);
                break;

            case AMBIENT.Development:
                // Nothing

                Debug.Log($"<color=yellow><b>[DEV]  </b></color> Switch Ambient!");
                break;

            default:
                break;
        }
    }

    private void SwitchMovement()
    {
        globalController.player.SwitchMovement();
    }

    private void InitiateTurn()
    {
        combatManager.InitiateTurn();
    }

    #region Events

    public void OnEnterCombat(EnterCombatEvent evt)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = AMBIENT.Combat;
        currentNPC = evt.currentNPC;

        int indexArea = Random.Range(0, combatAreas.Length);
        _currentCombatArea = combatAreas[indexArea];
        combatManager.SetData(_currentCombatArea, combatCharacters, evt.npc.combatCharacters);

        _fadeEvent.callbackStart = SwitchMovement;
        _fadeEvent.callbackMid = SwitchAmbient;
        _fadeEvent.callbackEnd = StartCombat;

        EventController.TriggerEvent(_fadeEvent);
    }

    public void OnExitCombat(ExitCombatEvent evt)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = AMBIENT.World;

        _fadeEvent.callbackStart = null;
        _fadeEvent.callbackMid = SwitchAmbient;
        _fadeEvent.callbackEnd = SwitchMovement;

        EventController.TriggerEvent(_fadeEvent);
    }

    private void StartCombat()
    {
        currentNPC.Kill();
        combatManager.InitiateTurn();
    }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {

        if (!dictionaryQuest.ContainsKey(data.id))
        {
            dictionaryQuest.Add(data.id, data);

            dictionaryProgress.Add(data.id, 0);
        }
    }

    public void ProgressQuest(QuestSO quest)
    {
        if (dictionaryProgress[quest.id] >= dictionaryQuest[quest.id].objetives.Length)
        {
            return;

        }

        dictionaryProgress[quest.id]++;
        Debug.Log($"<b> Progress {dictionaryProgress[quest.id]} - quest{dictionaryQuest[quest.id].objetives.Length} </b> ");

        if (dictionaryProgress[quest.id] == dictionaryQuest[quest.id].objetives.Length)
        {
            Complete();
        }
        else
        {
            worldUI.UpdateObjectives(dictionaryQuest[quest.id].objetives[dictionaryProgress[quest.id]], dictionaryProgress[quest.id]);
        }
    }

    public void Complete()
    {
        worldUI.questTitleDiaryTxt.fontStyle = FontStyles.Strikethrough;
        worldUI.questTitleDiaryTxt.color = Color.grey;
        worldUI.questDescriptionTxt.fontStyle = FontStyles.Strikethrough;
        worldUI.questDescriptionTxt.color = Color.grey;

        worldUI.questComplete.SetActive(true);

        worldUI.questCompleteTxt.text = worldUI.questTitleTxt.text;
        worldUI.questCompleteTxt.fontStyle = FontStyles.Strikethrough;

        StartCoroutine(DeactivateWorldUI());
    }

    public void GiveReward()
    {
        // TODO Mariano: Dar recompensa del QuestSO
        // Instantiate item in inventory
    }

    public IEnumerator DeactivateWorldUI()
    {
        yield return _waitDeactivateUI;

        worldUI.questComplete.SetActive(false);
        worldUI.questPopup.SetActive(false);
    }

    #endregion

    #region Other

    public Vector3 GetPlayerFootPosition()
    {
        return globalController.player.gameObject.transform.position - GameData.Instance.gameConfig.playerBaseOffset;
    }

    public Ray GetRayMouse()
    {
        return globalController.mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    #endregion

}