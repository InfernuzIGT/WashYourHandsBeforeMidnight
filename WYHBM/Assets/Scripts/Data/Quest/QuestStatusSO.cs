using UnityEngine;

public enum QUEST_STATUS
{
    New = 0,
    Update = 1,
    Completed = 2
}

[CreateAssetMenu(fileName = "New Quest Status", menuName = "Quest/Status", order = 0)]
public class QuestStatusSO : ScriptableObject
{
    [Header("Quest Status")]
    public QUEST_STATUS state = QUEST_STATUS.New;
    [Space]
    public QuestSO previusQuest;
    public QuestSO newQuest;

}