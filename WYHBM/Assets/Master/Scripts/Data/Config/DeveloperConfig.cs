using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DeveloperConfig", menuName = "Config/DeveloperConfig")]
public class DeveloperConfig : ScriptableObject
{
    [Header("Developer")]
    public bool autoInit;
    public bool isBuild;

    [Header("Game")]
    public bool silentSteps;
    public bool dDLegacyMode;
    public bool dontSave;

    [Header("Debug")]
    public bool printInputInfo;

    public void SetBuildState()
    {
        autoInit = false;
        isBuild = true;

        silentSteps = false;
        dDLegacyMode = false;
        dontSave = false;

        printInputInfo = false;
    }
}