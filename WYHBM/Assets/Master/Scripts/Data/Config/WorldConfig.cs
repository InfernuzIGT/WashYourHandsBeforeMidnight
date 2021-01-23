using UnityEngine;

[CreateAssetMenu(fileName = "New WorldConfig", menuName = "Config/WorldConfig", order = 0)]
public class WorldConfig : ScriptableObject
{
    [Header("General")]
    public float fovTime = 1f;

    [Header("Fade")]
    public float fadeCutsceneDuration = 1;
    public float fadeFastDuration = 1.5f;
    public float fadeSlowDuration = 3;

    [Header("Layers")]
    public LayerMask layerFOVTarget;
    public LayerMask layerFOVObstacle;
    [Space]
    public LayerMask layerGround;
    public LayerMask layerClimbable;
    public LayerMask layerNPC;
    [Space]
    public LayerMask layerOcclusionMask;
}