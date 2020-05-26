using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMode.Combat
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoBehaviour
    {
        [Header("General")]
        public TextMeshProUGUI messageTxt;
        public GameObject panelPlayer;
        public GameObject panelEnemy;

        [Header("Actions")]
        public Transform actionsContainer;
        public List<Actions> actions;

        [Header("Character")]
        public Image currentCharacter;
        public Image[] playerCharacter;
        public Image[] enemyCharacter;

        private Canvas _canvas;
        private Actions _actions;
        private int _lastIndex = 0;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            actions = new List<Actions>();
        }

        public void CreateActions(List<ItemSO> items)
        {
            _actions = Instantiate(GameData.Instance.combatConfig.actionsPrefab, actionsContainer);
            _actions.Init(items);
            _actions.gameObject.SetActive(false);
            actions.Add(_actions);
        }

        public void ShowActions(int index)
        {
            actions[_lastIndex].gameObject.SetActive(false);
            actions[index].gameObject.SetActive(true);

            _lastIndex = index;
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