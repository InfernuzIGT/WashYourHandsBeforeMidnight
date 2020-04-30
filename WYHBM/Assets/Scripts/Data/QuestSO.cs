using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 0)]
public class QuestSO : ScriptableObject
{
    public int id;
    // public int goldReward; // TODO Mariano: Reemplazar por SO de Items

    [Header("Details")]
    public string title;
    [TextArea()]
    public string description;
    [Space]
    public string[] objetives;
}