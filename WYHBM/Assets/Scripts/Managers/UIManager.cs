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
        public Transform objectivesPanel;
        public GameObject questObjectivesPanel;
        public TextMeshProUGUI questObjectivesPanelTxt;
        public TextMeshProUGUI[] questObjectives;
        [Space]
        public TextMeshProUGUI questPopupTxt;
        public TextMeshProUGUI questCompleteTxt;
        public GameObject questComplete;
        public GameObject questPopup;
        public GameObject questTitles;
        public GameObject questGroup;
        public Transform questTitlesParent;

        [Header("Items")]
        public Transform itemParents;
        public Transform itemEquippedParents;
        public Inventory inventorySlots;
        public GameObject itemInfo;
        public Image itemSprite;
        public TextMeshProUGUI itemDescription;
        public GameObject damageTxtDescription;
        public TextMeshProUGUI damageIntTxtDescription;
        public GameObject inventoryPopUp;
        public TextMeshProUGUI inventoryPopUpTxt;
        public bool onMouseOver;

        private Tween _txtAnimation;

        private EnableMovementEvent _enableMovementEvent;
        private QuestSO _currentQuest;

        // Dialogues
        private string _currentSentence;
        private int _dialogIndex;
        private int _objectivesIndex;
        private bool _isWriting;

        //bool
        private bool _isInDiary;
        private bool _isInInventory;
        private bool _isInSystem;

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

            Resume();

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
                EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);

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

        // public void OnClickQuest(bool _isOpen)
        // {
        //     // SetQuestInformation(_currentQuest);
        // }

        // public void SetQuestInformation(QuestSO data)
        // {
        //     // Actualiza informacion de la UI con data

        //     // questObjectives[0].text = data.objetives[0];
        //     // questTitleDiaryTxt.text = data.title;
        //     // questTitleTxt.text = data.title;
        //     // questDescriptionTxt.text = data.description;
            
        // }
        public void SetQuest(QuestSO data)
        {

            if (data == null)
            {
                Debug.Log($"<b> QUEST EMPTY </b>");
                return;
            }

            // PopUp quest
            questPopup.SetActive(true);
            questPopupTxt.DOFade(1, GameData.Instance.gameConfig.fadeSlowDuration);

            Instantiate(questTitles, questTitlesParent);

            GameManager.Instance.AddQuest(data);

            questPopupTxt.text = data.objetives[0];

            questObjectives[0].text = data.objetives[0];
            questTitleDiaryTxt.text = data.title;
            questTitleTxt.text = data.title;
            questDescriptionTxt.text = data.description;

            GameManager.Instance.StartCoroutine(GameManager.Instance.DeactivateWorldUI());

            SetQuestLog(data);
        }

        public void SetQuestLog(QuestSO data)
        {
            questTitleDiaryTxt.text = data.title;
        }

        public void UpdateObjectives(string objetive, int index)
        {
            questPopup.SetActive(true);
            questPopupTxt.DOFade(1, GameData.Instance.gameConfig.fadeSlowDuration);

            questObjectives[index - 1].fontStyle = FontStyles.Strikethrough;

            questObjectives[index].text = objetive;
            questPopupTxt.text = objetive;

            GameManager.Instance.StartCoroutine(GameManager.Instance.DeactivateWorldUI());
        }

        public void FadeOutUI()
        {
            questPopupTxt.DOFade(0, GameData.Instance.gameConfig.fadeSlowDuration);
            questCompleteTxt.DOFade(0, GameData.Instance.gameConfig.fadeSlowDuration);
            inventoryPopUpTxt.DOFade(0, GameData.Instance.gameConfig.fadeSlowDuration);

        }

        #endregion

        #region Inventory

        public void SetItemInformation(ItemSO item)
        {
            damageIntTxtDescription.text = item.valueMin.ToString();
            itemSprite.sprite = item.previewSprite;
            itemDescription.text = item.description;

            if (onMouseOver)
            {
                itemInfo.SetActive(true);
                damageTxtDescription.SetActive(true);
            }

            else
            {
                itemInfo.SetActive(false);
            }

        }

        public void InventoryPopUp()
        {
            inventoryPopUp.SetActive(true);
            inventoryPopUpTxt.DOFade(1, GameData.Instance.gameConfig.fadeFastDuration);

            GameManager.Instance.StartCoroutine(GameManager.Instance.DeactivateWorldUI());

        }

        public void TakeOffItemInformation()
        {
            itemInfo.SetActive(false);
        }

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
            GameData.Instance.LoadScene(SCENE_INDEX.MainMenu);
        }

        #endregion

        #endregion
    }
}