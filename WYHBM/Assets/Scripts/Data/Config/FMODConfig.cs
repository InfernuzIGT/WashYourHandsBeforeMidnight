using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "New FMODConfig", menuName = "Config/FMODConfig", order = 0)]
public class FMODConfig : ScriptableObject
{
    [Header("Movement")]
    [EventRef]
    public string footsteps = null;

    [Header("Attacks")]
    [EventRef]
    public string[] swordAttacks = null;

}