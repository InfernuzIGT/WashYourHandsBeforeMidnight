using System.Collections;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    public class UIController : MonoBehaviour
    {
        [Header("Menu")]
        public GameObject menuAction;
        public GameObject menuTurn;
        public GameObject exitButton;

        [Header("Panels")]
        public Image fadeScreen;
        public TextMeshProUGUI panelTxt;

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


        private void Start()
        {

        }

        private void OnEnable()
        {
            EventController.AddListener<FadeInEvent>(FadeIn);
            EventController.AddListener<FadeOutEvent>(FadeOut);
        }
        private void OnDisable()
        {
            EventController.RemoveListener<FadeInEvent>(FadeIn);
            EventController.RemoveListener<FadeOutEvent>(FadeOut);

        }
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

        #region Fade

        private void FadeIn(FadeInEvent evt)
        {
            // TODO Mariano: Add start text
            // panelTxt.text = evt.text;           
            StartCoroutine(StartFadeIn(evt.duration));
        }        
        
        private void FadeOut(FadeOutEvent evt)
        {
            // TODO Mariano: Add end text
            // panelTxt.text = evt.text;           
            StartCoroutine(StartFadeOut(evt.duration));
        }

        private IEnumerator StartFadeIn(float duration)
        {
            fadeScreen.DOFade(1, duration);
            yield return null;
        }
        
        private IEnumerator StartFadeOut(float duration)
        {
            fadeScreen.DOFade(0, duration);
            yield return null;
        }
        

        #endregion
    }

}