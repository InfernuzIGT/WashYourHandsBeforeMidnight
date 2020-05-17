using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static bool isGamePaused = false;

    [Header ("GameObjects")]
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

    private void Start ()
    {
        Resume();
    }

    private void Update () //Move to PlayerController
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                Pause ();

            }
            else
            {
                Resume ();
            }

        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive (false);
        Time.timeScale = 1f;
        isGamePaused = false;

    }

    public void Pause ()
    {
        pauseMenuUI.SetActive (true);
        Time.timeScale = 0f;
        isGamePaused = true;

    }
    #region Diary

    public void OnDiaryButton ()
    {

        systemUI.SetActive (false);
        optionsUI.SetActive (false);
        inventoryUI.SetActive (false);

        pauseMenuSprite.SetActive(true);
        diaryUI.SetActive (true);

        _isInDiary = true;

    }

    #endregion

    #region Inventory
    public void OnInventoryButton ()
    {
        pauseMenuSprite.SetActive(false);
        systemUI.SetActive (false);
        optionsUI.SetActive (false);
        diaryUI.SetActive(false);

        inventoryUI.SetActive (true);

        _isInInventory = true;

    }

    #endregion

    #region System
    public void OnSystemButton ()
    {
        diaryUI.SetActive(false);
        optionsUI.SetActive(false);
        inventoryUI.SetActive(false);

        pauseMenuSprite.SetActive(true);
        systemUI.SetActive (true);

        _isInSystem = true;
    }

    public void OnOptionsButton ()
    {
        optionsUI.SetActive (true);

        _isInSystem = true;
    }

    public void OnQuitButton ()
    {
        Application.Quit ();

    }

    #endregion
}