using UnityEngine;

[CreateAssetMenu(fileName = "New CombatConfig", menuName = "Config/CombatConfig", order = 0)]
public class CombatConfig : ScriptableObject
{
    [Header("General")]
    public float fadeDuration = 3;
    public float startFillDuration = 5;
    public float fillDuration = .25f;
    public float canvasFadeDuration = .35f;
    public float fadeOutEndDuration = .75f;
    public float offsetHealthBar = 2;

    [Header("Layer")]
    public LayerMask layerNone;
    public LayerMask layerPlayer;
    public LayerMask layerEnemy;

    [Header("Action")]
    public float actionTimeThresholdMultiplier = 1f;
    public float waitTimeToStart = 3;
    public float waitTimePerAction = 1.5f;
    public float waitTimeBetweenTurns = 1;

    [Header("Animation - Combat")]
    public float animationDuration = .35f;
    public Vector3 positionAction = new Vector3(-2, 0, 0);
    // public float positionActionX = -2;
    // public float waitCombatDuration = 1.65f; 
    // public float evaluationDuration = 1.25f; 
    // public float scaleCombat = 17.5f;
    // public float positionXCharacter = -4;

    [Header("Info Text")]
    public float infoTextMoveDuration = 1;
    public float infoTextFadeDuration = .35f;
    public float infoTextFadeDelay = .65f;
    public float positionYTextStart = 1;
    public float positionYTextEnd = 2.5f;

    [Header("Shake")]
    public float shakeDuration = .5f;
    public float shakeAmplitude = .25f;
    public float shakeFrequency = 25;

}