using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
     [Header ("Basic Stats")]

    public int reactionBase;
    public int damageBase; 
    public int vitalityBase;
    public int dexterityBase;
    private int _currentHealth;

    [Header ("Stats Enemy")]

    public int vitality;
    public int reaction;
    public int strength;
    public int dexterity;
    public int criticPercentage;
    public int hitPercentage;
    public int dodgePercentage;

    void Start()
    {
        Stats();
    
        _currentHealth = vitalityBase;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamageEnemy();
        }
    }
    private void TakeDamageEnemy()
    {
        // if (Input.GetKeyDown (KeyCode.Space))
        // {
        //     _currentHealth -= damageBase;
        
        // Debug.Log ($"<b> Vida actual Enemy: </b>" + _currentHealth);
        // }
    }

    private void Stats()
    {
    reactionBase *= reaction;
    damageBase *= strength;
    vitalityBase *= vitality;
    dexterityBase *= dexterity;
    }
}
