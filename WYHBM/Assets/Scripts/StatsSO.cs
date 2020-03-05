using UnityEngine;

[CreateAssetMenu(fileName = "New StatsSO", menuName = "Scriptable Objects/Stats", order = 0)]
public class StatsSO : ScriptableObject
{   
    [Header("Stats - General")]
    
    public int speed;
    public int strength;
    public int vitality;
    public int dexterity;
   
}
