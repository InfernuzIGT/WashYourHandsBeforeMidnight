using UnityEngine;

[CreateAssetMenu(fileName = "New CombatConfig", menuName = "Config/CombatConfig", order = 0)]
public class CombatConfig : ScriptableObject
{
    [Header("General")]
    public float fadeDuration = 3;
    public float fillDuration = .25f;
    public float canvasFadeDuration = .35f;
    public float fadeOutEndDuration = .75f;

    [Header("Action")]
    public ActionObject actionObjectPrefab;

    [Header("Animation - Combat")]
    public float transitionDuration = .35f;
    public float waitCombatDuration = 1.65f;
    public float evaluationDuration = 1.25f;
    public float scaleCombat = 17.5f;
    [Space]
    public Vector3 positionCombat = new Vector3(-3.5f, 0, 0);
    public float positionXCharacter = -4;
    
    [Header ("Info Text")]
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