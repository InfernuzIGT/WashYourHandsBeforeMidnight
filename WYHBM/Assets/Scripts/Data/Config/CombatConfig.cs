﻿using UnityEngine;

[CreateAssetMenu(fileName = "New CombatConfig", menuName = "Config/CombatConfig", order = 0)]
public class CombatConfig : ScriptableObject
{
    [Header("General")]
    public Vector3 positionAction = new Vector3(-.5f, 0, 0);
    public float animationDuration = .35f;
    [Space]
    public float startFillDuration = 5;
    public float fillDuration = .25f;
    public float offsetHealthBar = 2;

    [Header("Fade")]
    public float fadeDuration = 3;
    public float canvasFadeDuration = .35f;
    public float fadeOutEndDuration = .75f;

    [Header("Layers")]
    public LayerMask layerNone;
    public LayerMask layerPlayer;
    public LayerMask layerEnemy;

    [Header("References")]
    public CharacterUI characterUIPrefab;
    public Actions actionsPrefab;
    public ActionButton actionButtonPrefab;

    [Header("Action")]
    public float actionTimeThresholdMultiplier = 1f;
    public float waitTimeToStart = 3;
    public float waitTimePerAction = 0.5f;
    public float waitTimeBetweenTurns = 1;

    // [Header("Info Text")]
    // public float infoTextMoveDuration = 1;
    // public float infoTextFadeDuration = .35f;
    // public float infoTextFadeDelay = .65f;
    // public float positionYTextStart = 1;
    // public float positionYTextEnd = 2.5f;

    [Header("Shake")]
    public float shakeDuration = .25f;
    public float shakeAmplitude = .75f;
    public float shakeFrequency = 25;
}