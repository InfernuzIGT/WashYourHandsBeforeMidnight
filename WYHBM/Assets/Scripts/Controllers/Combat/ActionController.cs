﻿using System.Collections;
using Events;
using UnityEngine;

namespace GameMode.Combat
{
    public class ActionController : MonoBehaviour
    {
        public ACTION_TYPE actionActual;
        public float actionValue;

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

        private void Update()
        {
            _isActionEnable = Input.GetMouseButtonDown(0) && !CombatManager.Instance.isPaused && inAction && CombatManager.Instance.isTurnPlayer;

            if (_isActionEnable)
            {
                ActionDamage();
            }
        }

        public void ChooseAction(ActionSO _action, float _value)
        {
            actionActual = _action.actionType;
            actionValue = _value;
        }

        public void Play()
        {
            Debug.Log($"<b> Play action: {actionActual.ToString()} </b>");
            // TODO Mariano: Modo accion, donde desaparece la UI y se muestra la accion a realizar.

            switch (actionActual)
            {
                case ACTION_TYPE.weapon:
                    CombatManager.Instance.listEnemies[0].ActionReceiveDamage(actionValue);
                    break;
                case ACTION_TYPE.defense:
                    // TODO Mariano: Desaparece UI
                    // TODO Mariano: Modo Defensa
                    break;
                case ACTION_TYPE.itemPlayer:
                    CombatManager.Instance.listPlayers[0].ActionHeal(actionValue);
                    break;
                case ACTION_TYPE.itemEnemy:
                    // TODO Mariano: Seleccionar enemigo
                    break;

                default:
                    break;
            }

            // TODO Mariano: Redo THIS!
            CombatManager.Instance.listPlayers[0].ActionReceiveDamage(Random.Range(10f, 20f));
        }

        private RaycastHit2D GetHit(LayerMask hitLayer)
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics2D.Raycast(_ray.origin, _ray.direction, 10, hitLayer);
        }

        public void ActionDamage()
        {
            _hit = GetHit(enemyLayer);

            if (_hit.collider != null)
            {
                inAction = false;

                _actualLayer = ignoreLayer;

                // TODO Mariano: Identificar al enemigo de otra manera
                _enemy = _hit.collider.GetComponent<Enemy>();
                _enemy.ActionReceiveDamage(10);
            }

        }

        // public void ActionHeal()
        // {
        //     _hit = GetHit(_actualLayer);

        //     if (_hit.collider != null)
        //     {
        //         inAction = false;

        //         _actualLayer = ignoreLayer;

        //         _player = _hit.collider.GetComponent<Player>();
        //         _player.ActionHeal(10);
        //     }
        // }

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