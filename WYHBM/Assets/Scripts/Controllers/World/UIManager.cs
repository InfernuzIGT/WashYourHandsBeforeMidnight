using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.World
{
    public class UIManager : MonoBehaviour
    {
        public GameObject panelDialog;

        [Header("Player")]
        public Image staminaImg;

        [Header("Dialogues")]
        public TextMeshProUGUI dialogTxt;
        public TextMeshProUGUI continueTxt;
        public float dialogSpeed;
        public DialogSO currentDialog;

        private Tween _txtAnimation;

        private EnableMovementEvent _enableMovementEvent;

        // Dialogues
        private bool _isSentenceComplete;
        private string _currentSentence;
        private int _dialogIndex;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            _enableMovementEvent = new EnableMovementEvent();
            continueTxt.enabled = false;

            panelDialog.SetActive(false);
        }

        private void OnEnable()
        {
            EventController.AddListener<EnableDialogEvent>(OnEnableDialog);
        }

        private void OnDisable()
        {
            EventController.RemoveListener<EnableDialogEvent>(OnEnableDialog);
        }

        #region Events

        // Enable interaction dialog
        private void OnEnableDialog(EnableDialogEvent evt)
        {
            if (evt.enable)
            {
                currentDialog = evt.dialog;
                EventController.AddListener<InteractionEvent>(OnInteractionDialog);
            }
            else
            {
                currentDialog = null;
                EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);
            }
        }

        // Execute dialog
        private void OnInteractionDialog(InteractionEvent evt)
        {
            if (currentDialog.sentences.Length == 0)
            {
                Debug.Log($"Dialog EMPTY");
                return;
            }

            if (_isSentenceComplete)
            {
                CompleteText();
                return;
            }

            // Dialogues
            if (_dialogIndex == currentDialog.sentences.Length)
            {
                _dialogIndex = 0;
                panelDialog.SetActive(false);

                TurnOffTxt();

                _enableMovementEvent.canMove = true;
                EventController.TriggerEvent(_enableMovementEvent);
            }
            else
            {
                SetText();
                panelDialog.SetActive(true);

                _enableMovementEvent.canMove = false;
                EventController.TriggerEvent(_enableMovementEvent);
            }
        }

        #endregion

        #region Player

        public void UpdateStamina(float value)
        {
            staminaImg.fillAmount = value;
        }

        #endregion

        #region  Dialogues

        private void SetText()
        {
            if (_dialogIndex < currentDialog.sentences.Length)
            {
                dialogTxt.text = "";
                ExecuteText();
            }
        }

        private void ExecuteText()
        {
            _isSentenceComplete = true;

            _currentSentence = currentDialog.sentences[_dialogIndex];
            _txtAnimation = dialogTxt.DOText(_currentSentence, dialogSpeed);

            if (_isSentenceComplete)
            {
                TurnOffTxt();
            }

            _dialogIndex++;
        }

        private void CompleteText()
        {
            _txtAnimation.Kill();
            dialogTxt.text = _currentSentence;

            continueTxt.enabled = true;

            _isSentenceComplete = false;
        }

        private void TurnOffTxt()
        {
            continueTxt.enabled = false;
        }

        #endregion

        public void EnableCanvas(bool enabled)
        {
            _canvas.enabled = enabled;
        }
    }
}