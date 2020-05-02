using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC", order = 0)]
public class NPCSO : ScriptableObject
{
    public new string name;

    [Header("Interaction")]
    public NPC_INTERACTION_TYPE interactionType;
    [Space]
    public DialogSO dialog;
    public List<Enemy> combatCharacters;
}