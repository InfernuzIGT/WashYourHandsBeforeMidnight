using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollectible : Interaction
{

    private int _index;

    private void Awake() 
    {
    }
    public override void Execute(bool enable)
    {
        base.Execute();

        QuestProgress();
    }

    private void QuestProgress()
    {
        // _goalQuest.UpdateType(GOAL_TYPE.interact);

        // if (quest.requiredAmount == quest.currentAmount)
        // {
        //     _goalQuest.Complete();
            
        // }
    }
}