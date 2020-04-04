using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Sprites;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header ("Scripts")]
    public Unit unit;
    public EquipmentSO equipmentSO;
    // public CharacterSO characterSO;

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
    public Image abilityBar;
    private string formatUI = "STR {0}\nDEX {1}\nDEF {2}\nAGI {3}\nLCK {4}\nRCT {5}";
    
    
    void Start()
    {
        statsUIText.text = string.Format(formatUI, unit.strength, unit.dexterity, unit.defense,
         unit. agility, unit.luck, unit.reaction);

        damageUIText.text = "Damage " + equipmentSO.valueMax + "-" + equipmentSO.valueMax;
        effectUIText.text = "Effect " + equipmentSO.effectValueMin + "-" + equipmentSO.effectValueMax ;

        // iconCharacter.sprite = equipmentSO.sprite;

    }


}
