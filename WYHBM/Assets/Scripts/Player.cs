using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float healthEnemy = 100f;
    public GameObject _enemy;
    public int damage;
    public int defenseEnemy;
    public int healPotion;
    public int healthPlayer;

    void Start()
    {
        // Recibir daño y curarse (jugador)
        // barra de vida 
        // actualizar barra a 50 
        // clickear item curar jugador (no supera 100 de vida)
    }
    void Update()
    {
        if (InputAttack())
        {
            DamageEnemy();

            print(healthEnemy);

            if (healthEnemy <= 0)
            {
                DeadEnemy();
            }
        }
    }
    private bool InputAttack()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    private void DeadEnemy()
    {
        healthEnemy = 0;
        Destroy(_enemy, 0f);
    }

    public void DamageEnemy()
    {
        float result = defenseEnemy - damage;

        if (result < 0)
        {
            healthEnemy -= Mathf.Abs(result);
        }

        // healthEnemy = healthEnemy - (defenseEnemy - damage); // TODO Mariano: REVISAR
    }
    public void PlayerHeal()
    {
        healthPlayer += healPotion;
        healthPlayer = 0;
        
    }

}