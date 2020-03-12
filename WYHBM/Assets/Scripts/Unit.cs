using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header ("Variables")]
    public string unitName;
    public int unitLevel;

    [Header ("Stats")]
    public int damageMelee;
    public int strength;
    public int agility;
    public int maxHP;
    public int currentHP;
    public int defense;

    public void Stats()
    {
        damageMelee *= strength;
    }

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Agility()
    {
        // Bolsa de probabilidad de esquivar y acertar hit.

        // float Choose (float[] probs) {

        // float total = 0;

        // foreach (float elem in probs) {
        //     total += elem;
        // }

        // float randomPoint = Random.value * total;

        // for (int i= 0; i < probs.Length; i++) {
        //     if (randomPoint < probs[i]) {
        //         return i;
        //     }
        //     else {
        //         randomPoint -= probs[i];
        //     }
        // }
        // return probs.Length - 1;
    }
    
    public void Luck()
    {
        // Bolsa de probabilidad de acertar critico.
        // https://gist.github.com/angeldelrio/0aa70ca63e0e153c6022
        
    }

    public void Defense()
    {
        

    }
    
}
