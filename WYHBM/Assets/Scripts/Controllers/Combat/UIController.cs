using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    public class UIController : MonoBehaviour
    {
        [Header("General")]
        public CanvasGroup canvasGroup;
        public Image fadeScreen;
        public TextMeshProUGUI endTxt;
        [Space]
        public GameObject menuAction;
        public GameObject menuTurn;
        public GameObject buttonExit;
        public GameObject CanvasPlay;
        public GameObject CanvasOptions;
        public GameObject CanvasExit;
        public GameObject CanvasMenu;

        [Header("Action")]
        public ACTION_TYPE actualActionType;
        [Space]
        public PanelActions panelActions;
        [Space]
        [SerializeField] private TextMeshProUGUI _playTxt = null;
        [SerializeField] private TextMeshProUGUI _descriptionTxt = null;
        [SerializeField] private TextMeshProUGUI _informationTxt = null;
        [Space]
        public TextMeshProUGUI turnTxt;

        private string _informationType;
        private ActionObject _actionObject;

        private List<ActionObject> _actionObjects;

        private void Start()
        {
            _playTxt.text = GameData.Instance.textConfig.playActionTxt;
        }

        public void CreateActionObjects(List<EquipmentSO> _equipment)
        {
            _actionObjects = new List<ActionObject>();

            for (int i = 0; i < _equipment.Count; i++)
            {
                _actionObject = Instantiate(GameData.Instance.combatConfig.actionObjectPrefab, panelActions.transform);
                _actionObject.equipment = _equipment[i];
                _actionObjects.Add(_actionObject);
            }

            _actionObjects[0].SelectAction();
        }

        public void ChooseAction(EquipmentSO _equipment)
        {
            _descriptionTxt.text = _equipment.actionDescription;

            switch (_equipment.actionType)
            {
                case ACTION_TYPE.weapon:
                    _informationType = GameData.Instance.textConfig.actionTypeWeapon;
                    break;

                case ACTION_TYPE.itemPlayer:
                    _informationType = null;
                    break;

                case ACTION_TYPE.itemEnemy:
                    _informationType = null;
                    break;

                case ACTION_TYPE.defense:
                    _informationType = GameData.Instance.textConfig.actionTypeDefense;
                    break;

                default:
                    Debug.LogError($"<color=red><b>[ERROR]</b></color> \"None\" in \"GetEquipmentText\"");
                    _informationType = "";
                    break;
            }

            if (_informationType != null)
            {
                if (_equipment.valueMin == _equipment.valueMax)
                {
                    _informationTxt.text = string.Format(
                        GameData.Instance.textConfig.informationOneText,
                        _informationType,
                        _equipment.valueMax);
                }
                else
                {
                    _informationTxt.text = string.Format(
                        GameData.Instance.textConfig.informationTwoText,
                        _informationType,
                        _equipment.valueMin,
                        _equipment.valueMax);
                }
            }
            else
            {
                _informationTxt.text = "";
            }
        }

        public void SelectAction(ACTION_TYPE actionType)
        {
            actualActionType = actionType;
        }

        public void SelectOption(OPTION_TYPE optionType)
        {
            switch (actualActionType)
            {
                case ACTION_TYPE.weapon:
                    break;

                case ACTION_TYPE.itemPlayer:
                case ACTION_TYPE.itemEnemy:
                    break;

                case ACTION_TYPE.defense:
                    break;

                default:
                    break;
            }
        }

        public void Attack()
        {
            // CombatManager.Instance.actionController.Attack();
        }

        public void Defense()
        {
            // CombatManager.Instance.actionController.Defense();
        }

        public void Item()
        {
            // CombatManager.Instance.actionController.Item();
        }

        public void Run()
        {
            // CombatManager.Instance.actionController.Run();
        }

        public void ChangeUI(bool isPlayer)
        {
            menuAction.SetActive(isPlayer);
            menuTurn.SetActive(!isPlayer);
            // buttonExit.SetActive(isPlayer);
        }

        public void Menu(bool isPaused)
        {
            CanvasMenu.SetActive(isPaused);
        }

    }
}