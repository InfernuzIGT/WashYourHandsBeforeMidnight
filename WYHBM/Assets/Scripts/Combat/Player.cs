using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CombatAnimator))]
public class Player : CombatCharacter
{
    private List<ItemSO> equipment;
    public List<ItemSO> Equipment { get { return equipment; } set { equipment = value; } }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        // CreateEquipmentList();
    }

    /// <summary>
    /// Espera la accion
    /// </summary>
    public override IEnumerator WaitingForAction()
    {
        base.WaitingForAction();

        GameManager.Instance.combatUI.ShowPlayerPanel(true);

        _isActionDone = false;

        while (!_isActionDone)
        {
            yield return null;
        }
        
        GameManager.Instance.combatUI.ShowPlayerPanel(false);

        Debug.Log($"<color=green><b> [COMBAT] </b></color> Action by {gameObject.name}");

        yield return _waitPerAction;

        AnimationAction(COMBAT_STATE.Idle);

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

    public override void CheckCharacters()
    {
        base.CheckCharacters();

        GameManager.Instance.combatManager.CheckGame(this);
    }

}