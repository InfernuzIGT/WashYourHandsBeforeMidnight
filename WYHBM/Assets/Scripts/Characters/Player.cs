using UnityEngine;

public class Player : Character
{
    [Header("Custom")]
    public float healthEnemy = 100f;
    public GameObject _enemy;
    public int defenseEnemy;
    public int healPotion;
    public int healthPlayer;

    private void Update()
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