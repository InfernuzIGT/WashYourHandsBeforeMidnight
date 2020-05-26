using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject[] cams;

    // private List<int> cameras;

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

    // [Header("Graphics")]
    // public GameObject DropDownPanel;

    // [Header ("Audio")]

    // GameObjects
    private GameObject _lastPanel;
    private GameObject _lastCam;

    // Bool
    private bool _inAudio;
    private bool _isSelectedYes;
    private bool _isInCredits;
    private bool _isDataLoad;

    private int _index;

    FMODUnity.StudioEventEmitter buttonSounds;
    FMODUnity.StudioEventEmitter menuMusic;

    private void Start()
    {
        if (_isDataLoad)
        {
            NewGameButton.SetActive(false);

            ContinueButton.SetActive(true);

        }

        mainPanel.SetActive(true);

        buttonSounds = GetComponent<FMODUnity.StudioEventEmitter>();
        menuMusic = GameObject.Find("Main Menu Music").GetComponent<FMODUnity.StudioEventEmitter>();
        menuMusic.Play();

        // Last cam active set in private GO
    }

    private void Update()
    {
        if (_isInCredits == true)
        {
            menuMusic.EventInstance.setParameterByName("Credits", 1f);
        }
        else
        {
            menuMusic.EventInstance.setParameterByName("Credits", 0f);
        }
    }

    public void MainMenu(MENU_TYPE menuType)
    {

        switch (menuType)
        {
            case MENU_TYPE.Continue:
                // GameData.Instance.LoadScene(SCENE_INDEX.Master);
                // GameManager.Instance.LoadGame();
                Debug.Log($"<b> Master </b>");

                break;

            case MENU_TYPE.NewGame:
                // GameData.Instance.LoadScene(SCENE_INDEX.Master);
                // GameManager.Instance.LoadGame();
                Debug.Log($"<b> Master </b>");

                _isDataLoad = true;

                break;

            case MENU_TYPE.Options:
                mainPanel.SetActive(false);
                _lastPanel = mainPanel;

                optionsPanel.SetActive(true);

                cams[1].SetActive(true);
                _lastCam = cams[0];

                break;

            case MENU_TYPE.Audio:
                optionsPanel.SetActive(false);
                _lastPanel = optionsPanel;

                _inAudio = true;

                audioPanel.SetActive(true);

                _lastCam = cams[0];

                break;

            case MENU_TYPE.Credits:
                mainPanel.SetActive(false);
                _lastPanel = mainPanel;

                creditsPanel.SetActive(true);

                cams[2].SetActive(true);
                _lastCam = cams[0];

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

                break;

            case MENU_TYPE.Exit:
                DesactivateAll();

                ShowPopup(true);

                break;

            case MENU_TYPE.YesPopup:

                Application.Quit();

                break;
            default:
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

    }

    //     public void OnBackButton()
    //     {
    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 0f);

    //         _lastCam.SetActive(true);
    //         _lastPanel.SetActive(true);

    //         if (_isInOptions)
    //         {
    //             optionsPanel.SetActive(false);

    //             _isInOptions = false;

    //         }

    //         if (_isInSpecificSettings)
    //         {
    //             audioPanel.SetActive(false);
    //             graphicsPanel.SetActive(false);

    //             OnOptionsButton();

    //             _isInSpecificSettings = false;
    //             _isInOptions = true;
    //         }

    //         if (_isInExtras)
    //         {
    //             extrasPanel.SetActive(false);

    //             _isInExtras = false;

    //         }
    //         if (_isInCredits)
    //         {
    //             creditsPanel.SetActive(false);

    //             OnExtrasButton();

    //             _isInCredits = false;
    //             _isInExtras = true;
    //         }

    //     }

    //     #region Main Menu

    //     public void OnContinueButton()
    //     {

    //         _lastPanel = mainPanel;

    //         _lastCam.SetActive(false);
    //         _lastPanel.SetActive(false);

    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 1f);

    //         SetConfirmPanel();
    //         _isContinuing = true;
    //     }

    //     public void OnExtrasButton()
    //     {

    //         _lastPanel = mainPanel;

    //         _lastCam.SetActive(false);
    //         _lastPanel.SetActive(false);

    //         if (_lastPanel = mainPanel)
    //         {
    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 1f);
    //         }

    //         extrasPanel.SetActive(true);

    //         _isInExtras = true;

    //     }

    //     public void OnOptionsButton()
    //     {

    //         _lastPanel = mainPanel;

    //         optionsPanel.SetActive(true);

    //         if (_lastPanel = mainPanel)
    //         {
    //             buttonSounds.Play();
    //             buttonSounds.EventInstance.setParameterByName("UI", 1f);
    //         }

    //         _lastCam.SetActive(false);
    //         _lastPanel.SetActive(false);

    //         _isInOptions = true;
    //     }

    //     public void OnNewGameButton()
    //     {

    //         _lastPanel = mainPanel;

    //         _lastCam.SetActive(false);
    //         _lastPanel.SetActive(false);

    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 1f);

    //         SetConfirmPanel();
    //         _isCreatingNew = true;
    //     }

    //     public void OnQuitButton()
    //     {

    //         _lastPanel = mainPanel;

    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 4f);

    //         _lastCam.SetActive(false);
    //         _lastPanel.SetActive(false);

    //         SetConfirmPanel();
    //         _isQuitting = true;
    //         //Confirm text

    //     }

    //     #endregion 

    //     #region Options Menu

    //     public void OnGameButton()
    //     {

    //         _lastPanel = optionsPanel;

    //         _lastPanel.SetActive(false);

    //         Debug.Log($"<b> Game settings is open </b>");

    //         // gamePanel.SetActive (true);

    //         _isInOptions = false;
    //         _isInSpecificSettings = true;
    //     }

    //     public void OnGraphicsButton()
    //     {
    //         _lastPanel = optionsPanel;

    //         _lastPanel.SetActive(false);

    //         Debug.Log($"<b> Graphics settings is open </b>");

    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 1f);

    //         graphicsPanel.SetActive(true);

    //         _isInOptions = false;
    //         _isInSpecificSettings = true;

    //     }

    //     public void OnAudioButton()
    //     {
    //         _lastPanel = optionsPanel;

    //         _lastPanel.SetActive(false);

    //         Debug.Log($"<b> Audio settings is open </b>");

    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 1f);

    //         audioPanel.SetActive(true);

    //         _isInOptions = false;
    //         _isInSpecificSettings = true;

    //     }

    //     #endregion

    //     #region Graphics Menu

    //     public void OnDropDownButton(bool isOpening)
    //     {
    //         DropDownPanel.SetActive(isOpening);
    //     }

    //     #endregion

    //     #region Extras Menu

    //     public void OnCreditsButton()
    //     {

    //         _lastPanel = extrasPanel;

    //         _lastPanel.SetActive(false);
    //         _lastCam.SetActive(false);

    //         creditsPanel.SetActive(true);
    //         // creditsCam.SetActive (true);

    //         _isInCredits = true;
    //         _isInExtras = false;

    //     }

    //     #endregion

    //     #region Confirm Menu

    //     public void SetConfirmPanel()
    //     {
    //         mainPanel.SetActive(false);
    //         confirmPanel.SetActive(true);

    //     }

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

    //     public void LoadScene(SCENE_INDEX sceneIndex)
    //     {
    //         StartCoroutine(LoadYourAsyncScene(sceneIndex));
    //     }

    //     private IEnumerator LoadYourAsyncScene(SCENE_INDEX index)
    //     {
    //         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync((int) index);

    //         while (!asyncLoad.isDone)
    //         {
    //             yield return null;
    //         }
    //     }

    //     public void OnNoButton()
    //     {
    //         buttonSounds.Play();
    //         buttonSounds.EventInstance.setParameterByName("UI", 2f);

    //         if (_isQuitting)
    //         {
    //             mainPanel.SetActive(true);

    //             confirmPanel.SetActive(false);

    //             _isQuitting = false;

    //         }
    //         if (_isCreatingNew)
    //         {
    //             mainPanel.SetActive(true);

    //             confirmPanel.SetActive(false);

    //             _isCreatingNew = false;
    //         }

    //         if (_isContinuing)
    //         {
    //             mainPanel.SetActive(true);

    //             confirmPanel.SetActive(false);

    //             _isContinuing = false;
    //         }

    //     }
    //     #endregion
}