using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [Header ("Cameras")]
    public GameObject mainCam;
    public GameObject continueCam;
    public GameObject optionsCam;
    public GameObject extrasCam;
    private GameObject _lastCam;

    [Header ("Generals")]

    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject extrasPanel;
    public GameObject confirmPanel;
    public GameObject graphicsPanel;
    public GameObject audioPanel;
    private GameObject _lastPanel;
    
    private bool _isQuitting;
    private bool _isCreatingNew;

    private void Start ()
    {
        mainCam.SetActive (true);
        mainPanel.SetActive (true);

        // Last cam active set in private GO

    }

    public void OnBackButton()
    {

        _lastCam.SetActive(true);
        _lastPanel.SetActive(true);
        
    }

    #region Main Menu

    public void OnContinueButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastPanel.SetActive (false);
        _lastCam.SetActive (false);

        confirmPanel.SetActive (true);
        continueCam.SetActive (true);

    }

    public void OnExtrasButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        _lastCam.SetActive (false);
        _lastPanel.SetActive(false);

        extrasCam.SetActive(true);
        extrasPanel.SetActive(true);

    }

    public void OnOptionsButton ()
    {
        _lastCam = mainCam;
        _lastPanel = mainPanel;

        optionsCam.SetActive (true);
        optionsPanel.SetActive(true);

        _lastCam.SetActive (false);
        _lastPanel.SetActive (false);
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

    public void OnGraphicsButton()
    {
        _lastPanel = optionsPanel;

        graphicsPanel.SetActive(true);
        
    }

    public void OnAudioButton()
    {
        _lastPanel = optionsPanel;

        audioPanel.SetActive(true);
        
    }

    #endregion

    #region Extras Menu

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
            // Save new data in GAMEDATA

        }

    }

    public void OnNoButton ()
    {
        if (_isQuitting)
        {
            mainCam.SetActive (true);
            mainPanel.SetActive(true);

            confirmPanel.SetActive(false);

            _isQuitting = false;

        }
        if (_isCreatingNew)
        {
            mainCam.SetActive (true);
            mainPanel.SetActive(true);

            confirmPanel.SetActive(false);

            _isCreatingNew = false;
        }

    }
    #endregion

}