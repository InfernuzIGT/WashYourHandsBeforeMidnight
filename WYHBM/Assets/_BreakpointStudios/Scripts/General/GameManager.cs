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
    // public CombatController combatManager;
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

    // Coroutines
    private Coroutine _coroutineEnconters;

    // Events
    private FadeEvent _fadeEvent;

    // Properties
    private List<ItemSO> _items;
    public List<ItemSO> Items { get { return _items; } }

    // public bool IsInventoryFull { get { return _items.Count == _inventoryMaxSlots; } }
    // public bool IsEquipmentFull { get { return combatPlayers[_characterIndex].equipment.Count == _equipmentMaxSlots; } }

    // private QuestData _currentQuestData;
    // public QuestData CurrentQuestData { get { return _currentQuestData; } }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _items = new List<ItemSO>();

        listSlots = new List<Slot>();

        _fadeEvent = new FadeEvent();
        _fadeEvent.fast = true;

        // _characterIndex = 0;
        // worldUI.ChangeCharacter(combatPlayers[_characterIndex], _characterIndex, inLeftLimit : true);

        inCombat = false;

        // CheckEncounters(true);
    }

    // private void OnEnable()
    // {
    //     // EventController.AddListener<EnterCombatEvent>(OnEnterCombat);
    //     // EventController.AddListener<ExitCombatEvent>(OnExitCombat);
    //     // EventController.AddListener<CutsceneEvent>(OnCutscene);

    // }

    // private void OnDisable()
    // {
    //     // EventController.RemoveListener<EnterCombatEvent>(OnEnterCombat);
    //     // EventController.RemoveListener<ExitCombatEvent>(OnExitCombat);
    //     // EventController.RemoveListener<CutsceneEvent>(OnCutscene);
    // }

    // public void CheckEncounters(bool isEnabled)
    // {
    //     if (isEnabled)
    //     {
    //         // _coroutineEnconters = StartCoroutine(CheckEncounter());
    //     }
    //     else
    //     {
    //         StopCoroutine(_coroutineEnconters);
    //         _coroutineEnconters = null;
    //     }
    // }

    // private IEnumerator CheckEncounter()
    // {
    //     yield return new WaitForSeconds(3f);

    //     while (true)
    //     {
    //         if (!inCombat && !isPaused && globalController.GetPlayerInMovement())
    //         {
    //             _currentTimeEncounter += Time.deltaTime;

    //             if (_currentTimeEncounter > _limitTimeEncounter)
    //             {
    //                 _currentTimeEncounter = 0;
    //                 TriggerCombat();
    //             }
    //         }
    //         yield return null;
    //     }
    // }

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

    // private void SwitchAmbient()
    // {
    //     CheckEncounters(!inCombat);

    //     // globalController.playerController.ToggleInputWorld(!inCombat);
    //     globalController.HidePlayer(inCombat);

    //     combatUI.Show(inCombat);
    //     worldUI.Show(!inCombat);

    //     // combatManager.ToggleInputCombat(inCombat);
    //     combatManager.SetCombatArea(inCombat);

    //     if (!inCombat)
    //     {
    //         globalController.ChangeToCombatCamera(null);
    //         combatUI.actions.Clear();
    //         combatUI.ClearTurn();
    //     }
    //     else
    //     {
    //         globalController.ChangeToCombatCamera(_currentCombatArea.virtualCamera);
    //     }
    // }

    // private void SwitchMovement()
    // {
    //     // globalController.playerController.SwitchMovement();
    // }

    // private void StartCombat()
    // {
    //     // currentNPC?.Kill();
    //     combatManager.InitiateTurn();
    // }

    // public void ReorderTurn()
    // {
    //     combatUI.ReorderTurn(combatManager.ListWaitingCharacters);
    // }

    // public void SelectButton(GameObject button)
    // {
    //     eventSystem.SetSelectedGameObject(button);
    // }

    // public void PlayerCanSelect(bool canSelect, int combatIndex = 0)
    // {
    //     combatManager.ChangeCombatState(canSelect);
    //     combatUI.ShowPlayerPanel(canSelect, true);
    //     if (canSelect)combatUI.ShowActions(combatIndex);
    // }

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

    #region Events

    // public void OnEnterCombat(EnterCombatEvent evt)
    // {
    //     inCombat = true;
    //     currentNPC = evt.currentNPC;

    //     int indexArea = Random.Range(0, combatAreas.Length);
    //     _currentCombatArea = combatAreas[indexArea];
    //     combatManager.SetData(_currentCombatArea, combatPlayers, evt.npc.combatEnemies);

    //     _fadeEvent.callbackStart = SwitchMovement;
    //     _fadeEvent.callbackMid = SwitchAmbient;
    //     _fadeEvent.callbackEnd = StartCombat;

    //     EventController.TriggerEvent(_fadeEvent);
    // }

    // private void TriggerCombat()
    // {
    //     if (!ProportionValue.GetProbability(0.4f))return;

    //     inCombat = true;
    //     currentNPC = null;

    //     int indexArea = Random.Range(0, combatAreas.Length);
    //     _currentCombatArea = combatAreas[indexArea];

    //     int indexEncounter = Random.Range(0, enemyEncounters.Count);
    //     combatManager.SetData(_currentCombatArea, combatPlayers, enemyEncounters[indexEncounter].enemies);

    //     _fadeEvent.callbackStart = SwitchMovement;
    //     _fadeEvent.callbackMid = SwitchAmbient;
    //     _fadeEvent.callbackEnd = StartCombat;

    //     EventController.TriggerEvent(_fadeEvent);
    // }

    // public void OnExitCombat(ExitCombatEvent evt)
    // {
    //     inCombat = false;

    //     _fadeEvent.callbackStart = null;
    //     _fadeEvent.callbackMid = SwitchAmbient;
    //     _fadeEvent.callbackEnd = SwitchMovement;

    //     EventController.TriggerEvent(_fadeEvent);
    // }

    // private void OnCutscene(CutsceneEvent evt)
    // {
    //     // Debug.Log($"<b> {evt.cutscene.name} </b>");
    //     playableDirector.playableAsset = evt.cutscene;

    //     playableDirector.Play();
    // }

    #endregion

    

}