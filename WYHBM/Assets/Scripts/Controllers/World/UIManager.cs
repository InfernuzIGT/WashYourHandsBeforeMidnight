using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;

namespace GameMode.World
{
    public class UIManager : MonoBehaviour
    {
        [Header("Dialogues")]
        public bool isDialogReady;
        public Canvas canvasDialog;
        public TextMeshProUGUI dialogTxt;
        public TextMeshProUGUI continueTxt;
        public float dialogSpeed;
        public DialogSO currentDialog;

        private Tween _txtAnimation;

        private StopMovementEvent _stopMovementEvent;

        // Dialogues
        private bool _isSentenceComplete;
        private string _currentSentence;
        private int _dialogIndex;

        private void Start()
        {
            _stopMovementEvent = new StopMovementEvent();
            canvasDialog.enabled = false;
            continueTxt.enabled = false;
        }

        private void OnEnable()
        {
            EventController.AddListener<UIEnableDialogEvent>(OnEnableDialog);
            EventController.AddListener<UIExecuteDialogEvent>(OnExecuteDialog);
        }

        private void OnDisable()
        {
            EventController.RemoveListener<UIEnableDialogEvent>(OnEnableDialog);
            EventController.RemoveListener<UIExecuteDialogEvent>(OnExecuteDialog);
        }

        #region Events

        private void OnEnableDialog(UIEnableDialogEvent evt)
        {
            isDialogReady = evt.enable;
            currentDialog = evt.dialog;
        }

        private void OnExecuteDialog(UIExecuteDialogEvent evt)
        {
            if (isDialogReady)
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

                if (_dialogIndex == currentDialog.sentences.Length)
                {
                    _dialogIndex = 0;
                    canvasDialog.enabled = false;

                    TurnOffTxt();

                    _stopMovementEvent.enable = true;
                    EventController.TriggerEvent(_stopMovementEvent);
                }
                else
                {
                    SetText();
                    canvasDialog.enabled = true;

                    _stopMovementEvent.enable = false;
                    EventController.TriggerEvent(_stopMovementEvent);
                }
            }
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
    }
}