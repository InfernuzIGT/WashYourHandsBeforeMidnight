using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public LayerMask enemyLayer;
    public bool isPaused; //mover a gamemanager
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPaused)
        {
            ClickDamage();
        }
    }
    public void ClickDamage()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, enemyLayer);
        
        if (hit.collider != null)
        {
        Enemy enemy = hit.collider.GetComponent<Enemy>();
        enemy.Damage(10);
        }

    }
}