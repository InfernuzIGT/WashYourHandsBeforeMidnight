using UnityEngine;

public class MenuController : MonoBehaviour
{

    [Header ("Cameras")]
    public GameObject mainCam;
    public GameObject continueCam;
    public GameObject optionsCam;
    public GameObject extrasCam;
    public GameObject creditsCam;
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

    private void Start ()
    {
        mainCam.SetActive (true);
        mainPanel.SetActive (true);

        // Last cam active set in private GO
    }

    public void OnBackButton ()
    {

        _lastCam.SetActive (true);
        _lastPanel.SetActive (true);

        if (_isInOptions)
        {
            optionsPanel.SetActive (false);

            _isInOptions = false;

        }

        if (_isInSpecificSettings)
        {

            OnOptionsButton ();

            _isInSpecificSettings = false;
            _isInOptions = false;
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

    public void OnContinueButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

        SetConfirmPanel ();
        _isContinuing = true;
    }

    public void OnExtrasButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);

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

        SetConfirmPanel ();
        _isCreatingNew = true;
    }

    public void OnQuitButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

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

        _isInExtras =false;
        _isInSpecificSettings = true;
    }

    public void OnGraphicsButton ()
    {
        _lastPanel = optionsPanel;
        
        _lastPanel.SetActive (false);

        Debug.Log ($"<b> Graphics settings is open </b>");

        // graphicsPanel.SetActive (true);

        _isInExtras =false;
        _isInSpecificSettings = true;

    }

    public void OnAudioButton ()
    {
        _lastPanel = optionsPanel;

        _lastPanel.SetActive (false);

        Debug.Log ($"<b> Audio settings is open </b>");

        // audioPanel.SetActive (true);

        _isInExtras =false;
        _isInSpecificSettings = true;

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
        creditsCam.SetActive (true);

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

        }
        if (_isCreatingNew)
        {
            Debug.Log ($"<b> New Game is created </b>");
            // Save new data in GAMEDATA

        }

        if (_isContinuing)
        {
            Debug.Log ($"<b> Loading Game </b>");
            // Load game data in GAMEDATA

        }

    }

    public void OnNoButton ()
    {
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