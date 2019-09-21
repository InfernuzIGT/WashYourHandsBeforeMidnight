using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    public class UIController : MonoBehaviour
    {
        [Header("Action")]
        public ACTION_TYPE actualActionType;

        [Header("Menu")]
        public bool isOpenActionMenu = false;
        public bool isOpenActionRadialMenu = false;
        public GameObject menuWeapon;
        public GameObject menuDefense;
        public GameObject menuItem;

        [Header("GameObjects")]
        public GameObject WIN;
        public GameObject LOSE;
        public GameObject Action1;
        public GameObject Action2;
        public GameObject Action3;
        public GameObject UI;

        public Image barHealthEnemy;
        public Image barHealthPlayer;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                OpenRadialMenu();
            }
        }

        public void CloseActionMenu()
        {
            if (isOpenActionMenu)
            {
                isOpenActionMenu = false;
                menuWeapon.SetActive(false);
                menuItem.SetActive(false);
                menuDefense.SetActive(false);
            }
        }

        public void CloseActionRadialMenu()
        {
            if (isOpenActionRadialMenu)
            {
                isOpenActionRadialMenu = false;
                UI.SetActive(false);
            }
        }

        #region Options

        public void Attack()
        {
            CloseActionRadialMenu();

            Action1.SetActive(false);
            Action2.SetActive(true);

            CombatManager.Instance.actionController.Attack();
        }
        public void Defense()
        {
            CloseActionRadialMenu();

            CombatManager.Instance.actionController.Defense();
        }
        public void Item()
        {
            CloseActionRadialMenu();

            Action1.SetActive(false);
            Action3.SetActive(true);

            CombatManager.Instance.actionController.Item();
        }

        public void Run()
        {
            CloseActionRadialMenu();

            CombatManager.Instance.actionController.Run();
        }
        public void Exit()
        {
            CloseActionRadialMenu();

            Action1.SetActive(false);

            CombatManager.Instance.actionController.Exit();
        }

        #endregion

        // Actions Selector -------------------------------------------------------------------------

        public void SelectAction(ACTION_TYPE actionType)
        {
            actualActionType = actionType;
            isOpenActionMenu = true;

            switch (actionType)
            {
                case ACTION_TYPE.weapon:
                    menuWeapon.SetActive(true);
                    menuItem.SetActive(false);
                    menuDefense.SetActive(false);
                    break;

                case ACTION_TYPE.item:
                    menuWeapon.SetActive(false);
                    menuItem.SetActive(true);
                    menuDefense.SetActive(false);
                    break;

                case ACTION_TYPE.defense:
                    menuWeapon.SetActive(false);
                    menuItem.SetActive(false);
                    menuDefense.SetActive(true);
                    break;

                default:
                    break;
            }
        }

        public void SelectOption(OPTION_TYPE optionType)
        {

            switch (actualActionType)
            {
                case ACTION_TYPE.weapon:
                    OptionWeapon(optionType);
                    break;

                case ACTION_TYPE.item:
                    OptionItem(optionType);
                    break;

                case ACTION_TYPE.defense:
                    OptionDefense(optionType);
                    break;

                default:
                    break;
            }

            isOpenActionMenu = false;
        }

        private void OptionWeapon(OPTION_TYPE optionType)
        {
            switch (optionType)
            {
                case OPTION_TYPE.option1:
                    Debug.Log($"Weapon - Option 1");
                    break;

                case OPTION_TYPE.option2:
                    Debug.Log($"Weapon - Option 2");
                    break;

                case OPTION_TYPE.option3:
                    Debug.Log($"Weapon - Option 3");
                    break;

                default:
                    break;
            }

            menuWeapon.SetActive(false);
        }

        private void OptionDefense(OPTION_TYPE optionType)
        {
            switch (optionType)
            {
                case OPTION_TYPE.option1:
                    Debug.Log($"Defense - Option 1");
                    break;

                case OPTION_TYPE.option2:
                    Debug.Log($"Defense - Option 2");
                    break;

                case OPTION_TYPE.option3:
                    Debug.Log($"Defense - Option 3");
                    break;

                default:
                    break;
            }

            menuDefense.SetActive(false);
        }

        private void OptionItem(OPTION_TYPE optionType)
        {
            switch (optionType)
            {
                case OPTION_TYPE.option1:
                    Debug.Log($"Item - Option 1");
                    break;

                case OPTION_TYPE.option2:
                    Debug.Log($"Item - Option 2");
                    break;

                case OPTION_TYPE.option3:
                    Debug.Log($"Item - Option 3");
                    break;

                default:
                    break;
            }

            menuItem.SetActive(false);
        }

        //-------------------------------------------------------------------------

        private void OpenRadialMenu()
        {
            isOpenActionRadialMenu = true;

            UI.transform.position = Input.mousePosition;
            UI.SetActive(true);
            Action1.SetActive(true);
            Action2.SetActive(false);
            Action3.SetActive(false);

        }

        public void UpdateBarEnemy(float actualHealthEnemy)
        {
            barHealthEnemy.fillAmount = actualHealthEnemy;
            if (actualHealthEnemy == 0)
            {
                WIN.SetActive(true);
            }
        }

        public void UpdateBarPlayer(float actualHealthPlayer)
        {
            barHealthEnemy.fillAmount = actualHealthPlayer;
            if (actualHealthPlayer == 0)
            {
                LOSE.SetActive(true);
            }

        }

    }

}