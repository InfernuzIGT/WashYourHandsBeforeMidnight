using TMPro;
using UnityEngine;

namespace GameMode.Combat
{
    public class UIController : MonoBehaviour
    {
        [Header("Menu")]
        public GameObject menuAction;
        public GameObject menuTurn;
        public GameObject exitButton;

        [Header("Action")]
        public ACTION_TYPE actualActionType;
        [Space]
        public Transform panelActions;
        [Space]
        public TextMeshProUGUI titleTxt;
        public TextMeshProUGUI descriptionTxt;
        public TextMeshProUGUI informationTxt;
        [Space]
        public TextMeshProUGUI turnTxt;

        public void ChooseAction(ActionSO _action)
        {
            titleTxt.text = _action.title;
            descriptionTxt.text = _action.description;
            informationTxt.text = _action.information;
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
            CombatManager.Instance.actionController.Attack();
        }

        public void Defense()
        {
            CombatManager.Instance.actionController.Defense();
        }

        public void Item()
        {
            CombatManager.Instance.actionController.Item();
        }

        public void Run()
        {
            CombatManager.Instance.actionController.Run();
        }

    }

}