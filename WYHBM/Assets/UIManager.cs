using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Sprites;

public class UIManager : MonoBehaviour
{
    [Header ("Scripts")]
    public Unit unit;
    public EquipmentSO equipmentSO;
    public CharacterSO characterSO;

    [Header ("Texts")]
    public TextMeshProUGUI statsUIText;
    public TextMeshProUGUI damageUIText;
    public TextMeshProUGUI effectUIText;

    [Header ("GameObjectsUI")]
    public Image iconCharacter;
    public Image iconAbility;
    public Sprite attackIcon;
    public Sprite defenseIcon;
    public Sprite itemIcon;
    public Slider abilityBar;
    public Slider HPBar;
    public Slider HPBarEnemy;
    private string formatUI = "STR {0}\nDEX {1}";
    
    void Start()
    {
        unit.currentHP = unit.maxHP;

        statsUIText.text = string.Format(formatUI, unit.strength, unit.dexterity);
        statsUIText.lineSpacing = 1;
        // statsUIText.text ="DEX -------------->" + unit.dexterity.ToString();
        statsUIText.lineSpacing = 1;
        // statsUIText.text ="DEF -------------->" + unit.defense.ToString();
        statsUIText.lineSpacing = 1;
        // statsUIText.text ="AGI -------------->" + unit.agility.ToString();
        statsUIText.lineSpacing = 1;
        // statsUIText.text ="LCK -------------->" + unit.luck.ToString();
        statsUIText.lineSpacing = 1;
        // statsUIText.text ="RCT -------------->" + unit.reaction.ToString();

        damageUIText.text = "Damage " + equipmentSO.valueMax + "-" + equipmentSO.valueMax;
        effectUIText.text = "Effect " + equipmentSO.effectValueMin + "-" + equipmentSO.effectValueMax ;

        iconCharacter.sprite = equipmentSO.sprite;

    }

    public float UpdateBar(Unit unit)
    {
        return unit.currentHP / unit.maxHP;
    }

}
