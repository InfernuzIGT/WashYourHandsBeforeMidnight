using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour, IInteractable, IDialogueable
{
    [Header("NPC")]
    [SerializeField] private NPCSO _data = null;
    [SerializeField] private WaypointController _waypoints = null;

    [Header("References")]
    [SerializeField] private WorldConfig _worldConfig = null;
    [SerializeField] private InteractionNPC _interactionNPC = null;
    [SerializeField] private FieldOfView _fieldOfView = null;

    private WorldAnimator _animatorController;
    private NavMeshAgent _agent;
    private QuestEvent _questEvent;
    private bool _canMove;
    private bool _isMoving;
    private int _positionIndex = 0;
    private Quaternion _lookRotation;

    private PlayerSO _playerData;
    private Coroutine _coroutinePatrol;
    private WaitForSeconds _waitForSeconds;
    private WaitUntil _waitUntilIsMoving;

    public NPCSO Data { get { return _data; } }

    private void Awake()
    {
        _animatorController = GetComponent<WorldAnimator>();
        _agent = GetComponent<NavMeshAgent>();
        _interactionNPC = GetComponentInChildren<InteractionNPC>();

        _questEvent = new QuestEvent();
    }

    private void Start()
    {

#if UNITY_EDITOR

        gameObject.name = string.Format("NPC_{0}", _data.Name);
        _interactionNPC.gameObject.name = string.Format("InteractionNPC_{0}", _data.Name);

#endif

        _canMove = !(!_agent.isOnNavMesh && _data.CanMove || !_data.CanMove || _waypoints == null);

        _waitForSeconds = new WaitForSeconds(_data.WaitTime);
        _waitUntilIsMoving = new WaitUntil(() => _isMoving);

        _agent.updateRotation = false;

        if (_canMove)_coroutinePatrol = StartCoroutine(MovementAgent());

        if (_data.DetectPlayer)_fieldOfView.Init();
    }

    private void OnEnable()
    {
        _fieldOfView.OnFindTarget += OnFindTarget;
        _fieldOfView.OnLossTarget += OnLossTarget;
    }

    private void OnDisable()
    {
        _fieldOfView.OnFindTarget -= OnFindTarget;
        _fieldOfView.OnLossTarget -= OnLossTarget;
    }

    private void Update()
    {
        if (_agent.isOnNavMesh && _canMove)
        {
            Movement();
            Rotation();
        }
    }

    private void Movement()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animatorController.Movement(_agent.desiredVelocity);
            _agent.speed = _data.SpeedMovement;
        }
        else
        {
            _animatorController.Movement(Vector3.zero);
            _agent.ResetPath();
            _isMoving = false;
        }
    }

    private void Rotation()
    {
        if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            _lookRotation = Quaternion.LookRotation(_agent.velocity.normalized);
            _fieldOfView.transform.rotation = Quaternion.RotateTowards(_fieldOfView.transform.rotation, _lookRotation, _data.SpeedRotation * Time.deltaTime);
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

    private void OnFindTarget(Vector3 targetLastPosition)
    {
        if (!_canMove)return;

        _agent.SetDestination(targetLastPosition);
        StopCoroutine(_coroutinePatrol);
    }

    private void OnLossTarget(Vector3 targetLastPosition)
    {
        if (!_canMove)return;
        
        _agent.SetDestination(targetLastPosition);
        _coroutinePatrol = StartCoroutine(MovementAgent());
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
        GetComponent<SpriteRenderer>().sprite = _data.Sprite;
        GetComponent<Animator>().runtimeAnimatorController = _data.AnimatorController;

        Debug.Log($"<color=green><b>[NPC {_data.Name}]</b></color> Data loaded successfully!");
    }

#endif
}