using UnityEngine;

[CreateAssetMenu(fileName = "New GameConfig", menuName = "Config/GameConfig", order = 0)]
public class GameConfig : ScriptableObject
{
    [Header("General")]
    public Vector3 interiorPosition = new Vector3(500, 500, 500);

    [Header("Fade")]
    public float fadeFastDuration = 1.5f;
    public float fadeSlowDuration = 3;

    [Header("UI")]
    public float messageLifetime = 3f;
    public float timeStart = 0.5f;
    public float timeSpace = 1f;

    [Header("Layer")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("References")]
    public GameObject emptyObject;
    public Slot slotPrefab;
    public SlotEquipped slotEquippedPrefab;
    public InteractionItem itemPrefab;

    [Header("Transform")]
    public Vector3 playerBaseOffset = new Vector3(0, 2.08f, 0);
}