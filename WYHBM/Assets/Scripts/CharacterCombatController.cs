using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatController : MonoBehaviour
{
    private StatsSO stats;
    private int _speedBase = 2;
    private int _damageBase = 2;
    private int _healthBase = 100;
    private int _dexterityBase = 2;
    private int _currentHealth;

private void Start()
{
    Stats();
}

private void Update()
{
    if(Input.GetKeyDown(KeyCode.Space))
    {
        Attack();
    }
}

private void Attack()
{
    _currentHealth = _healthBase;

    
    
    
}

private void Stats()
{
    _speedBase *= stats.speed;
    _damageBase *= stats.strength;
    _healthBase *= stats.vitality;
    _dexterityBase *= stats.dexterity;
}
}
