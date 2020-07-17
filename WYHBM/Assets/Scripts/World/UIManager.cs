using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.World
{
    public class UIManager : MonoBehaviour
    {
        [Header("General")]
        // public Image staminaImg;
        public Popup popup;
        [Space]
        public GameObject panelPlayer;
        public GameObject panelPause;
        public GameObject panelDialog;
        public GameObject panelNote;

        [Header("Dialog")]
        public TextMeshProUGUI dialogTxt;
        public TextMeshProUGUI continueTxt;

        [Header("Note")]
        public TextMeshProUGUI noteTxt;

        [Header("Pause - System")]
        public GameObject system;
        public GameObject systemOptions;

        [Header("Pause - Diary")]
        public GameObject diary;
        public Transform diaryTitleContainer;
        public Transform diaryDescriptionContainer;

        [Header("Pause - Inventory")]
        public GameObject inventory;
        public ItemDescription itemDescription;
        public Transform itemParents;
        public Button buttonLeft;
        public Button buttonRight;
        [Space]
        public TextMeshProUGUI characterNameTxt;
        public Image characterImg;
        public Transform[] characterSlot = new Transform[4];

        [Header("FMOD")]
        public Slider sliderSound;
        public Slider sliderMusic;
        [Space]

        // Inventory
        private int _lastSlot = 0;

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
        private bool _isComplete;

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

            buttonLeft.onClick.AddListener(() => GameManager.Instance.NextCharacter(true));
            buttonRight.onClick.AddListener(() => GameManager.Instance.NextCharacter(false));

            sliderSound.onValueChanged.AddListener(VolumeSound);
            sliderMusic.onValueChanged.AddListener(VolumeMusic);

            // buttonLeft.interactable = false;
            // buttonRight.interactable = false;

            _waitStart = new WaitForSeconds(GameData.Instance.worldConfig.timeStart);
            _waitSpace = new WaitForSeconds(GameData.Instance.worldConfig.timeSpace);
            _waitDeactivateUI = new WaitForSeconds(GameData.Instance.worldConfig.messageLifetime);

            // GameData.Instance.persistenceItem
        }

        // public void UpdateStamina(float value)
        // {
        //     staminaImg.fillAmount = value;
        // }

        public void EnableCanvas(bool isEnabled)
        {
            _canvas.enabled = isEnabled;
        }

        public void Pause(bool isPaused)
        {
            panelPause.gameObject.SetActive(isPaused);;
        }

        public void ChangeCharacter(CombatPlayer combatPlayer, int index, bool inLeftLimit = false, bool inRightLimit = false)
        {
            characterNameTxt.text = combatPlayer.character.Name;
            characterImg.sprite = combatPlayer.character.PreviewSprite;

            characterSlot[_lastSlot].gameObject.SetActive(false);
            characterSlot[index].gameObject.SetActive(true);

            _lastSlot = index;

            buttonLeft.interactable = !inLeftLimit;
            buttonRight.interactable = !inRightLimit;
        }

        public void VolumeSound(float vol)
        {
            RuntimeManager.StudioSystem.setParameterByName("SoundsSlider", vol);
        }

        public void VolumeMusic(float vol)
        {
            RuntimeManager.StudioSystem.setParameterByName("MusicSlider", vol);
        }

        public Transform GetSlot()
        {
            return characterSlot[_lastSlot];
        }

        #region Events

        public void OnInteractionDialog(InteractionEvent evt)
        {
            switch (GameManager.Instance.CurrentQuestData.state)
            {

                case QUEST_STATE.Ready:

                    if (GameManager.Instance.CurrentDialog.readySentences == null)
                    {
                        GameManager.Instance.CurrentQuestData.state = QUEST_STATE.Ready;

                        return;
                    }

                    if (_dialogIndex == GameManager.Instance.CurrentDialog.readySentences.Length)
                    {
                        SetQuest(GameManager.Instance.CurrentQuestData.quest);

                        panelDialog.SetActive(false);

                        _dialogIndex = 0;
                        _enableMovementEvent.canMove = true;

                        EventController.TriggerEvent(_enableMovementEvent);
                        EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);

                        // GameManager.Instance.CurrentQuestData.state = QUEST_STATE.InProgress;

                        return;
                    }

                    else
                    {
                        ExecuteText(GameManager.Instance.CurrentDialog.readySentences, _dialogIndex);
                    }

                    break;

                case QUEST_STATE.InProgress:

                    if (GameManager.Instance.CurrentDialog.inProgressSentences == null)
                    {
                        GameManager.Instance.CurrentQuestData.state = QUEST_STATE.Completed;

                        return;
                    }

                    if (_dialogIndex == GameManager.Instance.CurrentDialog.inProgressSentences.Length)
                    {
                        panelDialog.SetActive(false);

                        _dialogIndex = 0;
                        _enableMovementEvent.canMove = true;

                        EventController.TriggerEvent(_enableMovementEvent);
                        EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);

                        return;
                    }

                    else
                    {
                        ExecuteText(GameManager.Instance.CurrentDialog.inProgressSentences, _dialogIndex);
                    }

                    break;

                case QUEST_STATE.Completed:

                    if (GameManager.Instance.CurrentDialog.CompletedSentences == null)
                    {
                        GameManager.Instance.CurrentQuestData.state = QUEST_STATE.None;

                        return;
                    }

                    if (_dialogIndex == GameManager.Instance.CurrentDialog.CompletedSentences.Length)
                    {
                        panelDialog.SetActive(false);

                        _dialogIndex = 0;
                        _enableMovementEvent.canMove = true;

                        EventController.TriggerEvent(_enableMovementEvent);
                        EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);

                        GameManager.Instance.CurrentQuestData.state = QUEST_STATE.None;

                        return;
                    }

                    else
                    {
                        ExecuteText(GameManager.Instance.CurrentDialog.CompletedSentences, _dialogIndex);
                    }

                    break;

                case QUEST_STATE.None:

                    Debug.Log($"<b> QUEST NONE </b>");

                    if (_dialogIndex == GameManager.Instance.CurrentDialog.noneSentences.Length)
                    {
                        panelDialog.SetActive(false);

                        _dialogIndex = 0;
                        _enableMovementEvent.canMove = true;

                        EventController.TriggerEvent(_enableMovementEvent);
                        EventController.RemoveListener<InteractionEvent>(OnInteractionDialog);

                        return;
                    }

                    else
                    {
                        ExecuteText(GameManager.Instance.CurrentDialog.noneSentences, _dialogIndex);
                    }

                    break;
            }

        }

        public void ExecuteText(string[] sentences, int index)
        {
            _currentSentence = sentences[index];

            PlayText(sentences, index);

            panelDialog.SetActive(true);

            _enableMovementEvent.canMove = false;
            EventController.TriggerEvent(_enableMovementEvent);

        }

        #endregion

        #region 
        public void ActiveNote(bool _isActiveNote)
        {
            if (_isActiveNote)
            {
                panelNote.SetActive(false);

            }
            else
            {
                panelNote.SetActive(true);
            }
        }

        #endregion

        #region Dialogues

        public void PlayText(string[] sentence, int index)
        {
            if (_dialogIndex < sentence[index].Length)
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
            QuestTitle questTitle = Instantiate(GameData.Instance.worldConfig.questTitlePrefab, diaryTitleContainer);
            questTitle.Init(data);
            dicQuestTitle.Add(data, questTitle);

            // Create Quest Description
            QuestDescription questDescription = Instantiate(GameData.Instance.worldConfig.questDescriptionPrefab, diaryDescriptionContainer);
            questDescription.Init(data);
            dicQuestDescription.Add(data, questDescription);
            SelectQuest(data);

            GameManager.Instance.CurrentQuestData.state = QUEST_STATE.InProgress;
        }

        public void ReloadQuest(QuestSO data)
        {
            // Create Quest Title
            QuestTitle questTitle = Instantiate(GameData.Instance.worldConfig.questTitlePrefab, diaryTitleContainer);
            questTitle.Init(data);
            dicQuestTitle.Add(data, questTitle);

            // Create Quest Description
            QuestDescription questDescription = Instantiate(GameData.Instance.worldConfig.questDescriptionPrefab, diaryDescriptionContainer);
            questDescription.Init(data);
            dicQuestDescription.Add(data, questDescription);

            Debug.Log($"<b> Quest: {data.title} </b>");
        }

        public void SelectQuest(QuestSO data)
        {
            Debug.Log($"<b> Selected </b>");
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

                GameManager.Instance.CurrentQuestData.state = QUEST_STATE.Completed;
            }
            else
            {
                ShowPopup(string.Format(GameData.Instance.textConfig.popupNewObjetive, data.objetives[progress]));
            }
        }

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
                    GameManager.Instance.SetPause(false);
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