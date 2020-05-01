using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    public class UIManager : MonoBehaviour
    {
        [Header ("Character")]
        public Image currentCharacter;
        public Image[] playerCharacter;
        public Image[] enemyCharacter;
        
        [Header("Texts")]
        public TextMeshProUGUI statsTxt;

        [Header("Other")]
        public SelectableButton selectableButton;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void SetUI(CombatCharacter combatCharacter)
        {
            statsTxt.text = string.Format(
                GameData.Instance.textConfig.formatStats,
                combatCharacter.StatsStrength,
                combatCharacter.StatsDexterity,
                combatCharacter.StatsDefense,
                combatCharacter.StatsAgility,
                combatCharacter.StatsLuck,
                combatCharacter.StatsReaction);
        }

        public void EnableCanvas(bool enabled)
        {
            // _canvas.enabled = enabled;
        }
    }

}