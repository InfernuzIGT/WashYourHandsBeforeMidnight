using UnityEngine;

[CreateAssetMenu(fileName = "New GameConfig", menuName = "Config/GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [Header("General")]
    public Vector3 interiorPosition = new Vector3(500, 500, 500);

    [Header("Fade")]
    public float fadeFastDuration = 1.5f;
    public float fadeSlowDuration = 3;

    [Header("Tags")]
    [Tag] public string tagPlayer;
    
    [Header ("UI")]
    public float messageLifetime = 3f;
    public float timeStart = 0.5f;
    public float timeSpace = 1f;

    [Header ("References")]
    public Slot slotPrefab;
    public InteractionItem itemPrefab;

    [Header ("Transform")]
    public float playerBaseOffset = 1.51f;
}