using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC", order = 0)]
public class NPCSO : ScriptableObject
{
    public new string name;

    [Header("Interaction")]
    public NPC_INTERACTION_TYPE interactionType;

    [Header("Dialog")]
    public DialogSO dialog; // TODO Mariano: REMOVE
    [PreviewTexture(48)] public Sprite icon;
    public TextAsset dialogDD;
    
    [Header("Combat")]
    public List<Enemy> combatEnemies;
}