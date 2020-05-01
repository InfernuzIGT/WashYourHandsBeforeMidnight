using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Ambients")]
    public AMBIENT currentAmbient;

    [Header("References")]
    public GlobalController globalController;
    public CombatManager combatManager;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;

    public Dictionary<int, QuestSO> dictionaryQuest;
    public Dictionary<int, int> dictionaryProgress;

    private AMBIENT _lastAmbient;
    private NPCSO _currentNPC;

    private WaitForSeconds _waitDeactivateUI;

    private FadeEvent _fadeEvent;

    private void Start()
    {
        dictionaryQuest = new Dictionary<int, QuestSO>();
        dictionaryProgress = new Dictionary<int, int>();
        _waitDeactivateUI = new WaitForSeconds(GameData.Instance.gameConfig.messageLifetime);

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = false;
        _fadeEvent.callbackStart = SwitchAmbient;

        SwitchAmbient();
    }

    private void OnEnable()
    {
        EventController.AddListener<TriggerCombatEvent>(OnTriggerCombat);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<TriggerCombatEvent>(OnTriggerCombat);
    }

    private void SwitchAmbient()
    {
        switch (currentAmbient)
        {
            case AMBIENT.World:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                globalController.player.ChangeMovement(true);
                // TODO Mariano: Change camera
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

                globalController.player.ChangeMovement(false);
                // TODO Mariano: Change camera

                combatManager.SetCombat();
                break;

            case AMBIENT.Development:
                // Nothing

                Debug.Log($"<color=yellow><b>[DEV]  </b></color> Switch Ambient!");
                break;

            default:
                break;
        }
    }

    // private void SwitchCamera()
    // {
    //     cameras[(int)_lastAmbient].SetActive(false);
    //     cameras[(int)currentAmbient].SetActive(true);
    // }

    #region Events

    public void OnTriggerCombat(TriggerCombatEvent evt)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = AMBIENT.Combat;

        _currentNPC = evt.npc;

        EventController.TriggerEvent(_fadeEvent);
    }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {
        dictionaryQuest.Add(data.id, data);
    }

    public void ProgressQuest(QuestSO quest)
    {
        dictionaryProgress[quest.id]++;

        if (dictionaryProgress[quest.id] == dictionaryQuest[quest.id].objetives.Length)
        {
            // TODO Mariano: Dar recompensa del QuestSO
            Complete();
        }
        else
        {
            worldUI.UpdateObjectives(dictionaryQuest[quest.id].objetives[dictionaryProgress[quest.id]], dictionaryProgress[quest.id]);
        }
    }

    // when quest is reached delete quest from log of quest
    // Make UI follow quests superior corner left 

    public void Complete()
    {
        // TODO Mariano: Tachar titulo del diario
        // worldUI.questObjectives.text = "";
        // worldUI.questComplete.SetActive(true);
        worldUI.questTitleDiaryTxt.alpha = 0;

        StartCoroutine(DeactivateWorldUI());
    }

    private IEnumerator DeactivateWorldUI()
    {
        yield return _waitDeactivateUI;

        worldUI.questComplete.SetActive(false);
    }

    #endregion

}