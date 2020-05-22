using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.World
{
    public class UIManager : MonoBehaviour
    {
        [Header("General")]
        public Image staminaImg;
        public Popup popup;
        [Space]
        public GameObject panelPlayer;
        public GameObject panelPause;
        public GameObject panelDialog;

        [Header("Dialog")]
        public TextMeshProUGUI dialogTxt;
        public TextMeshProUGUI continueTxt;

        [Header("Pause - System")]
        public Image[] arrowsSystem;
        [Space]
        public GameObject system;
        public GameObject systemOptions;

        [Header("Pause - Diary")]
        public Image[] arrowsDiary;
        [Space]
        public GameObject diary;
        public Transform diaryTitleContainer;
        public Transform diaryDescriptionContainer;

        [Header("Pause - Inventory")]
        public Image[] arrowsInventory;
        [Space]
        public GameObject inventory;
        public ItemDescription itemDescription;
        public Transform itemParents;
        public Transform itemEquippedParents;
        public TextMeshProUGUI damageTxt;
        public TextMeshProUGUI defenseTxt;

        // Dialogues
        private char _charSpace = '*';
        private int _dialogIndex;
        private int _objectivesIndex;
        private bool _isWriting;
        private bool _isReading;
        private string _currentSentence;
        private string _textSkip;
        private Coroutine _coroutineWrite;

        // Diary
        private Dictionary<QuestSO, QuestTitle> dicQuestTitle;
        private Dictionary<QuestSO, QuestDescription> dicQuestDescription;
        private GameObject _currentQuestSelected;
        private bool _system;
        private bool _inventory;
        private bool _diary;

        private Tween _txtAnimation;
        private Canvas _canvas;
        private WaitForSeconds _waitStart;
        private WaitForSeconds _waitSpace;
        private WaitForSeconds _waitDeactivateUI;

        private EnableMovementEvent _enableMovementEvent;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            dicQuestTitle = new Dictionary<QuestSO, QuestTitle>();
            dicQuestDescription = new Dictionary<QuestSO, QuestDescription>();

            _enableMovementEvent = new EnableMovementEvent();
            continueTxt.enabled = false;

            _currentQuestSelected = new GameObject();

            panelDialog.SetActive(false);

            _waitStart = new WaitForSeconds(GameData.Instance.gameConfig.timeStart);
            _waitSpace = new WaitForSeconds(GameData.Instance.gameConfig.timeSpace);
            _waitDeactivateUI = new WaitForSeconds(GameData.Instance.gameConfig.messageLifetime);
        }

        public void UpdateStamina(float value)
        {
            staminaImg.fillAmount = value;
        }

        public void EnableCanvas(bool isEnabled)
        {
            _canvas.enabled = isEnabled;
        }

        public void Pause(bool isPaused)
        {
            panelPause.gameObject.SetActive(isPaused);;
        }

        #region Events

        public void OnInteractionDialog(InteractionEvent evt)
        {
            if (GameManager.Instance.CurrentDialog.sentences.Length == 0)
            {
                Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Dialog empty!");
                return;
            }

            // Dialogues
            if (_dialogIndex == GameManager.Instance.CurrentDialog.sentences.Length)
            {
                SetQuest(GameManager.Instance.CurrentQuest);

                _dialogIndex = 0;
                panelDialog.SetActive(false);

                _enableMovementEvent.canMove = true;
                EventController.TriggerEvent(_enableMovementEvent);

                EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);
            }

            else
            {
                PlayText();

                panelDialog.SetActive(true);

                _enableMovementEvent.canMove = false;
                EventController.TriggerEvent(_enableMovementEvent);
            }
        }

        public void PointerEnterDiary()
        {
            _diary = true;
            _inventory = false;
            _system = false;

            arrowsDiary[0].gameObject.SetActive(true);
            arrowsDiary[1].gameObject.SetActive(true);

        }

        public void PointerEnterSystem()
        {
            _diary = true;
            _inventory = false;
            _system = false;

            arrowsSystem[0].gameObject.SetActive(true);
            arrowsSystem[1].gameObject.SetActive(true);

        }

        public void PointerEnterInventory()
        {
            _diary = false;
            _inventory = true;
            _system = false;

            arrowsInventory[0].gameObject.SetActive(true);
            arrowsInventory[1].gameObject.SetActive(true);

        }

        public void PointerExitDiary()
        {
            _diary = false;

            arrowsDiary[0].gameObject.SetActive(false);
            arrowsDiary[1].gameObject.SetActive(false);

        }

        public void PointerExitSystem()
        {
            _system = false;

            arrowsSystem[0].gameObject.SetActive(false);
            arrowsSystem[1].gameObject.SetActive(false);

        }

        public void PointerExitInventory()
        {
            _inventory = false;

            arrowsInventory[0].gameObject.SetActive(false);
            arrowsInventory[1].gameObject.SetActive(false);

        }

        #endregion

        #region  Dialogues

        public void PlayText()
        {
            if (_dialogIndex < GameManager.Instance.CurrentDialog.sentences.Length)
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

            _currentSentence = GameManager.Instance.CurrentDialog.sentences[_dialogIndex];

            _textSkip = "";

            foreach (char c in _currentSentence)
            {
                _textSkip += c == _charSpace ? ' ' : c;
            }

            dialogTxt.text = "";

            _isReading = false;

            yield return _waitStart;

            foreach (char c in _currentSentence)
            {
                if (c == _charSpace)
                {
                    dialogTxt.text += ' ';
                    yield return _waitSpace;
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
            if (data == null)
            {
                return;
            }

            GameManager.Instance.AddQuest(data);
            ShowPopup(string.Format(GameData.Instance.textConfig.popupNewQuest, data.title));

            // Create Quest Title
            QuestTitle questTitle = Instantiate(GameData.Instance.gameConfig.questTitlePrefab, diaryTitleContainer);
            questTitle.Init(data);
            dicQuestTitle.Add(data, questTitle);

            // Create Quest Description
            QuestDescription questDescription = Instantiate(GameData.Instance.gameConfig.questDescriptionPrefab, diaryDescriptionContainer);
            questDescription.Init(data);
            dicQuestDescription.Add(data, questDescription);
            SelectQuest(data);
        }

        public void SelectQuest(QuestSO data)
        {
            _currentQuestSelected.gameObject.SetActive(false);
            _currentQuestSelected = dicQuestDescription[data].gameObject;
            _currentQuestSelected.gameObject.SetActive(true);
        }

        public void UpdateQuest(QuestSO data, int progress)
        {
            dicQuestDescription[data].UpdateObjetives();

            if (progress == data.objetives.Length)
            {
                ShowPopup(string.Format(GameData.Instance.textConfig.popupCompleted, data.title));
                dicQuestTitle[data].Complete();
            }
            else
            {
                ShowPopup(string.Format(GameData.Instance.textConfig.popupNewObjetive, data.objetives[progress]));
            }
        }

        // public void ShowQuest()
        // {
            // for (int i = 0; i < GameManager.Instance.dictionaryQuest.Count; i++)
            // {
            //     QuestTitle questTitle = Instantiate(GameData.Instance.gameConfig.questTitlePrefab, diaryTitleContainer);
            //     questTitle.Init(GameManager.Instance.dictionaryQuest[i]);
            //     dicQuestTitle.Add(GameManager.Instance.dictionaryQuest[i], questTitle);


            // }

            // QuestDescription questDescription = Instantiate(GameData.Instance.gameConfig.questDescriptionPrefab, diaryDescriptionContainer);
            // questDescription.Init(GameManager.Instance.dicti);
            // dicQuestDescription.Add(data, questDescription);
            // SelectQuest(data);

        // }

        #endregion

        public void ShowPopup(string text, bool canRepeat = true)
        {
            if (!canRepeat && popup.gameObject.activeSelf)
            {
                return;
            }

            popup.gameObject.SetActive(false);
            popup.SetTitle(text);
            popup.gameObject.SetActive(true);
        }

        public void MenuPause(BUTTON_TYPE buttonType)
        {

            switch (buttonType)
            {
                case BUTTON_TYPE.Diary:
                    system.SetActive(false);
                    systemOptions.SetActive(false);
                    inventory.SetActive(false);

                    diary.SetActive(true);
                    break;

                case BUTTON_TYPE.Inventory:
                    system.SetActive(false);
                    systemOptions.SetActive(false);
                    diary.SetActive(false);

                    inventory.SetActive(true);
                    break;

                case BUTTON_TYPE.System:
                    diary.SetActive(false);
                    systemOptions.SetActive(false);
                    inventory.SetActive(false);

                    system.SetActive(true);
                    break;

                case BUTTON_TYPE.Resume:
                    GameManager.Instance.SetPause();
                    break;

                case BUTTON_TYPE.Options:
                    systemOptions.SetActive(true);
                    break;

                case BUTTON_TYPE.Quit:
                    // GameData.Instance.LoadScene(SCENE_INDEX.MainMenu);
                    Application.Quit();
                    break;

                default:
                    break;
            }
        }

    }
}