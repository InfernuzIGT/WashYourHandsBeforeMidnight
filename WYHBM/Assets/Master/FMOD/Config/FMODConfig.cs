using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "New FMODConfig", menuName = "Config/FMODConfig", order = 0)]
public class FMODConfig : ScriptableObject
{
    [Header("Globar Parameters")]
    [ParamRef] public string actionSound;
    [ParamRef] public string stateSound;
    [Space]
    [ParamRef] public string masterSlider;
    [ParamRef] public string musicSlider;
    [ParamRef] public string soundSlider;

    [Header("Instance")]
    [EventRef] public string music = null;
    [EventRef] public string select = null;
    [EventRef] public string back = null;

    [Header("Movement")]
    [EventRef] public string climbing = null;

    [Header("Menu")]
    [EventRef] public string scroll = null;
    [EventRef] public string change = null;

    [Header("Popup")]
    [EventRef] public string popupQuest = null;
    [EventRef] public string popupDevice = null;
    [EventRef] public string popupTutorial = null;

    [Header("Other")]
    [EventRef] public string snapshotPause = null;
}