using DG.Tweening;
using Events;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header ("Ambients")]
    public AMBIENT currentAmbient;
    public GameObject currentInterior;
    [Space]
    public CombatManager combatManager;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;

    [Header ("Quest")]
    public QuestSO quest;

    private int _currentAmount;

    [Header ("Cameras")]
    public GameObject[] cameras;

    [Header ("Characters")]
    public PlayerController player;

    private AMBIENT _lastAmbient;
    private Camera _cameraMain;

    private FadeEvent _fadeEvent;

    private void Start ()
    {
        _cameraMain = Camera.main;

        _fadeEvent = new FadeEvent ();
        _fadeEvent.fadeFast = false;
        _fadeEvent.callbackStart = SetAmbient;

        StartGame ();
    }

    private void OnEnable ()
    {
        EventController.AddListener<CreateInteriorEvent> (OnCreateInterior);
    }

    private void OnDisable ()
    {
        EventController.RemoveListener<CreateInteriorEvent> (OnCreateInterior);
    }

    private void StartGame ()
    {
        SwitchAmbient ();
        // SwitchCamera();
    }

    public void ChangeAmbient (AMBIENT newAmbient)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = newAmbient;

        player.ChangeMovement (false);
    }

    private void SetAmbient ()
    {
        SwitchAmbient ();
        // SwitchCamera();
    }

    private void SwitchAmbient ()
    {
        switch (currentAmbient)
        {
            case AMBIENT.World:
                worldUI.EnableCanvas (true);
                combatUI.EnableCanvas (false);

                player.ChangeMovement (true);
                break;

            case AMBIENT.Interior:
                worldUI.EnableCanvas (true);
                combatUI.EnableCanvas (false);

                player.ChangeMovement (true);
                break;

            case AMBIENT.Location:
                worldUI.EnableCanvas (true);
                combatUI.EnableCanvas (false);

                player.ChangeMovement (true);
                break;

            case AMBIENT.Combat:
                worldUI.EnableCanvas (false);
                combatUI.EnableCanvas (true);

                combatUI.selectableButton.Select (); // TODO Mariano: REMOVE

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

    private void SwitchCamera ()
    {
        cameras[(int) _lastAmbient].SetActive (false);
        cameras[(int) currentAmbient].SetActive (true);
    }

    #region Events

    private void OnCreateInterior (CreateInteriorEvent evt)
    {
        if (evt.isCreating)
        {
            currentInterior = Instantiate (evt.newInterior, GameData.Instance.gameConfig.interiorPosition, Quaternion.identity);
        }
        else
        {
            Destroy (currentInterior, 1);
        }
    }

    #endregion

    #region Quest
    public void UpdateType (GOAL_TYPE goalType)
    {

        switch (goalType)
        {
            case GOAL_TYPE.kill:
                _currentAmount++;
                // Add executes when kill an objective
                break;

            case GOAL_TYPE.collect:
                _currentAmount++;
                break;

            case GOAL_TYPE.interact:
                _currentAmount++;
                break;
        }
    }

    public void ProgressQuest ()
    {
        _currentAmount++;

        worldUI.SetObjectives (_currentAmount.ToString (), quest.requiredAmount.ToString ());

        if (_currentAmount == quest.requiredAmount)
        {
            Complete ();

        }
    }

    // when quest is reached delete quest from log of quest
    // Make UI follow quests superior corner left 

    public void Complete ()
    {
        worldUI.questObjectives.text = "";
        worldUI.questComplete.SetActive (true);
        player.RemoveQuest (); // TODO Mariano: REVIEW
        Debug.Log ($"<b> was completed! </b>");
        Invoke ("TurnOffComplete", 3);
    }

    private void TurnOffComplete ()
    {
        worldUI.questComplete.SetActive (false);

    }
    #endregion

}