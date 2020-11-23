using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

[System.Serializable]
public class EnemyEncounter
{
    public List<Enemy> enemies = new List<Enemy>();
}

public class GameManager : MonoSingleton<GameManager>
{
    [Header("General")]
    public bool isPaused;
    public bool inCombat;

    [Header("References")]
    public GlobalController globalController;
    public CombatManager combatManager;
    public CinemachineManager cinemachineManager;
    public PlayableDirector playableDirector;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;
    public EventSystem eventSystem;

    [Header("Combat")]
    public CombatArea[] combatAreas;
    [Space]
    public List<Player> combatPlayers;
    [Space]
    public List<EnemyEncounter> enemyEncounters;

    public List<QuestData> listQuestData;
    // public List<QuestSO> listQuest;
    // public List<int> listProgress;
    public Dictionary<int, QuestSO> dictionaryQuest;
    public Dictionary<int, int> dictionaryProgress;
    public List<Slot> listSlots;

    // Combat
    private CombatArea _currentCombatArea;
    private NPCController currentNPC;
    private float _currentTimeEncounter;
    private float _limitTimeEncounter = 10;

    // Inventory
    // private int _inventoryMaxSlots = 8;
    // private int _equipmentMaxSlots = 3;
    // private int _characterIndex;

    // Hold System
    // private bool _holdStarted;
    // private float _holdCurrentTime;
    // private float _holdLimitTime;
    // private System.Action _holdCallback;
    // private UnityEngine.UI.Image _holdImage;

    // Coroutines
    private Coroutine _coroutineEnconters;
    // private Coroutine _coroutineHoldSystem;
    // private WaitForSeconds _waitHoldEnd;

    // Events
    private FadeEvent _fadeEvent;

    // Properties
    private List<ItemSO> _items;
    public List<ItemSO> Items { get { return _items; } }

    // private float _holdFillValue;
    // public float HoldFillValue { get { return _holdFillValue; } }

    // public bool IsInventoryFull { get { return _items.Count == _inventoryMaxSlots; } }
    // public bool IsEquipmentFull { get { return combatPlayers[_characterIndex].equipment.Count == _equipmentMaxSlots; } }

    private DialogSO _currentDialog;
    public DialogSO CurrentDialog { get { return _currentDialog; } }

    private QuestData _currentQuestData;
    public QuestData CurrentQuestData { get { return _currentQuestData; } }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _items = new List<ItemSO>();

        // listQuest = new List<QuestSO>();
        // listProgress = new List<int>();
        listQuestData = new List<QuestData>();
        dictionaryQuest = new Dictionary<int, QuestSO>();
        dictionaryProgress = new Dictionary<int, int>();

        listSlots = new List<Slot>();

        _fadeEvent = new FadeEvent();
        _fadeEvent.fadeFast = true;

        // _waitHoldEnd = new WaitForSeconds(1f);

        // _characterIndex = 0;
        // worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inLeftLimit : true);

        // GameManager.Instance.LoadGame();

        inCombat = false;

        CheckEncounters(true);
    }

    private void OnEnable()
    {
        EventController.AddListener<EnterCombatEvent>(OnEnterCombat);
        EventController.AddListener<ExitCombatEvent>(OnExitCombat);
        EventController.AddListener<EnableDialogEvent>(OnEnableDialog);
        EventController.AddListener<CutsceneEvent>(OnCutscene);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnterCombatEvent>(OnEnterCombat);
        EventController.RemoveListener<ExitCombatEvent>(OnExitCombat);
        EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);
        EventController.RemoveListener<CutsceneEvent>(OnCutscene);
    }

    public void CheckEncounters(bool isEnabled)
    {
        if (isEnabled)
        {
            _coroutineEnconters = StartCoroutine(CheckEncounter());
        }
        else
        {
            StopCoroutine(_coroutineEnconters);
            _coroutineEnconters = null;
        }
    }

    private IEnumerator CheckEncounter()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            if (!inCombat && !isPaused && globalController.GetPlayerInMovement())
            {
                _currentTimeEncounter += Time.deltaTime;

                if (_currentTimeEncounter > _limitTimeEncounter)
                {
                    _currentTimeEncounter = 0;
                    TriggerCombat();
                }
            }
            yield return null;
        }
    }

    public void Pause(PAUSE_TYPE type)
    {
        switch (type)
        {
            case PAUSE_TYPE.PauseMenu:
                if (!inCombat)
                {
                    isPaused = !isPaused;
                    Time.timeScale = isPaused ? 0 : 1;
                    worldUI.Pause(isPaused);
                }
                break;

            case PAUSE_TYPE.Inventory:
                if (!inCombat)
                {
                    isPaused = !isPaused;
                    Time.timeScale = isPaused ? 0 : 1;
                    // TODO Marcos: Show Inventory 
                }
                break;

            case PAUSE_TYPE.Note:
                isPaused = !isPaused;
                Time.timeScale = isPaused ? 0 : 1;
                worldUI.ActiveNote(isPaused);
                break;

            default:
                isPaused = !isPaused;
                Time.timeScale = isPaused ? 0 : 1;
                break;
        }

        if (!inCombat)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            worldUI.Pause(isPaused);
            // SetPause(false);
        }
    }

    // private void OpenInventory()
    // {
    //     if (Input.GetKeyDown(KeyCode.I))
    //     {
    //         worldUI.MenuPause(BUTTON_TYPE.Inventory);
    //         SetPause();
    //     }
    // }

    // private void OpenQuest()
    // {
    //     if (Input.GetKeyDown(KeyCode.Tab))
    //     {
    //         worldUI.MenuPause(BUTTON_TYPE.Diary);
    //         SetPause();
    //     }
    // }

    private void SwitchAmbient()
    {
        CheckEncounters(!inCombat);

        // globalController.playerController.ToggleInputWorld(!inCombat);
        globalController.HidePlayer(inCombat);

        combatUI.Show(inCombat);
        worldUI.Show(!inCombat);

        combatManager.ToggleInputCombat(inCombat);
        combatManager.SetCombatArea(inCombat);

        if (!inCombat)
        {
            globalController.ChangeToCombatCamera(null);
            combatUI.actions.Clear();
            combatUI.ClearTurn();
        }
        else
        {
            globalController.ChangeToCombatCamera(_currentCombatArea.virtualCamera);
        }
    }

    private void SwitchMovement()
    {
        globalController.playerController.SwitchMovement();
    }

    private void StartCombat()
    {
        // currentNPC?.Kill();
        combatManager.InitiateTurn();
    }

    public void ReorderTurn()
    {
        combatUI.ReorderTurn(combatManager.ListWaitingCharacters);
    }

    public void SelectButton(GameObject button)
    {
        eventSystem.SetSelectedGameObject(button);
    }

    public void PlayerCanSelect(bool canSelect, int combatIndex = 0)
    {
        combatManager.ChangeCombatState(canSelect);
        combatUI.ShowPlayerPanel(canSelect, true);
        if (canSelect)combatUI.ShowActions(combatIndex);
    }

    // #region Hold System

    // public void SetHoldSystem(ref UnityEngine.UI.Image image, float limitTime, System.Action callback)
    // {
    //     _holdImage = image;
    //     _holdLimitTime = limitTime;
    //     _holdCallback = callback;
    // }

    // public void CallHoldSystem(bool isStart)
    // {
    //     _holdStarted = isStart;

    //     if (_holdStarted)
    //     {
    //         _coroutineHoldSystem = StartCoroutine(Hold());
    //     }
    //     else
    //     {
    //         StopCoroutine(_coroutineHoldSystem);

    //         _holdCurrentTime = 0;
    //         _holdImage.fillAmount = 0;
    //     }
    // }

    // private IEnumerator Hold()
    // {
    //     while (_holdStarted)
    //     {
    //         _holdCurrentTime += Time.deltaTime;

    //         _holdImage.fillAmount = _holdCurrentTime / _holdLimitTime;

    //         if (_holdCurrentTime > _holdLimitTime)
    //         {
    //             _holdImage.fillAmount = 1;
    //             _holdCallback.Invoke();

    //             yield return _waitHoldEnd;

    //             CallHoldSystem(false);
    //         }

    //         yield return null;
    //     }
    // }

    // #endregion

    #region Inventory

    // public void AddItem(ItemSO item)
    // {
    //     _items.Add(item);
    // }

    // public void DropItem(Slot slot)
    // {
    //     _items.Remove(slot.Item);

    //     listSlots.Remove(slot);

    //     worldUI.itemDescription.Hide();
    // }

    // public void EquipItem(ItemSO item)
    // {
    //     combatPlayers[_characterIndex].equipment.Add(item);

    //     _items.Remove(item);

    //     worldUI.itemDescription.Hide();
    // }

    // public void UnequipItem(ItemSO item)
    // {
    //     combatPlayers[_characterIndex].equipment.Remove(item);

    //     _items.Add(item);
    // }

    // public void NextCharacter(bool isLeft)
    // {
    //     if (isLeft)
    //     {
    //         if (_characterIndex <= 0)return;

    //         _characterIndex--;
    //         worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inLeftLimit : _characterIndex <= 0);
    //     }
    //     else
    //     {
    //         if (_characterIndex >= combatPlayers.Count - 1)return;

    //         _characterIndex++;
    //         worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inRightLimit : _characterIndex >= combatPlayers.Count - 1);
    //     }
    // }

    #endregion

    #region Quest

    public void AddQuest(QuestSO data)
    {
        if (!dictionaryQuest.ContainsKey(data.GetInstanceID()))
        {
            dictionaryQuest.Add(data.GetInstanceID(), data);

            dictionaryProgress.Add(data.GetInstanceID(), 0);
        }

        // for (int i = 0; i < data.objectsToDestroy.Length; i++)
        // {
        // Destroy(data.objectsToDestroy[i].gameObject);
        // }

        // if (!listQuest.ContainsKey(data.GetInstanceID()))
        // {
        //     listQuest.Add(data.GetInstanceID(), data);

        //     ListProgress.Add(data.GetInstanceID(), 0);
        // }
    }

    public void ProgressQuest(QuestSO quest, int progress)
    {
        // if (!dictionaryQuest.ContainsKey(quest.GetInstanceID()) ||
        //     dictionaryProgress[quest.GetInstanceID()] != progress ||
        //     dictionaryProgress[quest.GetInstanceID()] >= dictionaryQuest[quest.GetInstanceID()].objetives.Length)

        // {
        //     return;
        // }

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
        inCombat = true;
        currentNPC = evt.currentNPC;

        int indexArea = Random.Range(0, combatAreas.Length);
        _currentCombatArea = combatAreas[indexArea];
        combatManager.SetData(_currentCombatArea, combatPlayers, evt.npc.combatEnemies);

        _fadeEvent.callbackStart = SwitchMovement;
        _fadeEvent.callbackMid = SwitchAmbient;
        _fadeEvent.callbackEnd = StartCombat;

        EventController.TriggerEvent(_fadeEvent);
    }

    private void TriggerCombat()
    {
        if (!ProportionValue.GetProbability(0.4f))return;

        inCombat = true;
        currentNPC = null;

        int indexArea = Random.Range(0, combatAreas.Length);
        _currentCombatArea = combatAreas[indexArea];

        int indexEncounter = Random.Range(0, enemyEncounters.Count);
        combatManager.SetData(_currentCombatArea, combatPlayers, enemyEncounters[indexEncounter].enemies);

        _fadeEvent.callbackStart = SwitchMovement;
        _fadeEvent.callbackMid = SwitchAmbient;
        _fadeEvent.callbackEnd = StartCombat;

        EventController.TriggerEvent(_fadeEvent);
    }

    public void OnExitCombat(ExitCombatEvent evt)
    {
        inCombat = false;

        _fadeEvent.callbackStart = null;
        _fadeEvent.callbackMid = SwitchAmbient;
        _fadeEvent.callbackEnd = SwitchMovement;

        EventController.TriggerEvent(_fadeEvent);
    }

    // Enable interaction dialog
    private void OnEnableDialog(EnableDialogEvent evt)
    {
        // if (evt.enable)
        // {
        //     _currentDialog = evt.dialog;
        //     _currentQuestData = evt.questData;
        //     EventController.AddListener<InteractionEvent>(worldUI.OnInteractionDialog);
        // }
        // else
        // {
        //     _currentDialog = null;
        //     _currentQuestData = null;
        //     EventController.RemoveListener<InteractionEvent>(worldUI.OnInteractionDialog);
        // }
    }

    private void OnCutscene(CutsceneEvent evt)
    {
        // Debug.Log($"<b> {evt.cutscene.name} </b>");
        playableDirector.playableAsset = evt.cutscene;

        playableDirector.Play();
    }

    #endregion

    #region Persistence

    /* public void LoadGame()
    {
        for (int i = 0; i < GameData.Data.items.Count; i++)
        {
            if (GameData.Data.items[i] == GameData.Instance.persistenceItem) continue;

            Slot newSlot = Instantiate(GameData.Instance.worldConfig.slotPrefab, worldUI.itemParents);
            newSlot.AddItem(GameData.Data.items[i]);
            listSlots.Add(newSlot);

            GameData.Data.items.Remove(GameData.Instance.persistenceItem);
        }

        foreach (var key in GameData.Data.listQuest.Keys)
        {
            Debug.Log($"<b> {GameData.Data.listQuest[key].title} </b>");

            if (GameData.Data.listQuest[key] == GameData.Instance.persistenceQuest)continue;

            listQuest.Add(GameData.Data.listQuest[key]);
            listProgress.Add(GameData.Data.listProgress[key]);

            worldUI.ReloadQuest(GameData.Data.listQuest[key]);
        }

        GameManager.Instance.globalController.spawnPoint = GameData.Data.newSpawnPoint;

    }

    public void SaveGame()
    {
        ClearOldData();

        for (int i = 0; i < _items.Count; i++)
        {
            GameData.Data.items.Add(_items[i]);
        }

        foreach (var key in listQuest.Keys)
        {
        GameData.Data.dictionaryQuest.Add(dictionaryQuest[key].GetInstanceID(), dictionaryQuest[key]);
        GameData.Data.dictionaryProgress.Add(dictionaryQuest[key].GetInstanceID(), dictionaryProgress[key]);

        }

        mueve el spawn point a la ultima posicion del jugador
        guarda la ulitma posicion para mover el spawn point

        GameManager.Instance.globalController.spawnPoint.TransformPoint(
            GameData.Data.newSpawnPoint.transform.position.x,
            GameData.Data.newSpawnPoint.transform.position.y,
            GameData.Data.newSpawnPoint.transform.position.z);

        GameData.SaveData();

    }

    private void ClearOldData()
    {
        GameData.Data.items.Clear();
        GameData.Data.listQuest.Clear();
        GameData.Data.listProgress.Clear();
        GameData.Data.position.Translate(59.1f, 0f, -49.4f);
    }

    [ContextMenu("Delete Game")]
    public void DeleteGame()
    {
        GameData.DeleteAllData();
        GameData.Data.isDataLoaded = false;
    } */

    #endregion

}