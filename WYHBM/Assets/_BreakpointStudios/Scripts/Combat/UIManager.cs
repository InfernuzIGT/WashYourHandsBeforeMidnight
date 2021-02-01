using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

namespace GameMode.Combat
{
    [RequireComponent(typeof(CanvasGroupUtility))]
    public class UIManager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private CombatConfig _combatConfig = null;
        public TextMeshProUGUI messageTxt;
        public GameObject panelPlayer;
        public GameObject panelEnemy;
        [Space]
        public List<Turn> turn;

        [Header("Actions")]
        public Transform actionsContainer;
        public List<Actions> actions;

        private List<Turn> _turn;
        private CanvasGroupUtility _canvasUtility;
        private Actions _actions;
        private int _lastIndex = 0;

        private void Awake()
        {
            _canvasUtility = GetComponent<CanvasGroupUtility>();
        }

        private void Start()
        {
            actions = new List<Actions>();
            _turn = new List<Turn>();
        }

        private void OnEnable()
        {
            EventController.AddListener<CombatUIEvent>(OnCombatUI);
            EventController.AddListener<CombatActionEvent>(OnCombatAction);
            EventController.AddListener<CombatPlayerEvent>(OnCombatPlayer);
            EventController.AddListener<CombatEnemyEvent>(OnCombatEnemy);
            EventController.AddListener<CombatCreateActionsEvent>(OnCombatCreateActions);
            EventController.AddListener<CombatCreateTurnEvent>(OnCombatCreateTurn);
            EventController.AddListener<CombatHideActionsEvent>(OnCombatHideActions);
        }

        private void OnDisable()
        {
            EventController.RemoveListener<CombatUIEvent>(OnCombatUI);
            EventController.RemoveListener<CombatActionEvent>(OnCombatAction);
            EventController.RemoveListener<CombatPlayerEvent>(OnCombatPlayer);
            EventController.RemoveListener<CombatCreateActionsEvent>(OnCombatCreateActions);
            EventController.RemoveListener<CombatCreateTurnEvent>(OnCombatCreateTurn);
            EventController.RemoveListener<CombatHideActionsEvent>(OnCombatHideActions);
        }

        private void OnCombatAction(CombatActionEvent evt)
        {
            if (evt.item == null)
            {
                messageTxt.text = "Select enemy";
                return;
            }

            switch (evt.item.type)
            {
                case ITEM_TYPE.WeaponMelee:
                    messageTxt.text = "Select enemy";
                    break;

                case ITEM_TYPE.WeaponOneHand:
                    messageTxt.text = "Select enemy";
                    break;

                case ITEM_TYPE.WeaponTwoHands:
                    messageTxt.text = "Select enemy";
                    break;

                case ITEM_TYPE.ItemHeal:
                    messageTxt.text = "Select player";
                    break;

                case ITEM_TYPE.ItemGrenade:
                    messageTxt.text = "Select enemy";
                    break;

                case ITEM_TYPE.ItemDefense:
                    messageTxt.text = "Select player";
                    break;

                default:
                    messageTxt.text = "";
                    break;
            }
        }

        private void OnCombatCreateActions(CombatCreateActionsEvent evt)
        {
            _actions = Instantiate(_combatConfig.actionsPrefab, actionsContainer);
            _actions.Init(evt.equipment);
            _actions.gameObject.SetActive(false);
            actions.Add(_actions);
        }

        private void OnCombatCreateTurn(CombatCreateTurnEvent evt)
        {
            _turn.Clear();

            for (int i = 0; i < evt.listAllCharacters.Count; i++)
            {
                _turn.Add(turn[i]);
                _turn[i].SetSprite(evt.listAllCharacters[i]);
                _turn[i].gameObject.SetActive(true);
            }
        }

        private void OnCombatHideActions(CombatHideActionsEvent evt)
        {
            actions[_lastIndex].gameObject.SetActive(!evt.canHighlight);
        }

        private void OnCombatUI(CombatUIEvent evt)
        {
            // Reorder
            for (int i = 0; i < _turn.Count; i++)
            {
                _turn[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < evt.listWaitingCharacters.Count; i++)
            {
                _turn[i].SetSprite(evt.listWaitingCharacters[i]);
                _turn[i].gameObject.SetActive(true);
            }
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
            actions[_lastIndex].gameObject.SetActive(false);
            actions[index].gameObject.SetActive(true);

            actions[index].SelectFirstButton();

            _lastIndex = index;
        }

        public void Show(bool isShowing)
        {
            _canvasUtility.Show(isShowing);
        }

        // public void EnableCanvas(bool enabled)
        // {
        //     _canvas.enabled = enabled;
        // }

        // public void EnableActions(bool enabled)
        // {
        //     for (int i = 0; i < actions.Count; i++)
        //     {
        //         actions[i].SetButtonsEnable(enabled);
        //     }
        // }

        private void ShowPlayerPanel(bool show, bool isPlayer)
        {
            if (show)
            {
                panelPlayer.SetActive(isPlayer);
                panelEnemy.SetActive(!isPlayer);
                messageTxt.text = isPlayer ? "Select Action" : "Enemy Turn";
            }
            else
            {
                panelPlayer.SetActive(false);
                panelEnemy.SetActive(false);
                messageTxt.text = "";
            }
        }

        public void ClearTurn()
        {
            for (int i = 0; i < turn.Count; i++)
            {
                turn[i].gameObject.SetActive(false);
            }

            _turn.Clear();
        }

    }

}