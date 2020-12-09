using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[System.Serializable]
public class Quest
{
    public QuestSO data;
    public int currentStep = 0;
}

public class GlobalController : MonoBehaviour
{
    [Header("General")]
    public PlayerSO playerData;
    [Space]
    public bool isPaused;
    public bool inCombat;
    [Space]
    public SessionData sessionData;

    [Header("Developer")]
    [SerializeField] private bool _inputDebugMode = true;
    [SerializeField] private Transform _customSpawnPoint = null;
    private bool skipEncounters = true;
    // public ItemSO[] items;

    [Header("References")]
    public GameData gameData;
    public PlayerController playerController;
    public Camera mainCamera;
    public CinemachineVirtualCamera worldCamera;
    [Space]
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;
    public Fade fadeUI;
    public EventSystemUtility eventSystemUtility;

    [Header("-DEPRECATED-")]
    public CinemachineVirtualCamera exteriorCamera;
    public CinemachineVirtualCamera interiorCamera;
    public CinemachineVirtualCamera cutscene;

    private CinemachineVirtualCamera _worldCamera;
    private CinemachineVirtualCamera _combatCamera;
    private bool _isInteriorCamera;

    private float _offsetPlayer = 1.505f;

    private EnableMovementEvent _enableMovementEvent;

    private void Awake()
    {

#if UNITY_EDITOR

        if (_inputDebugMode)InputUtility.debugMode = true;

#endif

    }

    private void Start()
    {
        CheckGameData();

        _enableMovementEvent = new EnableMovementEvent();

        SpawnPlayer();
        SpawnCameras();
        SpawnUI();
        // SetCamera();
        // AddItems();

#if UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // TODO Mariano: REORDER OBJECTS IN HIERARCHY
        // TODO Mariano: RENAME OBJECTS IN HIERARCHY
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
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);
        EventController.RemoveListener<QuestEvent>(OnQuest);
        EventController.RemoveListener<ChangeInputEvent>(OnChangeInput);
        EventController.RemoveListener<SessionEvent>(OnSession);
    }

    private void CheckGameData()
    {
        GameData tempGamedata = GameObject.FindObjectOfType<GameData>();
        
        gameData = tempGamedata != null ? tempGamedata : Instantiate(gameData);

        sessionData = gameData.LoadSession();
    }

    private void SpawnPlayer()
    {
        RaycastHit hit;

#if UNITY_EDITOR

        if (_customSpawnPoint != null)
        {
            if (Physics.Raycast(_customSpawnPoint.position, Vector3.down, out hit, Mathf.Infinity))
            {
                Vector3 spawnPosition = hit.point + new Vector3(0, _offsetPlayer, 0);
                playerController = Instantiate(playerController, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Can't detect surface to spawn!");

                playerController = Instantiate(playerController, _customSpawnPoint.position, Quaternion.identity);
            }
        }
        else
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            Vector3 sceneCameraPosition = sceneView.pivot - sceneView.camera.transform.position;

            if (Physics.Raycast(sceneCameraPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                Vector3 spawnPosition = hit.point + new Vector3(0, _offsetPlayer, 0);
                playerController = Instantiate(playerController, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Can't detect surface to spawn!");

                playerController = Instantiate(playerController, sceneCameraPosition, Quaternion.identity);
            }
        }
#else

        if (Physics.Raycast(spawnPoint.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Vector3 spawnPosition = hit.point + new Vector3(0, _offsetPlayer, 0);
            player = Instantiate(player, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Can't detect surface to spawn!");

            player = Instantiate(player, spawnPoint.position, Quaternion.identity);
        }

#endif

        playerController.SetPlayerData(playerData);

        sessionData.playerData = playerData;
    }

    private void SpawnCameras()
    {
        mainCamera = Instantiate(mainCamera);
        worldCamera = Instantiate(worldCamera);

        worldCamera.m_Follow = playerController.transform;
        worldCamera.m_LookAt = playerController.transform;

        DetectTargetBehind detectTargetBehind = mainCamera.GetComponent<DetectTargetBehind>();
        detectTargetBehind.SetTarget(playerController.transform);
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

    private void SetCamera()
    {
        exteriorCamera.m_Follow = playerController.transform;
        exteriorCamera.m_LookAt = playerController.transform;
        // exteriorCamera.transform.position = player.transform.position;

        interiorCamera.m_Follow = playerController.transform;
        interiorCamera.m_LookAt = playerController.transform;
        // interiorCamera.transform.position = player.transform.position;

        _worldCamera = _isInteriorCamera ? interiorCamera : exteriorCamera;

        DetectTargetBehind detectTargetBehind = mainCamera.GetComponent<DetectTargetBehind>();
        detectTargetBehind.SetTarget(playerController.transform);
    }

    public void ChangeWorldCamera()
    {
        _isInteriorCamera = !_isInteriorCamera;

        _worldCamera = _isInteriorCamera ? interiorCamera : exteriorCamera;

        exteriorCamera.gameObject.SetActive(!_isInteriorCamera);
        interiorCamera.gameObject.SetActive(_isInteriorCamera);
    }

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

    private void ChangeInput(bool enable)
    {
        if (enable)
        {
            playerController.Input.Player.Enable();
        }
        else
        {
            playerController.Input.Player.Disable();
        }

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
            ChangeInput(true);
        }
    }

    private void OnInteractionDialog(InteractionEvent evt)
    {
        if (!evt.isStart)return;

        ChangeInput(false);
    }

    private void OnChangeInput(ChangeInputEvent evt)
    {
        ChangeInput(evt.enable);
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