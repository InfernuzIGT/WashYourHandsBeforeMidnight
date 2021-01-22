using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerConfig", menuName = "Config/PlayerConfig", order = 0)]
public class PlayerConfig : ScriptableObject
{
    [Header("General")]
    public float height = 3.15f;

    [Header("Movement")]
    public float speedRun = 16f;
    public float speedJogging = 12f;
    public float speedWalk = 5f;
    public float speedCrouchFast = 3.5f;
    public float speedCrouch = 2f;
    [Space]
    public float gravity = 39.24f;
    public float magnitudeFall = 20f;

    [Header("Input System")]
    public float axisLimit = 0.7f;
    public float axisLimitCrouch = 0.5f;
    public float stickDeadZone = 0.05f;
    public float[] zoomValues;

    [Header("Layers")]
    public LayerMask layerGround;
    public LayerMask layerClimbable;

}