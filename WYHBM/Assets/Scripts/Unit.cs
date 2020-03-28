using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    [Header ("Variables")]
    public string unitName;
    public int unitLevel;

    [Header ("Stats")]
    public int damageMelee;
    public int strength;
    public int dexterity;
    public int defense;
    public int agility;
    public int luck;
    public int reaction;
    public int maxHP;
    public int currentHP;
    private List<WeaponSO> _equipmentWeapon;
    private List<ItemSO> _equipmentItem;
    private List<ArmorSO> _equipmentArmor;

    public virtual void Start()
    {
        _equipmentWeapon = new List<WeaponSO>();
        _equipmentItem = new List<ItemSO>();
        _equipmentArmor = new List<ArmorSO>();
    }
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
        currentHP += defense;
    }

    public void UseItem()
    {
        // Seleccioner que item usar y a quien
        // Setear beneficios de usar item
        Debug.Log ($"<b> Player use an item </b>");
    }
    
    public void Escape()
    {
        // El personaje vuelve al mundo y escapa del combate
        // Aplicar dolencia por escape
        Debug.Log ($"<b> Player escape from the enemy </b>");
        
    }
    
}
