using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RumbleValues
{
    public RUMBLE_TYPE type;
    [Range(0f, 5f)] public float duration;
    [Range(0f, 1f)] public float frequency;
}

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
    public float speedLadder = 5f;
    [Space]
    public float gravity = 39.24f;
    public float magnitudeFall = 30f;
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
    [SerializeField, ArrayElementTitle("type")] private RumbleValues[] rumbleValues;

    private Dictionary<RUMBLE_TYPE, RumbleValues> rumbleDictionary;
    
    public RumbleValues GetRumbleValues(RUMBLE_TYPE type)
    {
        if (rumbleDictionary.ContainsKey(type))
        {
            return rumbleDictionary[type];
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't find Rumble {type}");
            return null;
        }
    }

    [ContextMenu("Update Dictionary")]
    public void UpdateActionDictionary()
    {
        rumbleDictionary = new Dictionary<RUMBLE_TYPE, RumbleValues>();

        for (int i = 0; i < rumbleValues.Length; i++)
        {
            if (!rumbleDictionary.ContainsKey(rumbleValues[i].type))
            {
                rumbleDictionary.Add(rumbleValues[i].type, rumbleValues[i]);
            }
        }
    }

}