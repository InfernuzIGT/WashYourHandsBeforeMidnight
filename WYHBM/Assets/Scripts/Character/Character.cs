using UnityEngine;

public class Character : MonoBehaviour
{
    private bool inCombat = false;
    public bool InCombat { get { return inCombat; } set { inCombat = value; } }

    private CombatCharacter _combatCharacter;
    public CombatCharacter CombatCharacter { get { return _combatCharacter; } }

    private void Awake()
    {
        _combatCharacter = GetComponent<CombatCharacter>();
    }

}