using UnityEngine;

[CreateAssetMenu(fileName = "New CombatConfig", menuName = "Config/CombatConfig", order = 0)]
public class CombatConfig : ScriptableObject
{
    [Header("General")]
    public float fadeDuration = 3;
    public float fillDuration = .25f;

    [Header("Action")]
    public ActionObject actionObjectPrefab;
}