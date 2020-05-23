﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog", order = 0)]
public class DialogSO : ScriptableObject
{
    // [TextArea()]
    // public string[] preQuestSentences;
    [TextArea()]
    public string[] questSentences;
    [TextArea()]
    public string[] postQuestSentences;


    [Header("Quest")]
    public QuestSO questSO;

}