using Events;
using FMODUnity;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject[] cams;

    [Header("Generals")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    [Space]
    public GameObject ContinueButton;
    public GameObject NewGameButton;
    [Space]
    public GameObject Popup;

    [Header("Options")]
    public GameObject audioPanel;

    [Header("FMOD")]
    public StudioEventEmitter _buttonSounds;
    public StudioEventEmitter _menuMusic;

    private GameObject _lastPanel;
    private GameObject _lastCam;

    private bool _inAudio;
    private bool _isDataLoaded;

    private void Start()
    {
        if (_isDataLoaded)
        {
            NewGameButton.SetActive(false);
            ContinueButton.SetActive(true);
        }

        mainPanel.SetActive(true);

        _menuMusic.Play();
    }

    private void OnEnable()
    {
        EventController.AddListener<MainMenuEvent>(MainMenu);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<MainMenuEvent>(MainMenu);
    }

    private void PlayButtonSound(float parameter)
    {
        _buttonSounds.Play();
        _buttonSounds.EventInstance.setParameterByName(FMODParameters.UI, parameter);
    }

    public void MainMenu(MainMenuEvent evt)
    {
        switch (evt.menuType)
        {
            case MENU_TYPE.Continue:
                // GameData.Instance.LoadScene(SCENE_INDEX.Master);
                // GameManager.Instance.LoadGame();
                Debug.Log($"<b> Master </b>");

                PlayButtonSound(3);
                break;

            case MENU_TYPE.NewGame:
                // GameData.Instance.LoadScene(SCENE_INDEX.Master);
                // GameManager.Instance.LoadGame();
                Debug.Log($"<b> Master </b>");
                _isDataLoaded = true;

                PlayButtonSound(3);
                break;

            case MENU_TYPE.Options:
                mainPanel.SetActive(false);
                _lastPanel = mainPanel;

                optionsPanel.SetActive(true);

                cams[1].SetActive(true);
                _lastCam = cams[0];

                PlayButtonSound(1);
                break;

            case MENU_TYPE.Audio:
                optionsPanel.SetActive(false);
                _lastPanel = optionsPanel;

                _inAudio = true;

                audioPanel.SetActive(true);

                _lastCam = cams[0];

                PlayButtonSound(1);
                break;

            case MENU_TYPE.Credits:
                mainPanel.SetActive(false);
                _lastPanel = mainPanel;

                creditsPanel.SetActive(true);

                cams[2].SetActive(true);
                _lastCam = cams[0];

                _menuMusic.EventInstance.setParameterByName(FMODParameters.Credits, 1f);
                PlayButtonSound(1);
                break;

            case MENU_TYPE.Back:
                DesactivateAll();

                if (_inAudio)
                {
                    optionsPanel.SetActive(true);
                    _lastPanel = mainPanel;

                    _inAudio = false;
                    return;
                }

                for (int i = 0; i < cams.Length; i++)
                {
                    cams[i].SetActive(false);
                }

                _lastCam.SetActive(true);
                _lastPanel.SetActive(true);

                PlayButtonSound(0);
                break;

            case MENU_TYPE.Exit:
                DesactivateAll();

                ShowPopup(true);
                PlayButtonSound(3);
                break;

            case MENU_TYPE.YesPopup:
                Application.Quit();
                PlayButtonSound(3);
                break;
        }

    }
    public void ShowPopup(bool _isOpen)
    {
        Popup.SetActive(_isOpen);
    }

    public void DesactivateAll()
    {
        ShowPopup(false);
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        audioPanel.SetActive(false);

        PlayButtonSound(4);
        _menuMusic.EventInstance.setParameterByName(FMODParameters.Credits, 0f);
    }

    //     public void OnYesButton()
    //     {
    //         if (_isQuitting)
    //         {
    //             Application.Quit();

    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 0f);

    //         }
    //         if (_isCreatingNew)
    //         {
    //             Debug.Log($"<b> New Game is created </b>");

    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 3f);
    //             menuMusic.Stop();
    //             // Save new data in GAMEDATA

    //             LoadScene(SCENE_INDEX.Master);
    //         }

    //         if (_isContinuing)
    //         {
    //             Debug.Log($"<b> Loading Game </b>");

    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 3f);
    //             menuMusic.Stop();
    //             // Load demo scene
    //         }
    //     }

    //     public void OnNoButton()
    //     {
    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 2f);
    //     }
}