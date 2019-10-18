using System.Collections;
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
        private bool _isActionEnable;
        private LayerMask _actualLayer;
        private Ray _ray;
        private RaycastHit2D _hit;
        private Player _player;
        private Enemy _enemy;

        private WaitForSeconds combatTransition;
        private WaitForSeconds combatWaitTime;

        private ShakeEvent shakeEvent = new ShakeEvent();

        private void Start()
        {
            combatTransition = new WaitForSeconds(GameData.Instance.combatConfig.transitionDuration);
            combatWaitTime = new WaitForSeconds(GameData.Instance.combatConfig.waitCombatDuration);

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

        public void StartCombat()
        {
            _isActionEnable = !CombatManager.Instance.isPaused && CombatManager.Instance.isTurnPlayer;

            if (!_isActionEnable)
                return;

            CombatManager.Instance.isTurnPlayer = false;

            StartCoroutine(CombatPlayer());
        }

        private IEnumerator CombatPlayer()
        {
            // TODO Mariano: Fade IN
            CombatManager.Instance.FadeOutCanvas();
            CombatManager.Instance.listPlayers[0].ActionStartCombat();
            CombatManager.Instance.listEnemies[0].ActionStartCombat();

            yield return combatTransition;

            PlayAction();
            CombatManager.Instance.uIController.ChangeUI(!CombatManager.Instance.listEnemies[0].IsAlive);
            // CombatManager.Instance.uIController.ChangeUI(false);

            yield return combatWaitTime;

            // TODO Mariano: Fade OUT
            CombatManager.Instance.FadeInCanvas();
            CombatManager.Instance.listPlayers[0].ActionStopCombat();
            CombatManager.Instance.listEnemies[0].ActionStopCombat();

            yield return combatTransition;

            // TODO Mariano: Redo THIS!
            //-------------------------------

            yield return new WaitForSeconds(1.25f);

            if (CombatManager.Instance.listEnemies[0].IsAlive)
            {
                CombatManager.Instance.FadeOutCanvas();
                CombatManager.Instance.listPlayers[0].ActionStartCombat();
                CombatManager.Instance.listEnemies[0].ActionStartCombat();

                yield return combatTransition;

                CombatManager.Instance.listPlayers[0].ActionReceiveDamage(Random.Range(19, 23));
                EventController.TriggerEvent(shakeEvent);
                CombatManager.Instance.uIController.ChangeUI(true);

                yield return combatWaitTime;

                CombatManager.Instance.FadeInCanvas();
                CombatManager.Instance.listPlayers[0].ActionStopCombat();
                CombatManager.Instance.listEnemies[0].ActionStopCombat();

                yield return combatTransition;

                CombatManager.Instance.isTurnPlayer = true;
            }
            else
            {
                CombatManager.Instance.EndGame(true);
            }

        }

        private void PlayAction()
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