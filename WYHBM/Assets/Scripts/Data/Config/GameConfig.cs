using UnityEngine;

[CreateAssetMenu(fileName = "New GameConfig", menuName = "Config/GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [Header("General")]
    public string tagPlayer = "Player";
    public Vector3 interiorPosition = new Vector3(500, 500, 500);

    [Header("Fade")]
    public float fadeFastDuration = 1.5f;
    public float fadeSlowDuration = 3;

}