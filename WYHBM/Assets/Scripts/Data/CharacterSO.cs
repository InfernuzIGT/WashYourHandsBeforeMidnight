using UnityEngine;

public class CharacterSO : ScriptableObject
{
    [Header("General")]
    public new string name;

    [Header("Stats")]
    public int statsHealthMax = 100;
    public int statsDamage = 10;
    public int statsDefense = 5;
    public int statsReaction = 1;
}