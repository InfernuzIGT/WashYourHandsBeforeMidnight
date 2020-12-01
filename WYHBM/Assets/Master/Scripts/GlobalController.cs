﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Events;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GlobalController : MonoBehaviour
{
    [Header("General")]
    public bool isPaused;
    public bool inCombat;

    [Header("Developer")]
    [SerializeField] private bool _inputDebugMode = true;
    [Space]
    [SerializeField] private Transform _customSpawnPoint = null;
    private bool skipEncounters = true;
    // public ItemSO[] items;

    [Header("Player")]
    public PlayerSO playerData;
    public PlayerController playerController;

    [Header("Camera")]
    public Camera mainCamera;
    public CinemachineVirtualCamera worldCamera;

    [Header("UI")]
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;
    public Fade fadeUI;
    public EventSystemUtility eventSystemUtility;

    [Header("-DEPRECATED-")]
    public CinemachineVirtualCamera exteriorCamera;
    public CinemachineVirtualCamera interiorCamera;
    public CinemachineVirtualCamera cutscene;

    private DDUtility _ddUtility;

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
        EventController.AddListener<ChangeInputEvent>(OnChangeInput);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);
        EventController.RemoveListener<ChangeInputEvent>(OnChangeInput);
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
    }

    private void SpawnUI()
    {
        fadeUI = Instantiate(fadeUI);
        eventSystemUtility = Instantiate(eventSystemUtility);

        worldUI = Instantiate(worldUI);
        combatUI = Instantiate(combatUI);

        worldUI.Show(!inCombat);
        combatUI.Show(inCombat);

        _ddUtility = worldUI.DDUtility;
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

    #endregion

}