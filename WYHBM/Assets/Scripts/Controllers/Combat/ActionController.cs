using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

namespace GameMode.Combat
{
    public class ActionController : MonoBehaviour
    {
        public ACTION_TYPE actionActual;
        public int actionValue;

        [Header("General")]
        public bool inAction = false;
        public bool canDoAction = true;

        [Header("Layers")]
        public LayerMask playerLayer;
        public LayerMask enemyLayer;
        public LayerMask ignoreLayer;

        // Private variables
        private LayerMask _actualLayer;
        private Ray _ray;
        private RaycastHit2D _hit;
        private Player _player;
        private Enemy _enemy;

        private ShakeEvent shakeEvent = new ShakeEvent();

        private void Start()
        {
            _actualLayer = ignoreLayer;
        }

        private void OnEnable()
        {
            EventController.AddListener<FadeOutEvent>(FadeOut);
        }

        private void OnDisable()
        {
            EventController.RemoveListener<FadeOutEvent>(FadeOut);
        }

        public void ChooseAction(EquipmentSO _equipment, int _minValue, int _maxValue)
        {
            actionActual = _equipment.actionType;
            actionValue = Random.Range(_minValue, _maxValue);
        }

        public void PlayAction(List<Player> players)
        {
            switch (actionActual)
            {
                case ACTION_TYPE.weapon:
                    CombatManager.Instance.listEnemies[0].ActionReceiveDamage(actionValue);
                    EventController.TriggerEvent(shakeEvent);
                    break;

                case ACTION_TYPE.defense:

                    break;

                case ACTION_TYPE.itemPlayer:
                    CombatManager.Instance.listPlayers[0].ActionHeal(actionValue);
                    break;

                case ACTION_TYPE.itemEnemy:
                    break;

                default:
                    break;
            }
        }
        public void PlayAction(List<Enemy> enemies)
        {
            // TODO Mariano: REDO THIS!
            
            CombatManager.Instance.listPlayers[0].ActionReceiveDamage(Random.Range(19, 23));
            EventController.TriggerEvent(shakeEvent);

            // switch (actionActual)
            // {
            //     case ACTION_TYPE.weapon:
            //         CombatManager.Instance.listPlayers[0].ActionReceiveDamage(Random.Range(19, 23));
            //         EventController.TriggerEvent(shakeEvent);
            //         break;

            //     case ACTION_TYPE.defense:

            //         break;

            //     case ACTION_TYPE.itemPlayer:
            //         CombatManager.Instance.listEnemies[0].ActionHeal(actionValue);
            //         break;

            //     case ACTION_TYPE.itemEnemy:
            //         break;

            //     default:
            //         break;
            // }
        }

        private RaycastHit2D GetHit(LayerMask hitLayer)
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics2D.Raycast(_ray.origin, _ray.direction, 10, hitLayer);
        }

        public void Attack()
        {
            Debug.Log($"Action: Attack");

            inAction = true;
            _actualLayer = enemyLayer;
        }

        public void Defense()
        {
            Debug.Log($"Action: Defense");
        }

        public void Item()
        {
            Debug.Log($"Action: Item");

            inAction = true;
            _actualLayer = playerLayer;
        }

        public void Run()
        {
            Debug.Log($"Action: Run");
        }

        private void FadeIn(FadeInEvent evt)
        {
            StartCoroutine(StartFadeIn(evt.duration));
        }

        private void FadeOut(FadeOutEvent evt)
        {
            StartCoroutine(StartFadeOut(evt.duration));
        }

        private IEnumerator StartFadeIn(float duration)
        {
            canDoAction = true;
            yield return new WaitForSeconds(duration);
            canDoAction = false;
        }

        private IEnumerator StartFadeOut(float duration)
        {
            canDoAction = false;
            yield return new WaitForSeconds(duration);
            canDoAction = true;
        }

    }
}