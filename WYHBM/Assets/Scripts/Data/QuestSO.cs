using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
public class QuestSO : ScriptableObject
{
    public int id;
    public GOAL_TYPE goalType;

    [Header("Details")]
    public string title;
    [TextArea()]
    public string description;
    public int goldReward; // TODO Mariano: Ver tipos de Rewards
}