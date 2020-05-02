using System.Collections.Generic;
using DG.Tweening;
// using UnityEngine;

public class Player : CombatCharacter
{
    private List<EquipmentSO> equipment;
    public List<EquipmentSO> Equipment { get { return equipment; } set { equipment = value; } }

    public override void Awake()
    {
        base.Awake();
    }
    
    public override void Start()
    {
        base.Start();

        // CreateEquipmentList();
    }

    // private void CreateEquipmentList()
    // {
    //     equipment = new List<EquipmentSO>();

    //     equipment.AddRange(character.equipmentWeapon);
    //     equipment.AddRange(character.equipmentItem);
    //     equipment.AddRange(character.equipmentArmor);

    //     equipment.Sort((e1, e2) => e1.order.CompareTo(e2.order));

    //     // CombatManager.Instance.uIController.CreateActionObjects(equipment);
    // }
    
    public override void ActionStartCombat()
    {
        base.ActionStartCombat();

        transform.
        DOMove(GameData.Instance.combatConfig.positionCombat, GameData.Instance.combatConfig.transitionDuration).
        SetEase(Ease.OutQuad);

        transform.
        DOMoveX(GameData.Instance.combatConfig.positionXCharacter, GameData.Instance.combatConfig.waitCombatDuration).
        SetEase(Ease.OutQuad).
        SetDelay(GameData.Instance.combatConfig.transitionDuration);
    }

    public override void ActionStopCombat()
    {
        base.ActionStopCombat();

        transform.
        DOMove(StartPosition, GameData.Instance.combatConfig.transitionDuration).
        SetEase(Ease.OutQuad);
    }

}