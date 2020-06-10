using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour, IInteractable
{
    [Header("Movement")]
    public bool canMove = true;
    public WaypointController waypoints;
    public bool useRandomPosition = true;
    [Range(0f, 10f)]
    public float waitTime = 5;
    [Range(0f, 10f)]
    public float speed = 5;

    [Header("Field of View")]
    public bool canDetectPlayer;
    [Range(0f, 50f)]
    public float viewRadius = 10;

    private WorldAnimator _animatorController;
    private InteractionNPC _interactionNPC;
    private NavMeshAgent _agent;
    private bool _isMoving;
    private int _positionIndex = 0;

    // Field of View
    private bool _targetVisible;
    private bool _targetDetected;
    private Collider[] _targetsInViewRadius;
    private Transform _target;
    private Vector3 _targetLastPosition;
    private Vector3 _directionToTarget;
    private float _distanceToTarget;
    private Vector3 _dirFromAngle;

    private Coroutine _coroutinePatrol;
    private WaitForSeconds _waitForSeconds;
    private WaitForSeconds _delay;

    private List<Transform> _visibleTargets;
    public List<Transform> VisibleTargets { get { return _visibleTargets; } }

    private float _viewAngle = 360;
    public float ViewAngle { get { return _viewAngle; } }

    private void Awake()
    {
        _animatorController = GetComponent<WorldAnimator>();
        _interactionNPC = GetComponentInChildren<InteractionNPC>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (!_agent.isOnNavMesh && canMove)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> NPC '{gameObject.name}' isn't on NavMesh!");
            return;
        }

        if (!canMove || waypoints == null)
        {
            return;
        }

        _waitForSeconds = new WaitForSeconds(waitTime);

        _visibleTargets = new List<Transform>();

        _agent.updateRotation = false;

        _coroutinePatrol = StartCoroutine(MovementAgent());

        if (canDetectPlayer)
        {
            _delay = new WaitForSeconds(0.25f);
            StartCoroutine(FindTargetsWithDelay());
        }
    }

    private void Update()
    {
        if (_agent.isOnNavMesh && canMove)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animatorController.Movement(_agent.desiredVelocity);
            _agent.speed = speed;
        }
        else
        {
            _animatorController.Movement(Vector3.zero);
            _agent.ResetPath();
            _isMoving = false;
        }
    }

    private IEnumerator MovementAgent()
    {
        while (canMove)
        {
            ChangeDestination();

            while (_isMoving)
            {
                yield return null;
            }

            yield return _waitForSeconds;
        }
    }

    private void ChangeDestination()
    {
        if (!_agent.isStopped && !_agent.hasPath)
        {
            if (useRandomPosition)
            {
                _positionIndex = Random.Range(0, waypoints.positions.Length);
            }
            else
            {
                _positionIndex = _positionIndex < waypoints.positions.Length - 1 ? _positionIndex + 1 : 0;
            }

            _agent.SetDestination(waypoints.positions[_positionIndex]);
            _isMoving = true;
        }
    }

    #region Field of View

    private IEnumerator FindTargetsWithDelay()
    {
        while (canMove)
        {
            yield return _delay;
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        _visibleTargets.Clear();
        _targetVisible = false;

        _targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, GameData.Instance.worldConfig.layerEnemyTarget);

        for (int i = 0; i < _targetsInViewRadius.Length; i++)
        {
            _target = _targetsInViewRadius[i].transform;
            _directionToTarget = (_target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, _directionToTarget) < _viewAngle / 2)
            {
                _distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (!Physics.Raycast(transform.position, _directionToTarget, _distanceToTarget, GameData.Instance.worldConfig.layerEnemyObstacle))
                {
                    _visibleTargets.Add(_target);
                    _targetLastPosition = _target.transform.position;
                    _targetVisible = true;
                    _targetDetected = true;

                    StopCoroutine(_coroutinePatrol);
                    _agent.SetDestination(_targetLastPosition);
                }
            }
        }

        if (_targetDetected && !_targetVisible)
        {
            _targetDetected = false;
            _agent.SetDestination(_targetLastPosition);
            _coroutinePatrol = StartCoroutine(MovementAgent());
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
        _dirFromAngle = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        return _dirFromAngle;
    }

    #endregion

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<EnableMovementEvent>(OnStopMovement);
            _agent.isStopped = true;
            canMove = false;
            
            _animatorController?.Movement(Vector3.zero);

            _interactionNPC.Execute(true, this);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
            _agent.isStopped = false;
            canMove = true;

            _interactionNPC.Execute(false, this);
        }
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        if (!canMove || waypoints == null)
        {
            return;
        }

        if (canMove) _agent.isStopped = !evt.canMove;

        _animatorController?.Movement(Vector3.zero);
    }

    public void Kill()
    {
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        Destroy(gameObject);
    }
}