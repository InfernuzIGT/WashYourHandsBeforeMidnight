using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private List<EquipmentSO> equipment;
    public List<EquipmentSO> Equipment { get { return equipment; } set { equipment = value; } }

    private void Start()
    {
        CreateEquipmentList();
    }

    private void CreateEquipmentList()
    {
        equipment = new List<EquipmentSO>();

        equipment.AddRange(character.equipmentWeapon);
        equipment.AddRange(character.equipmentItem);
        equipment.AddRange(character.equipmentArmor);

        equipment.Sort((e1, e2) => e1.order.CompareTo(e2.order));
        
        CombatManager.Instance.uIController.CreateActionObjects(equipment);
    }

    public override void ActionReceiveDamage(float damageReceived)
    {
        base.ActionReceiveDamage(damageReceived);

    }

    public override void ActionHeal(float amountHeal)
    {
        base.ActionHeal(amountHeal);
    }

}