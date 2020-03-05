using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatController : MonoBehaviour
{
    public StatsSO stats;

    public int _speedBase = 2;
    public int _damageBase = 2;
    public int _healthBase = 100;
    public int _dexterityBase = 2;
    private int _currentHealth;

private void Start()
{
    Stats();
    
    _currentHealth = _healthBase;
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
    if (Input.GetKeyDown (KeyCode.Space))
    {
        _currentHealth -= _damageBase;
        
        Debug.Log ($"<b> Vida actual: </b>" + _currentHealth);
    }
}

private void Stats()
{
    _speedBase *= stats.speed;
    _damageBase *= stats.strength;
    _healthBase *= stats.vitality;
    _dexterityBase *= stats.dexterity;
}
}