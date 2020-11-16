using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Quest Data", menuName = "Quest/Data", order = 0)]
public class QuestSO : ScriptableObject
{
    [Header("Quest Data")]
    public LocalizedString title = new LocalizedString() { TableReference = "Quest_Title" };
    [Space]
    public LocalizedString description = new LocalizedString() { TableReference = "Quest_Description" };
}