using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    private Quest _quest;

    private int requiredAmount;
    private int currentAmount;

    public GOAL_TYPE goalType;

    public bool isReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void UpdateType()
    {
        switch (goalType)
        {
            case GOAL_TYPE.kill:
            currentAmount++;
            // Add executes when kill an objective
            break; 

            case GOAL_TYPE.gathering: 
            currentAmount++;
            break; 
            
        }
        
    }

    public void GatheringObject()
    {
        
        // Add executes when pick an object
        currentAmount++;
    }
        // when quest is reached delete quest from log of quest
        // Make UI follow quests superior corner left 

    public void Complete()
    {
        _quest.playerController.RemoveQuest();
        Quest.isActive = false;
        Debug.Log ($"<b> was completed! </b>");
        
    }
}