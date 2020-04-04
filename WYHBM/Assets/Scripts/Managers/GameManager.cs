using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Ambient
{
    World = 0,
    Interior = 1,
    Location = 2,
    Combat = 3
}

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Ambients")]
    public Ambient currentAmbient;
    [Space]
    public CombatManager combatManager;
    public GameMode.World.UIManager worldUI;
    public GameMode.Combat.UIManager combatUI;

    [Header("Cameras")]
    public GameObject[] cameras;

    [Header("Characters")]
    public PlayerController player;

    [Header("Other")]
    public Image fadeImg;

    private Ambient _lastAmbient;
    private Camera _cameraMain;

    private void Start()
    {
        _cameraMain = Camera.main;

        StartGame();
    }

    private void StartGame()
    {
        SwitchAmbient();
        // SwitchCamera();
    }

    public void ChangeAmbient(Ambient newAmbient)
    {
        _lastAmbient = currentAmbient;
        currentAmbient = newAmbient;

        fadeImg.enabled = true;
        fadeImg.DOFade(1, GameData.Instance.gameConfig.fadeDuration).OnKill(SetAmbient);

        player.ChangeMovement(false);
    }

    private void SetAmbient()
    {
        SwitchAmbient();
        // SwitchCamera();

        fadeImg.DOFade(0, GameData.Instance.gameConfig.fadeDuration)
            .OnKill(FadeOff);
    }

    private void SwitchAmbient()
    {
        switch (currentAmbient)
        {
            case Ambient.World:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case Ambient.Interior:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case Ambient.Location:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case Ambient.Combat:
                worldUI.EnableCanvas(false);
                combatUI.EnableCanvas(true);

                combatUI.selectableButton.Select(); // TODO Mariano: REMOVE

                // TODO Mariano: Save Player Position
                // TODO Mariano: Move Player to Combat Zone
                // TODO Mariano: Spawn Enemies
                // TODO Mariano: Wait X seconds, and StartCombat!
                break;

            default:
                break;
        }
    }

    private void SwitchCamera()
    {
        cameras[(int)_lastAmbient].SetActive(false);
        cameras[(int)currentAmbient].SetActive(true);
    }

    private void FadeOff()
    {
        fadeImg.enabled = false;
    }

}