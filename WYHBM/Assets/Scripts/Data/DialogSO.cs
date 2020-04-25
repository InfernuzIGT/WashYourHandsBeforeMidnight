using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog", order = 0)]
public class DialogSO : ScriptableObject
{
    [TextArea()]
    public string[] sentences;

    [Header ("Quest")]
    public QuestSO questSO;
}