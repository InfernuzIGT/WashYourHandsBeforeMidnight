using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour, IInteractable, IDialogueable
{
    [SerializeField] private WorldConfig _worldConfig = null;
    [SerializeField] private NPCSO _data = null;
    [SerializeField] private WaypointController _waypoints = null;

    private NavMeshAgent _agent;
    private WorldAnimator _animatorController;
    private InteractionNPC _interactionNPC;
    private QuestEvent _questEvent;
    private bool _canMove;
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

    private PlayerSO _playerData;
    private Coroutine _coroutinePatrol;
    private WaitForSeconds _waitForSeconds;
    private WaitForSeconds _delay;
    private WaitUntil _waitUntilIsMoving;

    // Properties
    private List<Transform> _visibleTargets;
    public List<Transform> VisibleTargets { get { return _visibleTargets; } }

    private float _viewAngle = 360;
    public float ViewAngle { get { return _viewAngle; } }

    public NPCSO Data { get { return _data; } }

    private void Awake()
    {
        _animatorController = GetComponent<WorldAnimator>();
        _interactionNPC = GetComponentInChildren<InteractionNPC>();
        _agent = GetComponent<NavMeshAgent>();

        _questEvent = new QuestEvent();
    }

    private void Start()
    {
        gameObject.name = string.Format("NPC_{0}", _data.Name);
        _interactionNPC.gameObject.name = string.Format("InteractionNPC_{0}", _data.Name);

        if (!_agent.isOnNavMesh && _data.CanMove || !_data.CanMove || _waypoints == null)
        {
            // Debug.LogError($"<color=red><b>[ERROR]</b></color> NPC '{gameObject.name}' isn't on NavMesh!");
            _canMove = false;
            return;
        }

        _canMove = true;

        _waitForSeconds = new WaitForSeconds(_data.WaitTime);
        _waitUntilIsMoving = new WaitUntil(() => _isMoving);

        _visibleTargets = new List<Transform>();

        _agent.updateRotation = false;

        _coroutinePatrol = StartCoroutine(MovementAgent());

        if (_data.CanDetectPlayer)
        {
            _delay = new WaitForSeconds(0.25f);
            StartCoroutine(FindTargetsWithDelay());
        }
    }

    private void Update()
    {
        if (_agent.isOnNavMesh && _canMove)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animatorController.Movement(_agent.desiredVelocity);
            _agent.speed = _data.Speed;
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
        while (_data.CanMove)
        {
            ChangeDestination();

            yield return _waitUntilIsMoving;

            yield return _waitForSeconds;
        }
    }

    private void ChangeDestination()
    {
        if (!_agent.isStopped && !_agent.hasPath)
        {
            if (_data.UseRandomPosition)
            {
                _positionIndex = Random.Range(0, _waypoints.positions.Length);
            }
            else
            {
                _positionIndex = _positionIndex < _waypoints.positions.Length - 1 ? _positionIndex + 1 : 0;
            }

            _agent.SetDestination(_waypoints.positions[_positionIndex]);
            _isMoving = true;
        }
    }

    #region Field of View

    private IEnumerator FindTargetsWithDelay()
    {
        while (_data.CanMove)
        {
            yield return _delay;
            FindVisibleTargets();
        }
    }

    private void FindVisibleTargets()
    {
        _visibleTargets.Clear();
        _targetVisible = false;

        _targetsInViewRadius = Physics.OverlapSphere(transform.position, _data.ViewRadius, _worldConfig.layerEnemyTarget);

        for (int i = 0; i < _targetsInViewRadius.Length; i++)
        {
            _target = _targetsInViewRadius[i].transform;
            _directionToTarget = (_target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, _directionToTarget) < _viewAngle / 2)
            {
                _distanceToTarget = Vector3.Distance(transform.position, _target.position);

                if (!Physics.Raycast(transform.position, _directionToTarget, _distanceToTarget, _worldConfig.layerEnemyObstacle))
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
        if (!angleIsGlobal)angleInDegrees += transform.eulerAngles.y;
        _dirFromAngle = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        return _dirFromAngle;
    }

    #endregion

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<EnableMovementEvent>(OnStopMovement);

            if (_agent.isOnNavMesh)_agent.isStopped = true;

            _canMove = false;

            if (_playerData == null)_playerData = other.gameObject.GetComponent<PlayerController>().PlayerData;

            _animatorController?.Movement(Vector3.zero);

            _interactionNPC.Execute(true, this);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);

            if (_agent.isOnNavMesh)_agent.isStopped = false;

            _canMove = true;

            _interactionNPC.Execute(false, this);
        }
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        if (!_data.CanMove || _waypoints == null)
        {
            return;
        }

        if (_data.CanMove)_agent.isStopped = !evt.canMove;

        _animatorController?.Movement(Vector3.zero);
    }

    public void Kill()
    {
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        Destroy(gameObject);
    }

    public TextAsset GetDialogData()
    {
        return _data.Data[_playerData.ID].dialogDD;
    }

    public QuestSO GetQuestData()
    {
        return _data.Data[_playerData.ID].quest;
    }

    #region Dialogue Designer

    public void DDQuest(QUEST_STATE state)
    {
        _questEvent.data = GetQuestData();
        _questEvent.state = state;
        EventController.TriggerEvent(_questEvent);
    }

    public bool DDFirstTime()
    {
        return !GameData.Instance.CheckAndWriteID(string.Format(DDParameters.Format, gameObject.name, DDParameters.FirstTime));
    }

    public bool DDFinished()
    {
        return GameData.Instance.CheckID(string.Format(DDParameters.Format, gameObject.name, DDParameters.Finished));
    }

    public bool DDCheckQuest()
    {
        return GameData.Instance.CheckQuest(GetQuestData());
    }

    public bool DDHaveQuest()
    {
        return GameData.Instance.HaveQuest(GetQuestData());
    }

    public void DDFinish()
    {
        GameData.Instance.WriteID(string.Format(DDParameters.Format, gameObject.name, DDParameters.Finished));
    }

    #endregion

#if UNITY_EDITOR

    public void SetData()
    {
        GetComponent<SpriteRenderer>().sprite = _data.Sprite;;
        GetComponent<Animator>().runtimeAnimatorController = _data.AnimatorController;

        Debug.Log($"<color=green><b>[NPC {_data.Name}]</b></color> Data loaded successfully!");
    }

#endif
}