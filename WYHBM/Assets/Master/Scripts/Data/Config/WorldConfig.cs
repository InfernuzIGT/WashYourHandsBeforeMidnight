using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New WorldConfig", menuName = "Config/WorldConfig", order = 0)]
public class WorldConfig : ScriptableObject
{
    [Header("General")]
    public Vector3 playerBaseOffset = new Vector3(0, 2.08f, 0);
    [Space]
    public float messageLifetime = 3f;
    public float timeStart = 0.5f;
    public float timeSpace = 1f;
    [Space]
    public float fovTime = 1f;

    [Header("Fade")]
    public float fadeCutsceneDuration = 1;
    public float fadeFastDuration = 1.5f;
    public float fadeSlowDuration = 3;

    [Header("Layers")]
    public LayerMask layerFOVTarget;
    public LayerMask layerFOVObstacle;
    [Space]
    public LayerMask layerOcclusionMask;

    [Header("References")]
    public Slot slotPrefab;
    public InteractionItem itemPrefab;
    public QuestTitle questTitlePrefab;
    public QuestDescription questDescriptionPrefab;
    public TextMeshProUGUI questObjetivePrefab;

    [Header("Placeholder")]
    public GameObject emptyObject;
    public Sprite spriteEmptyUI;

    [Header("Dialogs")]
    public Sprite samIcon;
}