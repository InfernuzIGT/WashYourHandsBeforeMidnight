using System.Collections.Generic;
using Events;
using UnityEngine;

[System.Serializable]
public class CombatPlayer
{
    public Player character;
    public List<ItemSO> equipment = new List<ItemSO>();
}

[System.Serializable]
public class CombatEnemy
{
    public Enemy character;
    public List<ItemSO> equipment = new List<ItemSO>();
}

public class GameManager : MonoSingleton<GameManager>
{
    [Header("General")]
    public bool isPaused;
    public bool inCombat;
    public bool inWorld;

    [Header("References")]
    public GlobalController globalController;
    public CombatManager combatManager;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;
    public MenuController menuController;
    public Vector3 dropZone;

    [Header("Combat")]
    public CombatArea[] combatAreas;
    [Space]
    public List<CombatPlayer> combatPlayers;

    public List<QuestSO> listQuest;
    public List<int> listProgress;
    public List<Slot> listSlots;

    // Combat
    private CombatArea _currentCombatArea;
    private NPCController currentNPC;

    // Inventory
    private int _inventoryMaxSlots = 8;
    private int _equipmentMaxSlots = 3;
    private int _characterIndex;

    // Events
    private FadeEvent _fadeEvent;

    // Properties
    public bool IsInventoryFull { get { return _items.Count == _inventoryMaxSlots; } }
    public bool IsEquipmentFull { get { return combatPlayers[_characterIndex].equipment.Count == _equipmentMaxSlots; } }

    private List<ItemSO> _items;
    public List<ItemSO> Items { get { return _items; } }

    private DialogSO _currentDialog;
    public DialogSO CurrentDialog { get { return _currentDialog; } }

    // private bool _hasPostDialogs;
    // public bool hasPostDialogs { get { return _hasPostDialogs; } }

    private QuestSO _currentQuest;
    public QuestSO CurrentQuest { get { return _currentQuest; } }

    private void Start()
    {
        _items = new List<ItemSO>();

        listQuest = new List<QuestSO>();
        listProgress = new List<int>();
        listSlots = new List<Slot>();

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = true;

        _characterIndex = 0;
        worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inLeftLimit : true);

        GameManager.Instance.LoadGame();

        inWorld = true;
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
        if (inWorld)
        {
            worldUI.EnableCanvas(true);
            combatUI.EnableCanvas(false);

            globalController.ChangeCamera(null);
            combatManager.CloseCombatArea();
            combatUI.actions.Clear();

            inCombat = false;
        }
        else
        {
            worldUI.EnableCanvas(false);
            combatUI.EnableCanvas(true);

            globalController.ChangeCamera(_currentCombatArea.virtualCamera);

            inCombat = true;
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
    }

    public void DropItem(Slot slot)
    {
        _items.Remove(slot.Item);

        listSlots.Remove(slot);

        worldUI.itemDescription.Hide();
    }

    public void EquipItem(ItemSO item)
    {
        combatPlayers[_characterIndex].equipment.Add(item);

        _items.Remove(item);

        worldUI.itemDescription.Hide();
    }

    public void UnequipItem(ItemSO item)
    {
        combatPlayers[_characterIndex].equipment.Remove(item);

        _items.Add(item);
    }

    public void NextCharacter(bool isLeft)
    {
        if (isLeft)
        {
            if (_characterIndex <= 0) return;

            _characterIndex--;
            worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inLeftLimit : _characterIndex <= 0);
        }
        else
        {
            if (_characterIndex >= combatPlayers.Count - 1) return;

            _characterIndex++;
            worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inRightLimit : _characterIndex >= combatPlayers.Count - 1);
        }
    }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {
        if (!listQuest.Contains(data))
        {
            listQuest.Add(data);

            listProgress.Add(0);
        }

        // if (!listQuest.ContainsKey(data.GetInstanceID()))
        // {
        //     listQuest.Add(data.GetInstanceID(), data);

        //     ListProgress.Add(data.GetInstanceID(), 0);
        // }
    }

    public void ProgressQuest(QuestSO quest, int progress)
    {
        if (!listQuest.Contains(quest) ||
            listProgress[quest.GetInstanceID()] != progress ||
            listProgress[quest.GetInstanceID()] >= listQuest[quest.GetInstanceID()].objetives.Length)
        {
            return;
        }

        listProgress[quest.GetInstanceID()]++;

        worldUI.UpdateQuest(listQuest[quest.GetInstanceID()], listProgress[quest.GetInstanceID()]);
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
        inWorld = false;
        currentNPC = evt.currentNPC;

        int indexArea = Random.Range(0, combatAreas.Length);
        _currentCombatArea = combatAreas[indexArea];
        combatManager.SetData(_currentCombatArea, combatPlayers, evt.npc.combatEnemies);

        _fadeEvent.callbackStart = SwitchMovement;
        _fadeEvent.callbackMid = SwitchAmbient;
        _fadeEvent.callbackEnd = StartCombat;

        EventController.TriggerEvent(_fadeEvent);
    }

    public void OnExitCombat(ExitCombatEvent evt)
    {
        inWorld = true;

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

    public void LoadGame()
    {
        for (int i = 0; i < GameData.Data.items.Count; i++)
        {
            if (GameData.Data.items[i] == GameData.Instance.persistenceItem) continue;

            Slot newSlot = Instantiate(GameData.Instance.worldConfig.slotPrefab, worldUI.itemParents);
            newSlot.AddItem(GameData.Data.items[i]);
            listSlots.Add(newSlot);
        }

        // foreach (var key in GameData.Data.listQuest.Keys)
        // {
        //     Debug.Log($"<b> {GameData.Data.listQuest[key].title} </b>");

        //     if (GameData.Data.listQuest[key] == GameData.Instance.persistenceQuest)continue;

        //     listQuest.Add(GameData.Data.listQuest[key]);
        //     listProgress.Add(GameData.Data.listProgress[key]);

        //     worldUI.ReloadQuest(GameData.Data.listQuest[key]);
        // }
    }

    public void SaveGame()
    {
        ClearOldData();

        for (int i = 0; i < _items.Count; i++)
        {
            GameData.Data.items.Add(_items[i]);
        }

        // foreach (var key in listQuest.Keys)
        // {
        // GameData.Data.dictionaryQuest.Add(dictionaryQuest[key].GetInstanceID(), dictionaryQuest[key]);
        // GameData.Data.dictionaryProgress.Add(dictionaryQuest[key].GetInstanceID(), dictionaryProgress[key]);

        // }

        GameData.Data.position = new Vector3(
            globalController.player.transform.position.x,
            globalController.player.transform.position.y,
            globalController.player.transform.position.z);

        GameData.SaveData();

    }

    private void ClearOldData()
    {
        GameData.Data.items.Clear();
        GameData.Data.listQuest.Clear();
        GameData.Data.listProgress.Clear();
        GameData.Data.position = new Vector3(0, 0, 0);
    }

    [ContextMenu("Delete Game")]
    public void DeleteGame()
    {
        GameData.DeleteAllData();
    }

    #endregion

}