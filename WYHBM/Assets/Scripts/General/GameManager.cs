using System.Collections.Generic;
using Events;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("General")]
    public bool isPaused;
    public bool inCombat;
    public AMBIENT currentAmbient;

    [Header("References")]
    public GlobalController globalController;
    public CombatManager combatManager;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;
    public Vector3 dropZone;

    [Header("Combat")]
    public CombatArea[] combatAreas;
    [Space]
    public List<Player> combatCharacters;

    public Dictionary<int, QuestSO> dictionaryQuest;
    public Dictionary<int, int> dictionaryProgress;
    public Dictionary<int, Slot> dictionarySlot;

    // Combat
    private CombatArea _currentCombatArea;
    private NPCController currentNPC;

    private AMBIENT _lastAmbient;
    private int _inventoryMaxSlots = 6;

    // Events
    private FadeEvent _fadeEvent;

    // Properties
    private bool _isInventoryFull;
    public bool IsInventoryFull { get { return _isInventoryFull; } }

    private List<ItemSO> _items;
    public List<ItemSO> Items { get { return _items; } }

    private DialogSO _currentDialog;
    public DialogSO CurrentDialog { get { return _currentDialog; } }

    private QuestSO _currentQuest;
    public QuestSO CurrentQuest { get { return _currentQuest; } }

    private void Start()
    {
        _items = new List<ItemSO>();

        dictionaryQuest = new Dictionary<int, QuestSO>();
        dictionaryProgress = new Dictionary<int, int>();
        dictionarySlot = new Dictionary<int, Slot>();

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = true;
    }

    private void OnEnable()
    {
        EventController.AddListener<EnterCombatEvent>(OnEnterCombat);
        EventController.AddListener<ExitCombatEvent>(OnExitCombat);
        EventController.AddListener<EnableDialogEvent>(OnEnableDialog);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnterCombatEvent>(OnEnterCombat);
        EventController.RemoveListener<ExitCombatEvent>(OnExitCombat);
        EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);

    }

    private void Update()
    {
        if (!inCombat)
        {
            Pause();
            OpenInventory();
            OpenQuest();
        }
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause();
        }
    }

    private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            worldUI.MenuPause(BUTTON_TYPE.Inventory);
            SetPause();
        }
    }

    private void OpenQuest()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            worldUI.MenuPause(BUTTON_TYPE.Diary);
            SetPause();
        }
    }

    public void SetPause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        worldUI.Pause(isPaused);
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

                inCombat = false;
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
                
                inCombat = true;
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

    private void StartCombat()
    {
        currentNPC.Kill();
        combatManager.InitiateTurn();
    }

    public Vector3 GetPlayerFootPosition()
    {
        return globalController.player.dropZone.transform.position;
    }

    public Ray GetRayMouse()
    {
        return globalController.mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    #region Inventory

    public void AddItem(ItemSO item)
    {
        _items.Add(item);

        _isInventoryFull = _items.Count == _inventoryMaxSlots;
    }

    public void DropItem(ItemSO item)
    {
        _items.Remove(item);

        dictionarySlot.Remove(item.GetInstanceID());

        worldUI.itemDescription.Hide();
    }

    public void EquipItem(ItemSO item)
    {
        switch (item.type)
        {
            case ITEM_TYPE.Weapon:
                combatCharacters[0].weapon = item;
                break;

            case ITEM_TYPE.Damage: 
            case ITEM_TYPE.Heal: 
                combatCharacters[0].item = item;
                break;

            case ITEM_TYPE.Defense:
                combatCharacters[0].defense = item;
                break;

            default:
                break;
        }

        _items.Remove(item);

        worldUI.itemDescription.Hide();
    }

    public void UnequipItem(ItemSO item)
    {
        switch (item.type)
        {
            case ITEM_TYPE.Damage:
                combatCharacters[0].weapon = null;
                break;

            case ITEM_TYPE.Heal: // TODO Mariano: Change to Generic Item
                combatCharacters[0].item = null;
                break;

            case ITEM_TYPE.Defense:
                combatCharacters[0].defense = null;
                break;

            default:
                break;
        }

        _items.Add(item);
    }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {
        if (!dictionaryQuest.ContainsKey(data.GetInstanceID()))
        {
            dictionaryQuest.Add(data.GetInstanceID(), data);

            dictionaryProgress.Add(data.GetInstanceID(), 0);
        }
    }

    public void ProgressQuest(QuestSO quest, int progress)
    {
        if (!dictionaryQuest.ContainsKey(quest.GetInstanceID()) ||
            dictionaryProgress[quest.GetInstanceID()] != progress ||
            dictionaryProgress[quest.GetInstanceID()] >= dictionaryQuest[quest.GetInstanceID()].objetives.Length)
        {
            return;
        }

        dictionaryProgress[quest.GetInstanceID()]++;

        worldUI.UpdateQuest(dictionaryQuest[quest.GetInstanceID()], dictionaryProgress[quest.GetInstanceID()]);
    }

    public void GiveReward()
    {
        // TODO Mariano: Dar recompensa del QuestSO
        // Instantiate item in inventory
    }

    #endregion

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

    // Enable interaction dialog
    private void OnEnableDialog(EnableDialogEvent evt)
    {
        if (evt.enable)
        {
            _currentDialog = evt.dialog;
            _currentQuest = evt.dialog.questSO;
            EventController.AddListener<InteractionEvent>(worldUI.OnInteractionDialog);
        }
        else
        {
            _currentDialog = null;
            _currentQuest = null;
            EventController.RemoveListener<InteractionEvent>(worldUI.OnInteractionDialog);
        }
    }

    #endregion

    #region Persistence

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        for (int i = 0; i < GameData.Data.items.Count; i++)
        {
            if (GameData.Data.items[i] == GameData.Instance.persistenceItem)continue;

            Slot newSlot = Instantiate(GameData.Instance.gameConfig.slotPrefab, worldUI.itemParents);
            newSlot.AddItem(GameData.Data.items[i]);
            dictionarySlot.Add(_items[i].GetInstanceID(), newSlot);
        }

        foreach (var key in GameData.Data.dictionaryQuest.Keys)
        {
            if (GameData.Data.dictionaryQuest[key] == GameData.Instance.persistenceQuest)continue;

            dictionaryQuest.Add(key, GameData.Data.dictionaryQuest[key]);
            dictionaryProgress.Add(key, GameData.Data.dictionaryProgress[key]);
            // TODO: Agregar Quest en progreso a UI
        }
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        ClearOldData();

        for (int i = 0; i < _items.Count; i++)
        {
            GameData.Data.items.Add(_items[i]);
        }

        for (int i = 0; i < dictionaryQuest.Count; i++)
        {
            GameData.Data.dictionaryQuest.Add(dictionaryQuest[i].GetInstanceID(), dictionaryQuest[i]);
            GameData.Data.dictionaryProgress.Add(dictionaryQuest[i].GetInstanceID(), dictionaryProgress[i]);
        }
        
        GameData.SaveData();
    }

    private void ClearOldData()
    {
        GameData.Data.items.Clear();
        GameData.Data.dictionaryQuest.Clear();
        GameData.Data.dictionaryProgress.Clear();
    }

    [ContextMenu("Delete Game")]
    public void DeleteGame()
    {
        GameData.DeleteAllData();
    }

    #endregion

}