using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class Unit : MonoBehaviour
{
    [Header ("Variables")]
    public string name;
    public int level;
    public bool isAlive;
    public Image HPBar;

    [Header ("Stats")]
    public int damageMelee;
    public int strength;
    public int dexterity;
    public int defense;
    public int agility;
    public int luck;
    public int reaction;
    public float maxHP;
    public float currentHP;
    private List<WeaponSO> _equipmentWeapon;
    private List<ItemSO> _equipmentItem;
    private List<ArmorSO> _equipmentArmor;

    void Awake()
    {
    }


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

    public void TakeDamage(Unit unit)
    {
        if (currentHP == 0)
            return;

        int totalDamage = damageMelee * strength - defense;

        currentHP -= totalDamage;

        isAlive = currentHP >= 0;

        HPBar.DOFillAmount(currentHP / maxHP, 0.25f);
        // OnComplete(Kill);
        if (currentHP < 0)
        {
            currentHP = 0;
        }

    }

    /*Executes corresponding actions*/
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
