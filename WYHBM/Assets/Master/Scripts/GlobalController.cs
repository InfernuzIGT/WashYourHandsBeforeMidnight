using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Events;
using FMODUnity;
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

    public StudioEventEmitter listenModeOnSound;
    public StudioEventEmitter listenModeOffSound;

    [Header("Developer")]
    [SerializeField] private bool _devAutoInit = false;
    [SerializeField] private bool _devSilentSteps = false;
    [SerializeField] private bool _devDDLegacyMode = false;

    [Header("General")]
    [SerializeField, ReadOnly] private PlayerSO playerData;
    [Space]
    [SerializeField, ReadOnly] private bool _isPaused;
    [SerializeField, ReadOnly] private bool _inDialog;
    [SerializeField, ReadOnly] private bool _inCombat;
    [Space]
    [SerializeField, ReadOnly] private SessionData sessionData;

    private bool skipEncounters = true;
    // public ItemSO[] items;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private WorldConfig _worldConfig;
    [SerializeField, ConditionalHide] private CombatConfig _combatConfig;
    [SerializeField, ConditionalHide] private Camera _mainCamera;
    [SerializeField, ConditionalHide] private CinemachineVirtualUtility _playerCamera;
    [Space]
    [SerializeField, ConditionalHide] private GameData _gameData;
    [SerializeField, ConditionalHide] private CanvasPersistent _canvasPersistent;
    [SerializeField, ConditionalHide] private PlayerController _playerController;
    [SerializeField, ConditionalHide] private CombatController _combatController;
    [Space]
    [SerializeField, ConditionalHide] private PlayableDirector _playableDirector;
    [SerializeField, ConditionalHide] private CanvasWorld _canvasWorld;
    [SerializeField, ConditionalHide] private CanvasCombat _canvasCombat;
    [Space]
    [SerializeField, ConditionalHide] private Material _materialFOV;
    [SerializeField, ConditionalHide] private Material _materialDitherNPC;

    // Shaders
    private int hash_IsVisible = Shader.PropertyToID("_IsVisible");

    // PostProcess
    private ColorAdjustments _ppColorAdjustments;
    private LensDistortion _ppLensDistortion;
    private DepthOfField _ppDepthOfField;
    private Vignette _ppVignette;
    private Color _colorVigneteInactive = new Color(0.025f, 0, 0.25f, 1);
    private Color _colorVigneteActive = new Color(0.1f, 0.1f, 0.1f, 1);

    private CinemachineVirtualCamera _combatCamera;
    private Coroutine _coroutineListenMode;
    private bool _isInteriorCamera;
    private bool _fovIsActive;
    private float _fovCurrentTime = 0;

    // Events
    private EnableMovementEvent _enableMovementEvent;
    private CutsceneEvent _cutsceneEvent;
    private DialogDesignerEvent _interactionDialogEvent;
    private PauseEvent _pauseEvent;
    private FadeEvent _fadeEvent;

    public SessionData SessionData { get { return sessionData; } set { sessionData = value; } }
    public PlayerSO PlayerData { get { return playerData; } }

    private void Start()
    {
        if (_devAutoInit)CheckPersistenceObjects();
    }

    public void Init(Vector3 spawnPosition)
    {
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Player"));

        if (!_devAutoInit)CheckPersistenceObjects();

        _enableMovementEvent = new EnableMovementEvent();

        _interactionDialogEvent = new DialogDesignerEvent();
        _interactionDialogEvent.enable = false;

        _cutsceneEvent = new CutsceneEvent();
        _cutsceneEvent.show = false;

        _pauseEvent = new PauseEvent();

        _fadeEvent = new FadeEvent();
        _fadeEvent.callbackMid = SwitchAmbient;

        SpawnPlayer(spawnPosition);
        SpawnUI();

        CheckCamera();
        // AddItems();

        _playableDirector.stopped += OnCutsceneStop;

        _materialFOV.SetFloat(hash_IsVisible, 0);
        _materialDitherNPC.SetFloat(hash_IsVisible, 0);

#if UNITY_EDITOR
        _gameData.gameObject.name = "GameData";
        _canvasWorld.gameObject.name = "Canvas (World)";
        _canvasCombat.gameObject.name = "Canvas (Combat)";
#endif
    }

    private void OnEnable()
    {
        EventController.AddListener<DialogDesignerEvent>(OnEnableDialog);
        EventController.AddListener<QuestEvent>(OnQuest);
        EventController.AddListener<ChangeInputEvent>(OnChangeInput);
        EventController.AddListener<SessionEvent>(OnSession);
        EventController.AddListener<CutsceneEvent>(OnCutscene);
        EventController.AddListener<PauseEvent>(OnPause);
        EventController.AddListener<CombatEvent>(OnCombat);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<DialogDesignerEvent>(OnEnableDialog);
        EventController.RemoveListener<QuestEvent>(OnQuest);
        EventController.RemoveListener<ChangeInputEvent>(OnChangeInput);
        EventController.RemoveListener<SessionEvent>(OnSession);
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
        EventController.RemoveListener<PauseEvent>(OnPause);
        EventController.RemoveListener<CombatEvent>(OnCombat);
    }

    private void OnCombat(CombatEvent evt)
    {
        _inCombat = evt.isEnter;

        if (evt.isEnter)
        {
            _fadeEvent.instant = true;
            _fadeEvent.delay = _worldConfig.fadeDelay;
            _fadeEvent.callbackEnd = _combatController.InitiateTurn;
            StartCoroutine(StartCombat());
        }
        else
        {
            _fadeEvent.instant = false;
            _fadeEvent.delay = 0;
            _fadeEvent.callbackEnd = () => EnableMovement(true);
            StartCoroutine(FinishCombat());
        }
    }

    private IEnumerator StartCombat()
    {
        yield return new WaitForSeconds(_combatConfig.waitTimeToStart);
        EventController.TriggerEvent(_fadeEvent);
    }

    private IEnumerator FinishCombat()
    {
        yield return new WaitForSeconds(_combatConfig.waitTimeToFinish);
        EventController.TriggerEvent(_fadeEvent);
    }

    private void SwitchAmbient()
    {
        _canvasCombat.Show(_inCombat);
        _canvasWorld.Show(!_inCombat);

        _combatController.SetCombatArea(_inCombat);
        if (!_inCombat)_canvasCombat.ClearActions();

        ChangeToCombatCamera(_inCombat ? _combatController.GetCombatAreaCamera() : null);
    }

    private void OnPause(PauseEvent evt)
    {
        _isPaused = evt.isPaused;

        if (_isPaused)
        {

            ListenModeInstant(false);

            // switch (evt.pauseType)
            // {
            //     case PAUSE_TYPE.PauseMenu:

            //         break;

            //     default:
            //         break;
            // }
        }
        // else
        // {

        // }

        _ppColorAdjustments.saturation.value = _isPaused ? -80 : 0;
        _ppDepthOfField.gaussianStart.value = _isPaused ? 0 : 22.5f;
        _ppDepthOfField.gaussianEnd.value = _isPaused ? 0 : 60;

        _enableMovementEvent.canMove = !evt.isPaused;
        EventController.TriggerEvent(_enableMovementEvent);
    }

    public List<Player> GetListPlayer()
    {
        return _combatController.ListPlayers;
    }

    private void CheckPersistenceObjects()
    {
        GameData tempGamedata = GameObject.FindObjectOfType<GameData>();

        _gameData = tempGamedata != null ? tempGamedata : Instantiate(_gameData);
        _gameData.DevDDLegacyMode = _devDDLegacyMode;
        if (_devAutoInit)_gameData.GetSceneReferences();
        sessionData = _gameData.LoadSessionData();

        CanvasPersistent tempCanvasPersistent = GameObject.FindObjectOfType<CanvasPersistent>();

        tempCanvasPersistent = tempCanvasPersistent != null ? tempCanvasPersistent : Instantiate(_canvasPersistent);
    }

    private void SpawnPlayer(Vector3 spawnPosition)
    {
        _playerController = Instantiate(_playerController, spawnPosition, Quaternion.identity);
        _playerController.DevSilentSteps = _devSilentSteps;
        _playerController.SetInput(() => Pause(PAUSE_TYPE.PauseMenu), () => Pause(PAUSE_TYPE.Inventory));
        _playerController.SetPlayerData(playerData);

        sessionData.playerData = playerData;

        _playerController.Input.Player.ListenMode.started += ctx => ListenMode(true);
        _playerController.Input.Player.ListenMode.canceled += ctx => ListenMode(false);

        _playerController.cancelListerMode += CancelListenMode;
    }

    private void CancelListenMode()
    {
        if (!_playerController.IsCrouching)return;

        ListenMode(false);
    }

    private void ListenMode(bool active)
    {
        if (!_playerController.IsCrouching)return;

        _fovIsActive = active;

        // if (!playerController.IsCrouching)playerController.Crouch();

        if (_coroutineListenMode != null)
        {
            StopCoroutine(_coroutineListenMode);
            _coroutineListenMode = null;
        }

        _coroutineListenMode = StartCoroutine(ChangeListenMode());
    }

    private void ListenModeInstant(bool active)
    {
        _fovIsActive = active;

        if (_coroutineListenMode != null)
        {
            StopCoroutine(_coroutineListenMode);
            _coroutineListenMode = null;

        }

        _fovCurrentTime = _fovIsActive ? _worldConfig.fovTime : 0;

        _materialFOV.SetFloat(hash_IsVisible, _fovIsActive ? 0.35f : 0);
        _materialDitherNPC.SetFloat(hash_IsVisible, _fovIsActive ? 1 : 0);

        SetValuesListenMode();
    }

    private IEnumerator ChangeListenMode()
    {
        if (_fovIsActive)
        {
            listenModeOnSound.Play();
            while (_fovCurrentTime < _worldConfig.fovTime)
            {
                SetValuesListenMode();

                _fovCurrentTime += Time.deltaTime;

                yield return null;
            }
        }
        else
        {
            listenModeOffSound.Play();
            while (_fovCurrentTime > 0)
            {
                SetValuesListenMode();

                _fovCurrentTime -= Time.deltaTime;

                yield return null;
            }
        }

        _fovCurrentTime = _fovIsActive ? _worldConfig.fovTime : 0;

        _materialFOV.SetFloat(hash_IsVisible, _fovIsActive ? 0.35f : 0);
        _materialDitherNPC.SetFloat(hash_IsVisible, _fovIsActive ? 1 : 0);
    }

    private void SetValuesListenMode()
    {
        _ppColorAdjustments.saturation.value = Mathf.Lerp(0, -50, (_fovCurrentTime / _worldConfig.fovTime));
        _ppLensDistortion.intensity.value = Mathf.Lerp(0, 0.15f, (_fovCurrentTime / _worldConfig.fovTime));
        _ppDepthOfField.gaussianStart.value = Mathf.Lerp(22.5f, 24, (_fovCurrentTime / _worldConfig.fovTime));
        _ppDepthOfField.gaussianEnd.value = Mathf.Lerp(60, 30f, (_fovCurrentTime / _worldConfig.fovTime));
        _ppVignette.intensity.value = Mathf.Lerp(0.2f, 0.5f, (_fovCurrentTime / _worldConfig.fovTime));
        _ppVignette.smoothness.value = Mathf.Lerp(1, 0.5f, (_fovCurrentTime / _worldConfig.fovTime));

        _ppVignette.color.value = Color.Lerp(_colorVigneteInactive, _colorVigneteActive, (_fovCurrentTime / _worldConfig.fovTime));

        _playerCamera.SetFOV(Mathf.Lerp(40, 35, (_fovCurrentTime / _worldConfig.fovTime)));

        _materialFOV.SetFloat(hash_IsVisible, Mathf.Lerp(0, 0.35f, (_fovCurrentTime / _worldConfig.fovTime)));
        _materialDitherNPC.SetFloat(hash_IsVisible, Mathf.Lerp(0, 1f, (_fovCurrentTime / _worldConfig.fovTime)));
    }

    private void Pause(PAUSE_TYPE pauseType)
    {
        if (_inCombat || _inDialog)return;

        _isPaused = !_isPaused;

        _pauseEvent.isPaused = _isPaused;
        _pauseEvent.pauseType = pauseType;
        EventController.TriggerEvent(_pauseEvent);
    }

    private void CheckCamera()
    {
        _playerCamera.Init(_playerController, _mainCamera);

        DetectTargetBehind detectTargetBehind = _mainCamera.GetComponent<DetectTargetBehind>();
        detectTargetBehind.SetTarget(_playerController.transform);

        Volume volume = GetComponent<Volume>();
        volume.profile.TryGet(out _ppColorAdjustments);
        volume.profile.TryGet(out _ppLensDistortion);
        volume.profile.TryGet(out _ppVignette);
        volume.profile.TryGet(out _ppDepthOfField);
    }

    private void SpawnUI()
    {
        _canvasWorld = Instantiate(_canvasWorld);
        _canvasCombat = Instantiate(_canvasCombat);

        _canvasWorld.Show(!_inCombat);
        _canvasCombat.Show(_inCombat);
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
            _playerCamera.gameObject.SetActive(true);
            _combatCamera = null;
        }
        else
        {
            combatCamera.gameObject.SetActive(true);
            _playerCamera.gameObject.SetActive(false);
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
        return _playerController.GetPlayerInMovement() && !skipEncounters;
    }

    public Interaction GetPlayerCurrentInteraction()
    {
        return _playerController.CurrentInteraction;
    }

    public void HidePlayer(bool isHiding)
    {
        _playerController.gameObject.SetActive(!isHiding);
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

    private void OnEnableDialog(DialogDesignerEvent evt)
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
        _playableDirector.playableAsset = evt.cutscene;
        _playableDirector.Play();
    }

    private void OnSession(SessionEvent evt)
    {
        if (_gameData == null)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> GameData NULL");
            return;
        }

        switch (evt.option)
        {
            case SESSION_OPTION.Save:
                _gameData.SaveSession(sessionData);
                break;

            case SESSION_OPTION.Load:
                sessionData = _gameData.LoadSessionData();
                break;

            case SESSION_OPTION.Delete:
                _gameData.DeleteAll();
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
                    if (sessionData.listQuest[i].data = evt.data)break;
                }

                Quest newQuest = new Quest();
                newQuest.data = evt.data;
                newQuest.currentStep = 0;

                sessionData.listQuest.Add(newQuest);
                break;

            case QUEST_STATE.Update:
                for (int i = 0; i < sessionData.listQuest.Count; i++)
                {
                    if (sessionData.listQuest[i].data = evt.data)
                    {
                        sessionData.listQuest[i].currentStep++;

                        if (sessionData.listQuest[i].currentStep >= evt.data.steps)CompleteQuest(i);
                        break;
                    }
                }
                break;

            case QUEST_STATE.Complete:
                for (int i = 0; i < sessionData.listQuest.Count; i++)
                {
                    if (sessionData.listQuest[i].data = evt.data)
                    {
                        CompleteQuest(i);
                        break;
                    }
                }
                break;
        }

        _gameData.SaveSession(sessionData);
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