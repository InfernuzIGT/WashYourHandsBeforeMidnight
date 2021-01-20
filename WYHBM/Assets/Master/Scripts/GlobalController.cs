using System.Collections;
using Cinemachine;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class Quest
{
    public QuestSO data;
    public int currentStep = 0;
}

[RequireComponent(typeof(Volume), typeof(PlayableDirector))]
public class GlobalController : MonoBehaviour
{
    [Header("General")]
    public PlayerSO playerData;
    [Space]
    public bool isPaused;
    public bool inCombat;
    [Space]
    public SessionData sessionData;

    private bool skipEncounters = true;
    // public ItemSO[] items;

    [Header("References")]
    [SerializeField] private bool ShowReferences = true;
    [ConditionalHide] public WorldConfig worldConfig;
    [ConditionalHide] public Camera mainCamera;
    [ConditionalHide] public CinemachineFreeLookUtility playerCamera;
    [Space]
    [ConditionalHide] public GameData gameData;
    [ConditionalHide] public PlayerController playerController;
    [Space]
    [ConditionalHide] public PlayableDirector playableDirector;
    [ConditionalHide] public GameMode.World.UIManager worldUI;
    [ConditionalHide] public GameMode.Combat.UIManager combatUI;
    [ConditionalHide] public Fade fadeUI;
    [ConditionalHide] public EventSystemUtility eventSystemUtility;
    [Space]
    [ConditionalHide] public Material materialFOV;
    [ConditionalHide] public Material materialDitherNPC;

    // Shaders
    private int hash_IsVisible = Shader.PropertyToID("_IsVisible");

    // PostProcess
    private ColorAdjustments _ppColorAdjustments;
    private LensDistortion _ppLensDistortion;
    private DepthOfField _ppDepthOfField;
    private Vignette _ppVignette;
    private Color _colorVigneteInactive = new Color(0.025f, 0, 0.25f, 1);
    private Color _colorVigneteActive = new Color(0.1f, 0.1f, 0.1f, 1);

    private CinemachineVirtualCamera _worldCamera;
    private CinemachineVirtualCamera _combatCamera;
    private Coroutine _coroutineListenMode;
    private bool _isInteriorCamera;
    private bool _fovIsActive;
    private float _fovCurrentTime = 0;

    private EnableMovementEvent _enableMovementEvent;
    private CutsceneEvent _cutsceneEvent;
    private EnableDialogEvent _interactionDialogEvent;

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Persistent"));

        CheckGameData();

        _enableMovementEvent = new EnableMovementEvent();

        _interactionDialogEvent = new EnableDialogEvent();
        _interactionDialogEvent.enable = false;

        _cutsceneEvent = new CutsceneEvent();
        _cutsceneEvent.show = false;

        SpawnPlayer();
        SpawnUI();

        CheckCamera();
        // AddItems();

        playableDirector.stopped += OnCutsceneStop;

        materialFOV.SetFloat(hash_IsVisible, 0);
        materialDitherNPC.SetFloat(hash_IsVisible, 0);

#if UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        worldUI.gameObject.name = "Canvas (World)";
        combatUI.gameObject.name = "Canvas (Combat)";
        fadeUI.gameObject.name = "Canvas (Fade)";
        eventSystemUtility.gameObject.name = "Event System";
#else
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif

    }

    private void OnEnable()
    {
        EventController.AddListener<EnableDialogEvent>(OnEnableDialog);
        EventController.AddListener<QuestEvent>(OnQuest);
        EventController.AddListener<ChangeInputEvent>(OnChangeInput);
        EventController.AddListener<SessionEvent>(OnSession);
        EventController.AddListener<CutsceneEvent>(OnCutscene);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);
        EventController.RemoveListener<QuestEvent>(OnQuest);
        EventController.RemoveListener<ChangeInputEvent>(OnChangeInput);
        EventController.RemoveListener<SessionEvent>(OnSession);
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
    }

    private void CheckGameData()
    {
        GameData tempGamedata = GameObject.FindObjectOfType<GameData>();

        gameData = tempGamedata != null ? tempGamedata : Instantiate(gameData);

        sessionData = gameData.LoadSession();
    }

    private void SpawnPlayer()
    {
        SpawnPoint spawnPoint = GameObject.FindObjectOfType<SpawnPoint>();

        if (spawnPoint != null)
        {
            playerController = Instantiate(playerController, spawnPoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't find a Spawn Point!");

            playerController = Instantiate(playerController);
        }

        playerController.SetPlayerData(playerData);

        sessionData.playerData = playerData;

        playerController.Input.Player.ListenMode.started += ctx => ListenMode(true);
        playerController.Input.Player.ListenMode.canceled += ctx => ListenMode(false);
    }

    private void ListenMode(bool active)
    {
        _fovIsActive = active;

        if (!playerController.IsCrouching)playerController.Crouch();

        if (_coroutineListenMode != null)
        {
            StopCoroutine(_coroutineListenMode);
            _coroutineListenMode = null;
        }

        _coroutineListenMode = StartCoroutine(ChangeListenMode());
    }

    private IEnumerator ChangeListenMode()
    {
        if (_fovIsActive)
        {
            while (_fovCurrentTime < worldConfig.fovTime)
            {
                SetValuesListenMode();

                _fovCurrentTime += Time.deltaTime;

                yield return null;
            }
        }
        else
        {
            while (_fovCurrentTime > 0)
            {
                SetValuesListenMode();

                _fovCurrentTime -= Time.deltaTime;

                yield return null;
            }
        }

        _fovCurrentTime = _fovIsActive ? worldConfig.fovTime : 0;
    }

    private void SetValuesListenMode()
    {
        _ppColorAdjustments.saturation.value = Mathf.Lerp(0, -50, (_fovCurrentTime / worldConfig.fovTime));
        _ppLensDistortion.intensity.value = Mathf.Lerp(0, 0.15f, (_fovCurrentTime / worldConfig.fovTime));
        _ppDepthOfField.gaussianStart.value = Mathf.Lerp(22.5f, 20, (_fovCurrentTime / worldConfig.fovTime));
        _ppDepthOfField.gaussianEnd.value = Mathf.Lerp(60, 22.5f, (_fovCurrentTime / worldConfig.fovTime));
        _ppVignette.intensity.value = Mathf.Lerp(0.2f, 0.5f, (_fovCurrentTime / worldConfig.fovTime));
        _ppVignette.smoothness.value = Mathf.Lerp(1, 0.5f, (_fovCurrentTime / worldConfig.fovTime));

        _ppVignette.color.value = Color.Lerp(_colorVigneteInactive, _colorVigneteActive, (_fovCurrentTime / worldConfig.fovTime));

        playerCamera.SetFOV(Mathf.Lerp(40, 45, (_fovCurrentTime / worldConfig.fovTime)));

        materialFOV.SetFloat(hash_IsVisible, Mathf.Lerp(0, 0.35f, (_fovCurrentTime / worldConfig.fovTime)));
        materialDitherNPC.SetFloat(hash_IsVisible, Mathf.Lerp(0, 0.15f, (_fovCurrentTime / worldConfig.fovTime)));
    }

    private void CheckCamera()
    {
        playerCamera.Init(playerController, eventSystemUtility.InputUIModule);

        DetectTargetBehind detectTargetBehind = mainCamera.GetComponent<DetectTargetBehind>();
        detectTargetBehind.SetTarget(playerController.transform);

        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet(out _ppColorAdjustments);
        volume.profile.TryGet(out _ppLensDistortion);
        volume.profile.TryGet(out _ppVignette);
        volume.profile.TryGet(out _ppDepthOfField);
    }

    private void SpawnUI()
    {
        fadeUI = Instantiate(fadeUI);
        eventSystemUtility = Instantiate(eventSystemUtility);

        worldUI = Instantiate(worldUI);
        combatUI = Instantiate(combatUI);

        worldUI.Show(!inCombat);
        combatUI.Show(inCombat);
    }

    // public void ChangeWorldCamera()
    // {
    //     _isInteriorCamera = !_isInteriorCamera;

    //     _worldCamera = _isInteriorCamera ? interiorCamera : exteriorCamera;

    //     exteriorCamera.gameObject.SetActive(!_isInteriorCamera);
    //     interiorCamera.gameObject.SetActive(_isInteriorCamera);
    // }

    public void ChangeToCombatCamera(CinemachineVirtualCamera combatCamera)
    {
        if (combatCamera == null)
        {
            _combatCamera.gameObject.SetActive(false);
            _worldCamera.gameObject.SetActive(true);
            _combatCamera = null;
        }
        else
        {
            combatCamera.gameObject.SetActive(true);
            _worldCamera.gameObject.SetActive(false);
            _combatCamera = combatCamera;
        }
    }

    private void OnCutsceneStop(PlayableDirector pd)
    {
        EnableMovement(true);

        EventController.TriggerEvent(_cutsceneEvent);
    }

    // private void AddItems()
    // {
    //     if (items.Length != 0)
    //     {
    //         for (int i = 0; i < items.Length; i++)
    //         {
    //             AddItem(i);
    //         }
    //     }
    // }

    // private void AddItem(int index)
    // {
    //     if (GameManager.Instance.IsInventoryFull)
    //     {
    //         GameManager.Instance.worldUI.ShowPopup(GameData.Instance.textConfig.popupInventoryFull);
    //         return;
    //     }

    //     Slot newSlot = Instantiate(GameData.Instance.worldConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
    //     newSlot.AddItem(items[index]);
    // }

    public bool GetPlayerInMovement()
    {
        return playerController.GetPlayerInMovement() && !skipEncounters;
    }

    public void HidePlayer(bool isHiding)
    {
        playerController.gameObject.SetActive(!isHiding);
    }

    private void EnableMovement(bool enable)
    {
        _enableMovementEvent.canMove = enable;
        EventController.TriggerEvent(_enableMovementEvent);
    }

    private void CompleteQuest(int index)
    {
        sessionData.listQuest[index].currentStep = sessionData.listQuest[index].data.steps;
    }

    #region Events

    private void OnEnableDialog(EnableDialogEvent evt)
    {
        if (evt.enable)
        {
            EventController.AddListener<InteractionEvent>(OnInteractionDialog);
        }
        else
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);
            EnableMovement(true);
        }
    }

    private void OnInteractionDialog(InteractionEvent evt)
    {
        if (!evt.isStart)return;

        EnableMovement(false);
    }

    private void OnChangeInput(ChangeInputEvent evt)
    {
        EnableMovement(evt.enable);
    }

    private void OnCutscene(CutsceneEvent evt)
    {
        playableDirector.playableAsset = evt.cutscene;
        playableDirector.Play();
    }

    private void OnSession(SessionEvent evt)
    {
        if (gameData == null)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> GameData NULL");
            return;
        }

        switch (evt.option)
        {
            case SESSION_OPTION.Save:
                gameData.SaveSession(sessionData);
                break;

            case SESSION_OPTION.Load:
                sessionData = gameData.LoadSession();
                break;

            case SESSION_OPTION.Delete:
                gameData.DeleteAll();
                break;
        }
    }

    private void OnQuest(QuestEvent evt)
    {
        switch (evt.state)
        {
            case QUEST_STATE.New:
                for (int i = 0; i < sessionData.listQuest.Count; i++)
                {
                    if (sessionData.listQuest[i].data = evt.data)return;
                }

                Quest newQuest = new Quest();
                newQuest.data = evt.data;
                newQuest.currentStep = 0;

                sessionData.listQuest.Add(newQuest);

                gameData.SaveSession(sessionData);
                return;

            case QUEST_STATE.Update:
                for (int i = 0; i < sessionData.listQuest.Count; i++)
                {
                    if (sessionData.listQuest[i].data = evt.data)
                    {
                        sessionData.listQuest[i].currentStep++;

                        if (sessionData.listQuest[i].currentStep >= evt.data.steps)CompleteQuest(i);

                        return;
                    }
                }
                return;

            case QUEST_STATE.Complete:
                for (int i = 0; i < sessionData.listQuest.Count; i++)
                {
                    if (sessionData.listQuest[i].data = evt.data)
                    {
                        CompleteQuest(i);
                        return;
                    }
                }
                return;
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> OnQuest fail! Quest: {evt.data.name}, State: {evt.state}");
    }

    #endregion

    #region  Editor

#if UNITY_EDITOR

    public void EditorSave()
    {
        SessionEvent sessionEvent = new SessionEvent();
        sessionEvent.option = SESSION_OPTION.Save;
        EventController.TriggerEvent(sessionEvent);
    }

    public void EditorLoad()
    {
        SessionEvent sessionEvent = new SessionEvent();
        sessionEvent.option = SESSION_OPTION.Load;
        EventController.TriggerEvent(sessionEvent);
    }

#endif

    #endregion

}