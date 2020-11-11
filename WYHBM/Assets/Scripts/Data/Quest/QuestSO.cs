using UnityEngine;
using UnityEngine.Localization;

public enum QUEST
{
    None = 0
}

[CreateAssetMenu(fileName = "New Quest Data", menuName = "Quest/Data", order = 0)]
public class QuestSO : ScriptableObject
{
    [Header("Quest Data")]
    public QUEST id = QUEST.None;
    [Space]
    public LocalizedString title = new LocalizedString() { TableReference = "Quest_Title" };
    [Space]
    public LocalizedString description = new LocalizedString() { TableReference = "Quest_Description" };
    [Space]
    public LocalizedString[] objetives = new LocalizedString[] { new LocalizedString { TableReference = "Quest_Objetive" } };

}