using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    public float healthMax;
    // public int damage;
    public int defense;

    private float _health;

    private void Start()
    {
        _health = healthMax;
    }

    public void Damage(float damage)
    {
        //si la vida es 0 sale del metodo
        if (_health == 0)
        {
            return;
        }
        //quita vida al enemigo
        _health -= damage;
        UIManager.Instance.UpdateBarEnemy(_health    / healthMax);

        if (_health <= 0)
        {
            Debug.Log("DEAD");
            UIManager.Instance.UpdateBarEnemy(0);
            _health = 0;
        }
    }
}