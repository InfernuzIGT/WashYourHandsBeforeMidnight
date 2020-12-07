using System.Collections.Generic;
using DG.Tweening;
using Events;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.World
{
    [RequireComponent(typeof(CanvasGroupUtility))]
    public class UIManager : MonoBehaviour
    {
        [Header("General")]
        public Popup popup;
        public Image progressImg;
        [Space]
        public GameObject panelPlayer;
        public GameObject panelPause;
        public GameObject panelNote;

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

        private CanvasGroupUtility _canvasUtility;

        private void Awake()
        {
            _canvasUtility = GetComponent<CanvasGroupUtility>();
        }

        private void Start()
        {
            dicQuestTitle = new Dictionary<QuestSO, QuestTitle>();
            dicQuestDescription = new Dictionary<QuestSO, QuestDescription>();

            _enableMovementEvent = new EnableMovementEvent();
            // continueTxt.enabled = false;

            // _currentQuestSelected = new GameObject();

            // panelDialog.SetActive(false);

            // buttonLeft.onClick.AddListener(() => GameManager.Instance.NextCharacter(true));
            // buttonRight.onClick.AddListener(() => GameManager.Instance.NextCharacter(false));

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

        // public void EnableCanvas(bool isEnabled)
        // {
        //     _canvas.enabled = isEnabled;
        // }

        public void Show(bool isShowing)
        {
            _canvasUtility.Show(isShowing);
        }

        public void Pause(bool isPaused)
        {
            panelPause.gameObject.SetActive(isPaused);
        }

        // public void ChangeCharacter(CombatPlayer combatPlayer, int index, bool inLeftLimit = false, bool inRightLimit = false)
        // {
        //     characterNameTxt.text = combatPlayer.character.Name;
        //     characterImg.sprite = combatPlayer.character.PreviewSprite;

        //     characterSlot[_lastSlot].gameObject.SetActive(false);
        //     characterSlot[index].gameObject.SetActive(true);

        //     _lastSlot = index;

        //     buttonLeft.interactable = !inLeftLimit;
        //     buttonRight.interactable = !inRightLimit;
        // }

        public void VolumeSound(float vol)
        {
            RuntimeManager.StudioSystem.setParameterByName(FMODParameters.SoundsSlider, vol);
        }

        public void VolumeMusic(float vol)
        {
            RuntimeManager.StudioSystem.setParameterByName(FMODParameters.MusicSlider, vol);
        }

        public Transform GetSlot()
        {
            return characterSlot[_lastSlot];
        }

        #region 

        public void ActiveNote(bool active)
        {
            panelNote.SetActive(active);
        }

        public void SetNoteText(string sentences)
        {
            noteTxt.text = sentences;
        }

        #endregion

        #region Quest

        public void SetQuest(QuestSO data)
        {
            if (data == null)
            {
                return;
            }

            // ShowPopup(string.Format(GameData.Instance.textConfig.popupNewQuest, data.title));

            // Create Quest Title
            QuestTitle questTitle = Instantiate(GameData.Instance.worldConfig.questTitlePrefab, diaryTitleContainer);
            questTitle.Init(data);
            dicQuestTitle.Add(data, questTitle);

            // Create Quest Description
            QuestDescription questDescription = Instantiate(GameData.Instance.worldConfig.questDescriptionPrefab, diaryDescriptionContainer);
            questDescription.Init(data);
            dicQuestDescription.Add(data, questDescription);
            SelectQuest(data);

            // GameManager.Instance.CurrentQuestData.state = QUEST_STATE.InProgress;
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

            // if (progress == data.objetives.Length)
            // {
            //     ShowPopup(string.Format(GameData.Instance.textConfig.popupCompleted, data.title));
            //     dicQuestTitle[data].Complete();

            //     GameManager.Instance.CurrentQuestData.state = QUEST_STATE.Completed;
            // }
            // else
            // {
            //     ShowPopup(string.Format(GameData.Instance.textConfig.popupNewObjetive, data.objetives[progress]));
            // }
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
                    GameManager.Instance.Pause(PAUSE_TYPE.PauseMenu);
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