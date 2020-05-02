using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Ambients")]
    public AMBIENT currentAmbient;
    public GameObject currentInterior;
    [Space]
    public CombatManager combatManager;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;

    [Header("Quest")]
    public Dictionary<int, QuestSO> dictionaryQuest;
    public Dictionary<int, int> dictionaryProgress;

    [Header("Cameras")]
    public GameObject[] cameras;

    [Header("Characters")]
    public PlayerController player;

    private AMBIENT _lastAmbient;
    private Camera _cameraMain;

    private WaitForSeconds _waitDeactivateUI;

    private FadeEvent _fadeEvent;

    private void Start()
    {
        _cameraMain = Camera.main;
        dictionaryQuest = new Dictionary<int, QuestSO>();
        dictionaryProgress = new Dictionary<int, int>();
        _waitDeactivateUI = new WaitForSeconds(seconds: GameData.Instance.gameConfig.messageLifetime);

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = false;
        _fadeEvent.callbackStart = SetAmbient;

        StartGame();
    }

    public Vector3 GetPlayerFootPosition()
    {
        Vector3 pos = new Vector3(player.gameObject.transform.position.x, GameData.Instance.gameConfig.samFoots, player.gameObject.transform.position.z);
        
        return pos;

    }



    private void OnEnable()
    {
        EventController.AddListener<CreateInteriorEvent>(OnCreateInterior);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<CreateInteriorEvent>(OnCreateInterior);
    }

    private void StartGame()
    {
        SwitchAmbient();
        // SwitchCamera();
    }

    public void ChangeAmbient(AMBIENT newAmbient)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = newAmbient;

        player.ChangeMovement(false);
    }

    private void SetAmbient()
    {
        SwitchAmbient();
        // SwitchCamera();
    }

    private void SwitchAmbient()
    {
        switch (currentAmbient)
        {
            case AMBIENT.World:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case AMBIENT.Interior:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case AMBIENT.Location:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case AMBIENT.Combat:
                worldUI.EnableCanvas(false);
                combatUI.EnableCanvas(true);

                combatUI.selectableButton.Select(); // TODO Mariano: REMOVE

                // TODO Mariano: Save Player Position
                // TODO Mariano: Move Player to Combat Zone
                // TODO Mariano: Spawn Enemies
                // TODO Mariano: Wait X seconds, and StartCombat!
                break;

            case AMBIENT.Development:
                // Nothing
                break;

            default:
                break;
        }
    }

    private void SwitchCamera()
    {
        cameras[(int) _lastAmbient].SetActive(false);
        cameras[(int) currentAmbient].SetActive(true);
    }

    #region Events

    private void OnCreateInterior(CreateInteriorEvent evt)
    {
        if (evt.isCreating)
        {
            currentInterior = Instantiate(evt.newInterior, GameData.Instance.gameConfig.interiorPosition, Quaternion.identity);
        }
        else
        {
            Destroy(currentInterior, 1);
        }
    }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {
        if (!dictionaryQuest.ContainsKey(data.id))
        {
            dictionaryQuest.Add(data.id, data);

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