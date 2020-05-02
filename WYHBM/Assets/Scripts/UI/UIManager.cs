using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI actionTxt;

        [Header("Character")]
        public Image currentCharacter;
        public Image[] playerCharacter;
        public Image[] enemyCharacter;

        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        public void EnableCanvas(bool enabled)
        {
            _canvas.enabled = enabled;
        }
    }

}