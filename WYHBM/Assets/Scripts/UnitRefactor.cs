using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitRefactor : MonoBehaviour
{
    [Header ("Basic Stats")]

    public int reactionBase;
    public int damageBase; 
    public int vitalityBase;
    public int dexterityBase;
    private int _currentHealth;

    [Header ("Stats ")]

    public int vitality;
    public int reaction;
    public int strength;
    public int dexterity;
    public int criticPercentage;
    public int hitPercentage;
    public int dodgePercentage;

void Awake()
{
    Stats();
}
public bool TakeDamage(int dmg)
{
        _currentHealth -= dmg;

        Debug.Log ($"<b> Esta es la vida actual del personaje : </b>" + _currentHealth);
        
        if(_currentHealth <= 0)

            {
                return true;
            }
        else
            {
                return false;
            }
}

private void Stats()
{
    reactionBase *= reaction;
    damageBase *= strength;
    vitalityBase *= vitality;
    dexterityBase *= dexterity;
}
}