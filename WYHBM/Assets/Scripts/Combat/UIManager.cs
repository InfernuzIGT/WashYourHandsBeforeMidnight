using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameMode.Combat
{
    [RequireComponent(typeof(Canvas))]
    public class UIManager : MonoBehaviour
    {
        [Header("General")]
        public TextMeshProUGUI messageTxt;
        public GameObject panelPlayer;
        public GameObject panelEnemy;
        [Space]
        public List<Turn> turn;

        [Header("Actions")]
        public Transform actionsContainer;
        public List<Actions> actions;

        private List<Turn> _turn;
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
            _turn = new List<Turn>();
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

        public void CreateTurn(List<CombatCharacter> characters)
        {
            _turn.Clear();

            for (int i = 0; i < characters.Count; i++)
            {
                _turn.Add(turn[i]);
                _turn[i].SetSprite(characters[i]);
                _turn[i].gameObject.SetActive(true);
            }
        }

        public void ReorderTurn(List<CombatCharacter> characters)
        {
            for (int i = 0; i < _turn.Count; i++)
            {
                _turn[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < characters.Count; i++)
            {
                _turn[i].SetSprite(characters[i]);
                _turn[i].gameObject.SetActive(true);
            }
        }

        public void ClearTurn()
        {
            for (int i = 0; i < turn.Count; i++)
            {
                turn[i].gameObject.SetActive(false);
            }

            _turn.Clear();
        }

    }

}