using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "New FMODConfig", menuName = "Config/FMODConfig", order = 0)]
public class FMODConfig : ScriptableObject
{
    [Header("Movement")]
    [EventRef] public string climbing = null;
    
    [Header("Other")]
    [EventRef] public string stinger = null;

    [Header("Menu")]
    [EventRef] public string music = null;
    [EventRef] public string scroll = null;
    [EventRef] public string select = null;
    [EventRef] public string back = null;
    [EventRef] public string change = null;

}