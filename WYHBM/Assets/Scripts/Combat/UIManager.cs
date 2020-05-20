using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI messageTxt;

        [Header("Panel")]
        public GameObject panelPlayer;
        public GameObject panelEnemy;

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

        public void ShowPlayerPanel(bool isPlayer)
        {
            panelPlayer.SetActive(isPlayer);
            panelEnemy.SetActive(!isPlayer);

            messageTxt.text = isPlayer ? "Select Action" : "Enemy Turn";
        }

    }

}