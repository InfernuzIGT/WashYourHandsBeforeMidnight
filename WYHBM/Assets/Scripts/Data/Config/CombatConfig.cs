using UnityEngine;

[CreateAssetMenu(fileName = "New CombatConfig", menuName = "Config/CombatConfig", order = 0)]
public class CombatConfig : ScriptableObject
{
    [Header("General")]
    public float fadeDuration = 3;
    public float fillDuration = .25f;
    public float transitionDuration = .35f;
    public float waitCombatDuration = 1.65f;
    public float scaleCombat = 17.5f;

    [Header("Action")]
    public ActionObject actionObjectPrefab;
}