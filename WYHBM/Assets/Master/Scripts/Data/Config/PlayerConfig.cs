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
    [Space]
    public float soundRadiusRun = 16f;
    public float soundRadiusJogging = 12f;
    public float soundRadiusWalk = 5f;
    public float soundRadiusCrouch = 0;

    [Header("Input System")]
    public float axisLimit = 0.7f;
    public float axisLimitCrouch = 0.5f;
    public float stickDeadZone = 0.05f;
    public Vector2 mouseOffset = new Vector2(0.5f, 0.5f);
    public float[] zoomValues;

}