﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Ambients")]
    public AMBIENT currentAmbient;
    public GameObject currentInterior;
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

    private AMBIENT _lastAmbient;
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

    public void ChangeAmbient(AMBIENT newAmbient)
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
            case AMBIENT.World:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case AMBIENT.Interior:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case AMBIENT.Location:
                worldUI.EnableCanvas(true);
                combatUI.EnableCanvas(false);

                player.ChangeMovement(true);
                break;

            case AMBIENT.Combat:
                worldUI.EnableCanvas(false);
                combatUI.EnableCanvas(true);

                combatUI.selectableButton.Select(); // TODO Mariano: REMOVE

                // TODO Mariano: Save Player Position
                // TODO Mariano: Move Player to Combat Zone
                // TODO Mariano: Spawn Enemies
                // TODO Mariano: Wait X seconds, and StartCombat!
                break;

            case AMBIENT.Development:
                // Nothing
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

    public void CreateInterior(bool isCreating, GameObject newInterior)
    {
        if (isCreating)
        {
            currentInterior = Instantiate(newInterior, GameData.Instance.gameConfig.interiorPosition, Quaternion.identity);
        }
        else
        {
            Destroy(currentInterior);
        }
    }

    private void FadeOff()
    {
        fadeImg.enabled = false;
    }

}