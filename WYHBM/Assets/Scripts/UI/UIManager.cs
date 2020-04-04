using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    public class UIManager : MonoBehaviour
    {
        [Header("Texts")]
        public TextMeshProUGUI statsUIText;
        public TextMeshProUGUI damageUIText;
        public TextMeshProUGUI effectUIText;

        [Header("GameObjectsUI")]
        public Image iconCharacter;
        public Image iconAbility;
        public Sprite attackIcon;
        public Sprite defenseIcon;
        public Sprite itemIcon;
        public Image abilityBar;
        private string formatUI = "STR {0}\nDEX {1}\nDEF {2}\nAGI {3}\nLCK {4}\nRCT {5}";

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void SetUI(CombatCharacter combatCharacter)
        {
            statsUIText.text = string.Format(
                formatUI,
                combatCharacter.StatsStrength,
                combatCharacter.StatsDexterity,
                combatCharacter.StatsDefense,
                combatCharacter.StatsAgility,
                combatCharacter.StatsLuck,
                combatCharacter.StatsReaction);

            // damageUIText.text = "Damage " + equipmentSO.valueMax + "-" + equipmentSO.valueMax;
            // effectUIText.text = "Effect " + equipmentSO.effectValueMin + "-" + equipmentSO.effectValueMax;

            // TODO Mariano: Todos los personajes deben tener un Icono/Portada/Imagen
            // iconCharacter.sprite = equipmentSO.sprite; 

        }

        public void EnableCanvas(bool enabled)
        {
            _canvas.enabled = enabled;
        }
    }

}