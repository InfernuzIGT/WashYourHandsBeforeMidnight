using UnityEngine;

[CreateAssetMenu(fileName = "New GameConfig", menuName = "Config/GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [Header ("General")]
    public string tagPlayer = "Player";
    
    [Header("Fade")]
    public float fadeDuration = 3;

}