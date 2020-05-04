using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [Header ("Cameras")]
    public GameObject mainCam;
    public GameObject continueCam;
    public GameObject optionsCam;
    public GameObject extrasCam;
    // public GameObject creditsCam;
    private GameObject _lastCam;

    [Header ("Generals")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject extrasPanel;
    public GameObject confirmPanel;

    [Header ("Options")]
    public GameObject graphicsPanel;
    public GameObject audioPanel;
    public GameObject gamePanel;

    [Header ("Graphics")]
    public GameObject DropDownPanel;

    // [Header ("Audio")]

    [Header ("Extras")]
    public GameObject creditsPanel;

    // GameObjects
    private GameObject _lastPanel;

    // Bool
    private bool _isQuitting;
    private bool _isCreatingNew;
    private bool _isContinuing;
    private bool _isInOptions;
    private bool _isInExtras;
    private bool _isInCredits;
    private bool _isInSpecificSettings;

    FMODUnity.StudioEventEmitter buttonSounds;
    FMODUnity.StudioEventEmitter menuMusic;

    private void Start ()
    {
        mainCam.SetActive (true);
        mainPanel.SetActive (true);

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

    public void OnBackButton ()
    {        
        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 0f);

        _lastCam.SetActive (true);
        _lastPanel.SetActive (true);

        if (_isInOptions)
        {
            optionsPanel.SetActive (false);

            _isInOptions = false;

        }

        if (_isInSpecificSettings)
        {
            audioPanel.SetActive(false);
            graphicsPanel.SetActive(false);

            OnOptionsButton ();

            _isInSpecificSettings = false;
            _isInOptions = true;
        }

        if (_isInExtras)
        {
            extrasPanel.SetActive (false);

            _isInExtras = false;

        }
        if (_isInCredits)
        {
            creditsPanel.SetActive (false);

            OnExtrasButton ();

            _isInCredits = false;
            _isInExtras = true;
        }

    }

    #region Main Menu

    public void OnContinueButton () //Borrar
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

        

        
        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 1f);

        SetConfirmPanel ();
        _isContinuing = true;
    }

    public void OnExtrasButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

        if (_lastPanel = mainPanel)
        {
            buttonSounds.Play();
            buttonSounds.EventInstance.setParameterByName("UI", 1f);
        }

        extrasCam.SetActive (true);
        extrasPanel.SetActive (true);

        _isInExtras = true;

    }

    public void OnOptionsButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        optionsCam.SetActive (true);
        optionsPanel.SetActive (true);

        if (_lastPanel = mainPanel)
        {
            buttonSounds.Play();
            buttonSounds.EventInstance.setParameterByName("UI", 1f);
        }

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

        _isInOptions = true;
    }

    public void OnNewGameButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

        SceneManager.LoadScene(1);
        
        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 1f);

        SetConfirmPanel ();
        _isCreatingNew = true;
    }

    public void OnQuitButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 4f);

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

        SetConfirmPanel ();
        _isQuitting = true;
        //Confirm text

    }
    
    #endregion 

    #region Options Menu

    public void OnGameButton ()
    {

        _lastPanel = optionsPanel;
        
        _lastPanel.SetActive (false);

        Debug.Log ($"<b> Game settings is open </b>");

        // gamePanel.SetActive (true);

        _isInOptions =false;
        _isInSpecificSettings = true;
    }

    public void OnGraphicsButton ()
    {
        _lastPanel = optionsPanel;
        
        _lastPanel.SetActive (false);

        Debug.Log ($"<b> Graphics settings is open </b>");
        
        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 1f);

        graphicsPanel.SetActive (true);

        _isInOptions =false;
        _isInSpecificSettings = true;

    }

    public void OnAudioButton ()
    {
        _lastPanel = optionsPanel;

        _lastPanel.SetActive (false);

        Debug.Log ($"<b> Audio settings is open </b>");
        
        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 1f);

        audioPanel.SetActive (true);

        _isInOptions =false;
        _isInSpecificSettings = true;

    }

    #endregion

    #region Graphics Menu
    
    public void OnDropDownButton(bool isOpening)
    {
        DropDownPanel.SetActive(isOpening);
    }

    #endregion

    #region Extras Menu

    public void OnCreditsButton ()
    {
        _lastCam = extrasCam;
        _lastPanel = extrasPanel;

        _lastPanel.SetActive (false);
        _lastCam.SetActive (false);

        creditsPanel.SetActive (true);
        // creditsCam.SetActive (true);

        _isInCredits = true;
        _isInExtras = false;

    }

    #endregion

    #region Confirm Menu

    public void SetConfirmPanel ()
    {
        mainPanel.SetActive (false);
        confirmPanel.SetActive (true);

    }

    public void OnYesButton ()
    {
        if (_isQuitting)
        {
            Application.Quit ();
            
            buttonSounds.Play();
            buttonSounds.EventInstance.setParameterByName("UI", 0f);

        }
        if (_isCreatingNew)
        {
            Debug.Log ($"<b> New Game is created </b>");
            
            buttonSounds.Play();
            buttonSounds.EventInstance.setParameterByName("UI", 3f);
            menuMusic.Stop();
            // Save new data in GAMEDATA

        }

        if (_isContinuing)
        {
            Debug.Log ($"<b> Loading Game </b>");
            
            buttonSounds.Play();
            buttonSounds.EventInstance.setParameterByName("UI", 3f);
            menuMusic.Stop();
            // Load demo scene
        }

    }

    public void OnNoButton ()
    {        
        buttonSounds.Play();
        buttonSounds.EventInstance.setParameterByName("UI", 2f);

        if (_isQuitting)
        {
            mainCam.SetActive (true);
            mainPanel.SetActive (true);

            confirmPanel.SetActive (false);

            _isQuitting = false;

        }
        if (_isCreatingNew)
        {
            mainCam.SetActive (true);
            mainPanel.SetActive (true);

            confirmPanel.SetActive (false);

            _isCreatingNew = false;
        }

        if (_isContinuing)
        {
            mainCam.SetActive (true);
            mainPanel.SetActive (true);

            confirmPanel.SetActive (false);

            _isContinuing = false;
        }

    }
    #endregion
}