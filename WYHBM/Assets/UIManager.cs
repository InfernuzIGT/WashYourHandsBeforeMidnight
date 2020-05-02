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
        [Header("Pause Menu")]
        public static bool isGamePaused = false;

        [Header("GameObjects")]
        public GameObject pauseMenuUI;
        public GameObject pauseMenuSprite;
        public GameObject systemUI;
        public GameObject optionsUI;
        public GameObject inventoryUI;
        public GameObject diaryUI;
        public GameObject arrow;

        //bool
        private bool _isInDiary;
        private bool _isInInventory;
        private bool _isInSystem;

        [Header("Panels")]
        public GameObject panelDialog;

        [Header("Player")]
        public Image staminaImg;

        [Header("Dialogues")]
        public TextMeshProUGUI dialogTxt;
        public TextMeshProUGUI continueTxt;
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

        [Header("Items")]
        public Transform itemParents;
        public Inventory inventorySlots;
        private QuestSO _currentQuest;

        private Tween _txtAnimation;

        private EnableMovementEvent _enableMovementEvent;

        // Dialogues
        private string _currentSentence;
        private int _dialogIndex;
        private int _objectivesIndex;
        private bool _isWriting;

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

            _waitStart = new WaitForSeconds(GameData.Instance.gameConfig.timeStart);
            _waitSpace = new WaitForSeconds(GameData.Instance.gameConfig.timeSpace);

            // Resume();

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
            }
        }

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

            GameManager.Instance.StartCoroutine(GameManager.Instance.DeactivateWorldUI());

            // PopUp quest

            GameManager.Instance.AddQuest(data);
            questObjectives[0].text = data.objetives[0];
            questTitleDiaryTxt.text = data.title;
            questTitleTxt.text = data.title;
            questDescriptionTxt.text = data.description;

            SetQuestLog(data);
        }

        public void SetQuestLog(QuestSO data)
        {
            questTitleDiaryTxt.text = data.title;
        }

        public void UpdateObjectives(string objetive, int index)
        {
            questPopup.SetActive(true);

            questPopupTxt.text = questObjectives[index].text;

            questObjectives[index - 1].fontStyle = FontStyles.Strikethrough;

            questPopupTxt.text = questObjectives[index].text;
            questObjectives[index].text = objetive;

            questPopup.SetActive(true);
            questPopupTxt.text = questObjectives[index].text;

            GameManager.Instance.DeactivateWorldUI();
        }

        #endregion

        #region Inventory

        #endregion
        public void EnableCanvas(bool enabled)
        {
            _canvas.enabled = enabled;
        }

        #region Pause Menu
        private void Update() //Move to PlayerController
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isGamePaused)
                {
                    Pause();

                }
                else
                {
                    Resume();
                }

            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isGamePaused = false;

        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isGamePaused = true;

        }
        #region Diary

        public void OnDiaryButton()
        {

            systemUI.SetActive(false);
            optionsUI.SetActive(false);
            inventoryUI.SetActive(false);

            pauseMenuSprite.SetActive(true);
            diaryUI.SetActive(true);

            _isInDiary = true;

        }

        #endregion

        #region Inventory
        public void OnInventoryButton()
        {
            pauseMenuSprite.SetActive(false);
            systemUI.SetActive(false);
            optionsUI.SetActive(false);
            diaryUI.SetActive(false);

            inventoryUI.SetActive(true);

            _isInInventory = true;

        }

        #endregion

        #region System
        public void OnSystemButton()
        {
            diaryUI.SetActive(false);
            optionsUI.SetActive(false);
            inventoryUI.SetActive(false);

            pauseMenuSprite.SetActive(true);
            systemUI.SetActive(true);

            _isInSystem = true;
        }

        public void OnOptionsButton()
        {
            optionsUI.SetActive(true);

            _isInSystem = true;
        }

        public void OnQuitButton()
        {
            Application.Quit();

        }

        #endregion

        #endregion
    }
}