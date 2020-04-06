using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoSingleton<DebugManager>
{
    public PlayerController player;

    [Header("UI")]
    public TextMeshProUGUI movementTxt;
    public TextMeshProUGUI magnitudeTxt;



}