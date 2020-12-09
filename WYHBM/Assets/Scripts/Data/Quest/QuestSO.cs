using UnityEngine;
using UnityEngine.Localization;

public enum QUEST_STATE
{
    New = 0,
    Update = 1,
    Complete = 2
}

[CreateAssetMenu(fileName = "New Quest Data", menuName = "Quest/Data", order = 0)]
public class QuestSO : ScriptableObject
{
    [Header("Quest Data")]
    public LocalizedString title = new LocalizedString() { TableReference = "Quest_Title" };
    [Space]
    public LocalizedString description = new LocalizedString() { TableReference = "Quest_Description" };
    [Space]
    [Range(0, 9)] public int steps;

    // TODO Mariano: Add Reward
}