using System.Collections;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.World
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject panelDialog;
        // TODO Mariano: Revisar; tira error
        // public GameObject panelQuest; 

        [Header("Player")]
        public Image staminaImg;

        [Header("Dialogues")]
        public TextMeshProUGUI dialogTxt;
        public TextMeshProUGUI continueTxt;
        // public float dialogSpeed;
        public DialogSO currentDialog;
        private char _charSpace = '*';
        private bool _isReading;
        private string _textSkip;
        private Coroutine _coroutineWrite;
        private WaitForSeconds _waitStart;
        private WaitForSeconds _waitSpace;

        [Header("Quest")]
        public TextMeshProUGUI questTitleDiaryTxt;
        public TextMeshProUGUI questTitleTxt;
        public TextMeshProUGUI questDescriptionTxt;
        public TextMeshProUGUI[] questObjectives;
        [Space]
        public TextMeshProUGUI questPopupTxt;

        public TextMeshProUGUI questCompleteTxt;
        public GameObject questComplete;
        public GameObject questPopup;
        private QuestSO _currentQuest;

        private Tween _txtAnimation;

        private EnableMovementEvent _enableMovementEvent;

        // Dialogues
        // private bool _isSentenceComplete;
        private string _currentSentence;
        private int _dialogIndex;
        private int _objectivesIndex;
        private bool _isWriting;

        // Quests

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
            // panelQuest.SetActive (false);

            _waitStart = new WaitForSeconds(GameData.Instance.gameConfig.timeStart);
            _waitSpace = new WaitForSeconds(GameData.Instance.gameConfig.timeSpace);
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
                _currentQuest = evt.dialog.questSO;

                EventController.AddListener<InteractionEvent>(OnInteractionDialog);
            }
            else
            {
                currentDialog = null;
                _currentQuest = null;
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

            // if (_isSentenceComplete)
            // {
            //     CompleteText();
            //     return;
            // }

            //Quest

            // Dialogues
            if (_dialogIndex == currentDialog.sentences.Length)
            {
                SetQuest(_currentQuest);

                _dialogIndex = 0;
                panelDialog.SetActive(false);

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
                if (_isWriting)
                {
                    CompleteText();

                }
                else
                {

                    _isWriting = true;

                    _coroutineWrite = StartCoroutine(WriteText());

                }
                // ExecuteText();
            }
        }

        // private void ExecuteText()
        // {
        //     _isSentenceComplete = true;

        // _txtAnimation = dialogTxt.DOText(_currentSentence, dialogSpeed);

        //     if (_isSentenceComplete)
        //     {
        //         TurnOffTxt();
        //     }

        //     _dialogIndex++;

        // }

        private IEnumerator WriteText()
        {

            _isReading = true;

            _currentSentence = currentDialog.sentences[_dialogIndex];

            _textSkip = ""; // String

            foreach (char c in _currentSentence) // o String
            {
                if (c == _charSpace)
                {
                    _textSkip += ' ';
                }
                else
                {
                    _textSkip += c;
                }
            }

            // Limpia el texto de la UI
            dialogTxt.text = "";

            

            _isReading = false;

            yield return _waitStart; // 0.5f

            foreach (char c in _currentSentence) // o String
            {
                if (c == _charSpace)
                {
                    dialogTxt.text += ' '; // en la UI
                    yield return _waitSpace; // 1f
                }
                else
                {
                    dialogTxt.text += c;
                }

                yield return null;
            }

            CompleteText();
        }

        public void CompleteText()
        {
            if (_isReading)
            {
                return;
            }

            _isWriting = false;

            StopCoroutine(_coroutineWrite);

            dialogTxt.text = _textSkip;

            continueTxt.enabled = true;

            _dialogIndex++;

        }

        #endregion

        #region Quest

        public void SetQuest(QuestSO data)
        {
            data = _currentQuest;

            questPopupTxt.text = questObjectives[_objectivesIndex].text;

            GameManager.Instance.StartCoroutine(GameManager.Instance.DeactivateWorldUI());

            // PopUp quest

            GameManager.Instance.AddQuest(data);
            questObjectives[0].text = data.objetives[0];
            questTitleDiaryTxt.text = data.title;
            questTitleTxt.text = data.title;
            questDescriptionTxt.text = data.description;

            questPopup.SetActive(true);
            questPopupTxt.text = questObjectives[_objectivesIndex].text;

            SetQuestLog(data);
        }

        public void SetQuestLog(QuestSO data)
        {
            questTitleDiaryTxt.text = data.title;
        }

        public void UpdateObjectives(string objetive, int index)
        {

            questObjectives[index - 1].fontStyle = FontStyles.Strikethrough;

            questObjectives[index].text = objetive;

            questPopup.SetActive(true);
            questPopupTxt.text = questObjectives[index].text;

            GameManager.Instance.DeactivateWorldUI();
        }

        #endregion

        public void EnableCanvas(bool enabled)
        {
            _canvas.enabled = enabled;
        }
    }
}