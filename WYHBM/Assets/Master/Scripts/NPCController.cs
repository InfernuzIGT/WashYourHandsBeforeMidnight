using System.Collections;
using Chronos;
using Events;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour, IInteractable, IDialogueable
{
    [Header("NPC")]
    [SerializeField] private NPCSO _data = null;
    [SerializeField] private WaypointController _waypoints = null;

    [Header("Field of View")]
    [SerializeField] private DIRECTION _startLookDirection = DIRECTION.UP;
    [SerializeField, Range(0, 30)] private float _viewRadius = 7.5f;
    [SerializeField, Range(0, 30)] private float _hearRadius = 12f;
    [SerializeField, Range(0, 360)] private float _viewAngle = 135f;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private WorldConfig _worldConfig = null;
    [SerializeField, ConditionalHide] private Transform _shadow = null;
    [SerializeField, ConditionalHide] private InteractionNPC _interactionNPC = null;
    [SerializeField, ConditionalHide] private InputHoldUtility _holdUtility = null;
    [SerializeField, ConditionalHide] private FieldOfView _fieldOfView = null;
    [SerializeField, ConditionalHide] private WorldAnimator _animatorController = null;
    [SerializeField, ConditionalHide] private NavMeshAgent _agent = null;
    [SerializeField, ConditionalHide] private Timeline _timeline = null;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Vector3 _lastDestination;
    private Quaternion _lookRotation;
    private bool _canMove;
    private bool _isMoving;
    private bool _hearSound;
    private bool _backToStart;
    private bool _isDetectingPlayer;
    private int _positionIndex = 0;

    private PlayerSO _playerData;
    private Coroutine _coroutinePatrol;
    private WaitForSeconds _waitForSeconds;
    private WaitUntil _waitUntilIsMoving;
    private WaitUntil _waitUntilCanMove;

    // Events
    private QuestEvent _questEvent;
    private EnableMovementEvent _enableMovementEvent;
    private CombatEvent _combatEvent;

    // FMOD
    public StudioEventEmitter zombieRoaming;
    public StudioEventEmitter zombieFootstep;
    public StudioEventEmitter zombieAlert;

    // Properties
    public DIRECTION StartLookDirection { get { return _startLookDirection; } set { _startLookDirection = value; } }

    public NPCSO Data { get { return _data; } }
    public FieldOfView FieldOfView { get { return _fieldOfView; } }
    public float ViewRadius { get { return _viewRadius; } }
    public float ViewAngle { get { return _viewAngle; } }

    private void Start()
    {

        _questEvent = new QuestEvent();
        _enableMovementEvent = new EnableMovementEvent();


        if (_data.CanCombat)
        {
            _combatEvent = new CombatEvent();
            _combatEvent.isEnter = true;
            _combatEvent.combatEnemies.AddRange(_data.CombatEnemies);

            StartRoaming();

        }

#if UNITY_EDITOR

        gameObject.name = string.Format("NPC_{0}", _data.Name);
        _interactionNPC.gameObject.name = string.Format("InteractionNPC_{0}", _data.Name);

#endif

        _canMove = GetCanMove();

        // if (_agent.isOnNavMesh && !_data.CanMove)
        // {
        //     _agent.areaMask = 0;
        // }

        _waitForSeconds = new WaitForSeconds(_data.WaitTime);
        _waitUntilIsMoving = new WaitUntil(() => _isMoving);
        _waitUntilCanMove = new WaitUntil(() => _canMove);

        _agent.updateRotation = false;

        if (_data.DetectPlayer)
        {
            _originalPosition = transform.position;
            _originalRotation = Quaternion.Euler(0, GetLookDirection(_startLookDirection), 0);

            _fieldOfView.transform.rotation = _originalRotation;

            _fieldOfView.Init(_data.TimeToDetect, _viewRadius, _viewAngle, _timeline);

            _holdUtility.Duration = _data.TimeToDetect;
            _holdUtility.OnFinished.AddListener(OnFinish);
        }

        _coroutinePatrol = StartCoroutine(MovementAgent());
    }

    private void OnEnable()
    {
        _fieldOfView.OnFindTarget += OnFindTarget;
        _fieldOfView.OnLossTarget += OnLossTarget;

        EventController.AddListener<CombatEvent>(OnCombat);
    }

    private void OnDisable()
    {
        _fieldOfView.OnFindTarget -= OnFindTarget;
        _fieldOfView.OnLossTarget -= OnLossTarget;

        EventController.RemoveListener<CombatEvent>(OnCombat);
    }

    private void StartRoaming()
    {
        zombieRoaming.Play();
    }

    private void OnCombat(CombatEvent evt)
    {
        if (evt.isEnter)
        {

        }
        else
        {
            // if (evt.isWin && _isDetectingPlayer)Destroy(gameObject);

            if (_isDetectingPlayer)
            {
                InteractionCorpse corpse = Instantiate(_worldConfig.interactionCorpse, _shadow.position, Quaternion.identity);
                corpse.Init(_data.SpriteCorpse);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (_agent.isOnNavMesh && _canMove && _timeline.timeScale > 0)
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
            if (_backToStart)
            {
                _backToStart = false;
                _fieldOfView.transform.rotation = _originalRotation;
                _canMove = GetCanMove();
            }

            if (_hearSound)
            {
                _hearSound = false;
                _holdUtility.SoundDetect(false);
                _fieldOfView.UpdateView(_data.TimeToDetect, _viewRadius, _viewAngle);

                _agent.ResetPath();
                _lastDestination = _originalPosition;
                _agent.SetDestination(_lastDestination);

                _backToStart = true;
                _isMoving = true;


            }
            else
            {
                _animatorController.Movement(Vector3.zero);
                _agent.ResetPath();
                _isMoving = false;
            }
        }
    }

    private void ZombieFootstep()
    {
        zombieFootstep.Play();
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
        while (true)
        {
            yield return _waitUntilCanMove;

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

            _lastDestination = _waypoints.positions[_positionIndex];
            _agent.SetDestination(_lastDestination);
            _isMoving = true;
        }
    }

    public void SetDestination(Vector3 newDestination)
    {
        if (_data.DetectPlayer && _agent.isOnNavMesh)
        {
            _agent.SetDestination(newDestination);
            _isMoving = true;

            _hearSound = true;
            _holdUtility.SoundDetect(true);
            _fieldOfView.UpdateView(_data.TimeToDetect, _hearRadius, _viewAngle);

            _canMove = true;

        }
    }

    #region Field of View

    private void OnFindTarget(Vector3 targetLastPosition)
    {
        if (_canMove)
        {
            zombieAlert.Play();

            _agent.SetDestination(targetLastPosition);
            if (_coroutinePatrol != null) StopCoroutine(_coroutinePatrol);

        }

        _holdUtility.OnStart();
    }

    private void OnLossTarget(Vector3 targetLastPosition)
    {
        if (_canMove)
        {
            _agent.SetDestination(targetLastPosition);
            _coroutinePatrol = StartCoroutine(MovementAgent());
        }

        _holdUtility.OnCancel();
    }

    public void OnFinish()
    {
        _isDetectingPlayer = true;
        _fieldOfView.SetState(false);

        _isMoving = false;
        _canMove = false;
        if (_agent.isOnNavMesh) _agent.isStopped = true;

        _animatorController.Movement(Vector3.zero);
        _animatorController.Detected(true);

        _enableMovementEvent.canMove = false;
        _enableMovementEvent.isDetected = true;
        EventController.TriggerEvent(_enableMovementEvent);

        _combatEvent.enemy = transform;
        EventController.TriggerEvent(_combatEvent);
    }

    #endregion

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<EnableMovementEvent>(OnStopMovement);

            if (_data.DetectPlayer)
            {
                _holdUtility.OnFinish();
            }
            else
            {
                if (_agent.isOnNavMesh) _agent.isStopped = true;

                _canMove = false;

                if (_playerData == null) _playerData = other.gameObject.GetComponent<PlayerController>().PlayerData;

                _animatorController?.Movement(Vector3.zero);

                _interactionNPC.Execute(true, this);
            }

        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);

            if (_agent.isOnNavMesh) _agent.isStopped = false;

            _canMove = GetCanMove();

            _interactionNPC.Execute(false, this);
        }
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        if (!_data.CanMove || _waypoints == null)
        {
            return;
        }

        if (_data.CanMove) _agent.isStopped = !evt.canMove;

        _animatorController?.Movement(Vector3.zero);
    }

    public float GetLookDirection(DIRECTION direction)
    {
        switch (direction)
        {
            case DIRECTION.UP:
                return 0;

            case DIRECTION.DOWN:
                return 180;

            case DIRECTION.LEFT:
                return -90;

            case DIRECTION.RIGHT:
                return 90;
        }

        return 0;
    }

    private bool GetCanMove()
    {
        return !(!_agent.isOnNavMesh && _data.CanMove || !_data.CanMove || _waypoints == null);
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
        return GameData.Instance.CheckQuestCurrentStep(GetQuestData());
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