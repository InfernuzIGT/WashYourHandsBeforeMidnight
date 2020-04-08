using Events;
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

    [Header("Cameras")]
    public GameObject[] cameras;

    [Header("Characters")]
    public PlayerController player;

    private AMBIENT _lastAmbient;
    private Camera _cameraMain;

    private FadeEvent _fadeEvent;

    private void Start()
    {
        _cameraMain = Camera.main;

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = false;
        _fadeEvent.callbackStart = SetAmbient;

        StartGame();
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
        cameras[(int)_lastAmbient].SetActive(false);
        cameras[(int)currentAmbient].SetActive(true);
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
}