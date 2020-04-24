using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    private QuestSO _quest;

    private int requiredAmount;
    private int currentAmount;

    private Dictionary<int, QuestSO> dictionaryQuest;

    private void Start()
    {
        dictionaryQuest = new Dictionary<int, QuestSO>();

        dictionaryQuest.Add(_quest.id, _quest);

        QuestSO test = dictionaryQuest[_quest.id];
    }

    public bool isReached()
    {
        return (currentAmount >= requiredAmount);
    }

    // Funcionalidad:
    // Actualizar el estado de una determiandad Quest
    // Identificar CUAL Quest es con un ID (key de Diccionario) EJ: dicQuest[ID]
    // Diferenciar el tipo de UPDATE con el enum GOAL_TYPE
    public void UpdateType(GOAL_TYPE goalType)
    {
        switch (goalType)
        {
            case GOAL_TYPE.kill:
                currentAmount++;
                // Add executes when kill an objective
                break;

            case GOAL_TYPE.collect:
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
        GameManager.Instance.player.RemoveQuest(); // TODO Mariano: REVIEW
        // QuestSO.isActive = false; // TODO: Ver si se usa
        Debug.Log($"<b> was completed! </b>");
    }
}