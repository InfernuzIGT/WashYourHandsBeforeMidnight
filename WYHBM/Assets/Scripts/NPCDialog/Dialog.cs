using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "Scripts/NPCDialog", order = 1)]
public class Dialog : ScriptableObject
{
    [TextArea(10, 3)]
    public string[] sentences;
}