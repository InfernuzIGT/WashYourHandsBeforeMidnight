using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = 0)]
public class Quest : ScriptableObject   
{
    [System.NonSerialized]
    public static bool isActive;

    [System.NonSerialized]
    public QuestGoal goal;
    [System.NonSerialized]
    public QuestGiver giver;
    [System.NonSerialized]
    public PlayerController playerController;
    public string title;
    [TextArea()]
    public string description;
    public int experienceReward;
    public int goldReward;
    public GOAL_TYPE goalType;

}
