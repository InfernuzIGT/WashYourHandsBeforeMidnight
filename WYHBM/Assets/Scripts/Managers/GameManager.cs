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
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;

    [Header("Combat")]
    public List<Player> combatCharacters;

    public Dictionary<int, QuestSO>     dictionaryQuest;
    public Dictionary<int, int> dictionaryProgress;

    private AMBIENT _lastAmbient;

    private WaitForSeconds _waitDeactivateUI;

    private FadeEvent _fadeEvent;

    private void Start()
    {
        dictionaryQuest = new Dictionary<int, QuestSO>();
        dictionaryProgress = new Dictionary<int, int>();
        _waitDeactivateUI = new WaitForSeconds(seconds: GameData.Instance.gameConfig.messageLifetime);

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = true;
        _fadeEvent.callbackStart = SwitchMovement;
        _fadeEvent.callbackMid = SwitchAmbient;
        // _fadeEvent.callbackEnd = InitiateTurn;

        SwitchAmbient();
    }

    public Vector3 GetPlayerFootPosition()
    {
        return globalController.player.gameObject.transform.position - GameData.Instance.gameConfig.playerBaseOffset;
    }

    public Ray GetRayMouse()
    {
        return globalController.mainCamera.ScreenPointToRay(Input.mousePosition);
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

                globalController.ChangeCamera(null);
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

                globalController.ChangeCamera(combatManager.SetCamera());

                combatManager.InitiateTurn();
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

    public void OnTriggerCombat(TriggerCombatEvent evt)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = AMBIENT.Combat;

        combatManager.SetData(combatCharacters, evt.npc.combatCharacters);

        EventController.TriggerEvent(_fadeEvent);
    }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {
        if (!dictionaryQuest.ContainsKey(data.id))
        {
            dictionaryQuest.Add(data.id, data);

            dictionaryProgress.Add(data.id, data.id);
        }
    }

    public void ProgressQuest(QuestSO quest)
    {
        dictionaryProgress[quest.id]++;

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
        // TODO Mariano: Tachar titulo del diario
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

}