using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog", order = 0)]
public class DialogSO : ScriptableObject
{
    [TextArea()]
    public string[] questSentences;

    [Space]
    [TextArea()] public string[] noneSentences;
    [TextArea()] public string[] readySentences;
    [TextArea()] public string[] inProgressSentences;
    [TextArea()] public string[] CompletedSentences;
}