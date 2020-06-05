using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog", order = 0)]
public class DialogSO : ScriptableObject
{
    // [TextArea()]
    // public string[] preQuestSentences;
    [TextArea()]
    public string[] questSentences;
    [TextArea()]
    public string[] postQuestSentences;

    [Space]

    // TODO Mariano: Complete
    [TextArea()] public string[] noneSentences;
    [TextArea()] public string[] readySentences;
    [TextArea()] public string[] inProgressSentences;
    [TextArea()] public string[] CompletedSentences;

    public bool isCompleted; // TODO Marcos: Remove

    [Header("Quest")]
    public QuestSO questSO;

}