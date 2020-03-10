using UnityEngine;

[CreateAssetMenu(fileName = "New StatsSO", menuName = "Scriptable Objects/Stats", order = 0)]
public class StatsSO : ScriptableObject
{   
    [Header("Stats - General")]
    
    public int vitality;
    public int reaction;
    public int strength;
    public int dexterity;
    public int criticPercentage;
    public int hitPercentage;
    public int dodgePercentage;
   
}
