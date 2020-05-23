using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
public class QuestSO : ScriptableObject
{
    public string title;
    [TextArea()]
    public string description;
    [Space]
    public string[] objetives;
}