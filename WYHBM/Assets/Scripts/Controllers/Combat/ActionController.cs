using UnityEngine;

namespace GameMode.Combat
{
    public class ActionController : MonoBehaviour
    {
        [Header("General")]
        public bool inAction = false;

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

        private void Start()
        {
            _actualLayer = ignoreLayer;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !CombatManager.Instance.isPaused && inAction)
            {
                ActionDamage();
            }
        }

        #region Actions

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

        #endregion

        #region Options

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

        public void Exit()
        {
            Debug.Log($"Action: Exit");
        }

        #endregion

    }
}