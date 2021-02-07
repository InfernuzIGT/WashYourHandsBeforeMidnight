using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(CanvasGroupUtility))]
public class CanvasCombat : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private Transform _actionsContainer;
    [SerializeField] private List<Actions> _actions;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private CombatConfig _combatConfig = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasUtility = null;
    [SerializeField, ConditionalHide] private TextMeshProUGUI _actionsTxt = null;
    [SerializeField, ConditionalHide] private LocalizeStringEvent _localizeStringEvent = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasTop = null;
    [SerializeField, ConditionalHide] private CanvasGroupUtility _canvasBot = null;

    // private int _lastIndex = 0;
    private LocalizedString _localizedAction;

    // private void Start()
    // {
    //     _actions = new List<Actions>();
    // }

    private void OnEnable()
    {
        EventController.AddListener<CombatActionEvent>(OnCombatAction);
        EventController.AddListener<CombatPlayerEvent>(OnCombatPlayer);
        EventController.AddListener<CombatEnemyEvent>(OnCombatEnemy);
        EventController.AddListener<CombatCreateActionsEvent>(OnCombatCreateActions);
        EventController.AddListener<CombatHideActionsEvent>(OnCombatHideActions);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<CombatActionEvent>(OnCombatAction);
        EventController.RemoveListener<CombatPlayerEvent>(OnCombatPlayer);
        EventController.RemoveListener<CombatCreateActionsEvent>(OnCombatCreateActions);
        EventController.RemoveListener<CombatHideActionsEvent>(OnCombatHideActions);
    }

    private void OnCombatAction(CombatActionEvent evt)
    {
        if (evt.item == null)
        {
            _actionsTxt.enabled = true;
            _localizedAction = _combatConfig.actionSelectEnemy;
        }
        else
        {
            switch (evt.item.type)
            {
                case ITEM_TYPE.WeaponMelee:
                case ITEM_TYPE.WeaponOneHand:
                case ITEM_TYPE.WeaponTwoHands:
                case ITEM_TYPE.ItemGrenade:
                    _actionsTxt.enabled = true;
                    _localizedAction = _combatConfig.actionSelectEnemy;
                    break;

                case ITEM_TYPE.ItemDefense:
                case ITEM_TYPE.ItemHeal:
                    _actionsTxt.enabled = true;
                    _localizedAction = _combatConfig.actionSelectPlayer;
                    break;

                default:
                    _actionsTxt.enabled = false;
                    break;
            }
        }

        _localizeStringEvent.StringReference = _localizedAction;
        _localizeStringEvent.OnUpdateString.Invoke(_actionsTxt.text);
    }

    private void OnCombatCreateActions(CombatCreateActionsEvent evt)
    {
        // Actions tempAction = Instantiate(_combatConfig.actionsPrefab, _actionsContainer);
        // tempAction.Init(evt.equipment);
        // tempAction.gameObject.SetActive(false);
        // _actions.Add(tempAction);

        _actions[0].Init(evt.equipment);
    }

    private void OnCombatHideActions(CombatHideActionsEvent evt)
    {
        // _actions[_lastIndex].gameObject.SetActive(!evt.canHighlight);

        _canvasBot.ShowInstant(!evt.canHighlight);
    }

    private void OnCombatPlayer(CombatPlayerEvent evt)
    {
        ShowPlayerPanel(evt.canSelect, true);
        if (evt.canSelect)ShowActions(evt.combatIndex);
    }

    private void OnCombatEnemy(CombatEnemyEvent evt)
    {
        ShowPlayerPanel(true, false);
    }

    private void ShowActions(int index)
    {
        // _actions[_lastIndex].gameObject.SetActive(false);
        // _actions[index].gameObject.SetActive(true);

        // _actions[index].SelectFirstButton();

        // _lastIndex = index;

        _actions[0].SelectButton();
    }

    private void ShowPlayerPanel(bool show, bool isPlayer)
    {
        _actionsTxt.enabled = show;

        _canvasTop.ShowInstant(show);
        if (isPlayer)_canvasBot.ShowInstant(show);

        _localizedAction = isPlayer ? _combatConfig.actionSelectAction : _combatConfig.actionEnemyTurn;

        _localizeStringEvent.StringReference = _localizedAction;
        _localizeStringEvent.OnUpdateString.Invoke(_actionsTxt.text);
    }

    public void Show(bool isShowing)
    {
        _canvasUtility.ShowInstant(isShowing);
    }

    public void ClearActions()
    {
        _actions.Clear();
    }

}