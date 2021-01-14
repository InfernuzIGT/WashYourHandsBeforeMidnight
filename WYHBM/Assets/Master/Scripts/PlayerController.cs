using System.Collections.Generic;
using Events;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(CharacterController), typeof(DeviceUtility))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, ReadOnly] private PlayerSO _playerData = null;
    [SerializeField] private PlayerConfig _playerConfig = null;
    // [SerializeField] private FMODConfig _fmodConfig = null;

    [Header("Review")]
    [SerializeField, ReadOnly] private MOVEMENT_STATE _movementState = MOVEMENT_STATE.Walk;
    public bool _inZipline = false;
    public Vector3 endPos;
    public bool ledgeDetected;
    public Transform wallCheck;
    public Transform ledgeCheck;
    public float wallCheckDistance;

    [Header("FMOD")]
    public StudioEventEmitter footstepSound;
    public StudioEventEmitter breathingSound;

    // References
    private CharacterController _characterController;
    private WorldAnimator _animatorController;
    private DeviceUtility _deviceUtility;

    // Events
    private InteractionEvent _interactionEvent;
    private LadderEvent _ladderEvent;
    private PauseEvent _pauseEvent;

    // Movement 
    private MOVEMENT_STATE _lastMovementState;
    private Vector2 _inputMovement;
    private Vector2 _inputMovementAux;
    private Vector3 _crouchHeight;
    private Vector3 _movement;
    private bool _canMove = true;
    private float _speedHorizontal;
    private float _speedVertical;
    private bool _isInteracting;
    private bool _isCrouching;

    //Jump
    // private float _jump = 9.81f;
    private bool _isJumping;

    // Ivy
    // private float _speedIvy = 5f;
    private bool _inIvy = false;
    private RaycastHit _hitBot;
    private Vector3 _botPosition;

    // Ledge
    private Vector3 newPos;

    private Vector3 _lastPosition;

    // Footstep
    private RaycastHit _hit;

    // Quest
    private bool _isOpenDiary;
    
    // Interaction
    private Interaction _currentInteraction;

    // Properties
    private CustomInputAction _input;
    public CustomInputAction Input { get { return _input; } set { _input = value; } }

    private bool _canPlayFootstep;
    public bool CanPlayFootstep { get { return _canPlayFootstep; } }

    private bool _isDetectingGround;
    public bool IsDetectingGround { get { return _isDetectingGround; } set { _isDetectingGround = value; } }

    public PlayerSO PlayerData { get { return _playerData; } }
    public Interaction CurrentInteraction { get { return _currentInteraction; } set { _currentInteraction = value; } }

    Dictionary<int, QuestSO> questLog = new Dictionary<int, QuestSO>();

    private void Awake()
    {
        CreateInput();

        _characterController = GetComponent<CharacterController>();
        _animatorController = GetComponent<WorldAnimator>();
        _deviceUtility = GetComponent<DeviceUtility>();
    }

    private void CreateInput()
    {
        _input = new CustomInputAction();

        _input.Player.Move.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        // _input.Player.Jump.performed += ctx => Jump();
        _input.Player.Interaction.performed += ctx => Interaction();
        _input.Player.Pause.performed += ctx => Pause(PAUSE_TYPE.PauseMenu);
        _input.Player.Options.performed += ctx => Pause(PAUSE_TYPE.Inventory);
        _input.Player.Crouch.performed += ctx => Crouch();
        // _input.Player.Run.started += ctx => Run(true);
        // _input.Player.Run.canceled += ctx => Run(false);

        // _input.Player.DebugMode.performed += ctx => GameData.Instance.SelectNextQuality(true);

        _input.Player.Enable();
        _input.UI.Disable();
    }

    private void Start()
    {
        _interactionEvent = new InteractionEvent();
        _ladderEvent = new LadderEvent();
        _pauseEvent = new PauseEvent();

        _crouchHeight = new Vector3(0, -(_playerConfig.height / 2) / 2, 0);

        _deviceUtility.DetectDevice();
    }

    private void OnEnable()
    {
        EventController.AddListener<EnableMovementEvent>(OnStopMovement);
        EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);
        EventController.AddListener<ChangePositionEvent>(OnChangePosition);
        EventController.AddListener<CurrentInteractEvent>(OnCurrentInteraction);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
        EventController.RemoveListener<ChangePositionEvent>(OnChangePosition);
        EventController.RemoveListener<CurrentInteractEvent>(OnCurrentInteraction);
    }

    private void Pause(PAUSE_TYPE pauseType)
    {
        _pauseEvent.pauseType = pauseType;
        EventController.TriggerEvent(_pauseEvent);
    }

    private void Update()
    {
        Movement();
        // IvyMovement();
    }

    private void StartClimb()
    {
        _characterController.enabled = false;

        _animatorController.ClimbLedge(true);

    }

    private void EndClimb()
    {
        transform.position = newPos;

        _canMove = true;

        _animatorController.ClimbLedge(false);

        ledgeDetected = false;

        _characterController.enabled = true;
    }

    // private void OnGUI()
    // {
    //     GUIStyle guiStyle = new GUIStyle();
    //     guiStyle.fontSize = 50;
    //     GUI.Label(new Rect(10, 10, 100, 20), string.Format("Velocity Y: {0}", _characterController.velocity.y.ToString()), guiStyle);
    // }

    private void Movement()
    {
        // Stop animation
        if (!_canMove)
        {
            StopMovement();
            return;
        }

        switch (_movementState)
        {
            case MOVEMENT_STATE.Walk:

                _speedHorizontal = _playerConfig.speedJogging;

                footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);

                _animatorController.Walk(false);

                // TODO Mariano: Enable
                // if (Mathf.Abs(_inputMovement.x) > _playerConfig.axisLimit || Mathf.Abs(_inputMovement.y) > _playerConfig.axisLimit)
                // {
                //     _speedHorizontal = _playerConfig.speedJogging;

                //     footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);

                //     _animatorController.Walk(false);
                // }
                // else
                // {
                //     _speedHorizontal = _playerConfig.speedWalk;

                //     footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);

                //     _animatorController.Walk(true);
                // }
                break;

            case MOVEMENT_STATE.Run:
                _speedHorizontal = _playerConfig.speedRun;
                footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
                break;

            case MOVEMENT_STATE.Crouch:
                if (Mathf.Abs(_inputMovement.x) > _playerConfig.axisLimit || Mathf.Abs(_inputMovement.y) > _playerConfig.axisLimit)
                {
                    _speedHorizontal = _playerConfig.speedWalk;
                    footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);
                }
                else if (Mathf.Abs(_inputMovement.x) > _playerConfig.axisLimitCrouch || Mathf.Abs(_inputMovement.y) > _playerConfig.axisLimitCrouch)
                {
                    _speedHorizontal = _playerConfig.speedCrouchFast;
                    footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);
                }
                else
                {
                    _speedHorizontal = _playerConfig.speedCrouch;
                    footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);
                }
                break;

            case MOVEMENT_STATE.Jump:

                break;

            default:
                break;
        }

        // Ladder
        // if (_inIvy) { return; }

        // Jump
        if (_characterController.isGrounded)
        {
            _speedVertical = -1;

            // if (_isJumping)
            // {
            //     // _speedVertical = _jump;
            //     _isJumping = false;

            // }

            // Add movement
            _inputMovementAux = _inputMovement.normalized;

            _movement.x = (_inputMovement.x != 0 ? _inputMovementAux.x : 0) * _speedHorizontal;
            _movement.z = (_inputMovement.y != 0 ? _inputMovementAux.y : 0) * _speedHorizontal;
            _movement = Vector3.ClampMagnitude(_movement, _speedHorizontal);

            _animatorController.Falling(false);
        }
        else if (Mathf.Abs(_characterController.velocity.y) > _playerConfig.magnitudeFall)
        {
            _animatorController.Falling(true);
        }

        // Move
        _speedVertical -= _playerConfig.gravity * Time.deltaTime;
        _movement.y = _speedVertical;
        _characterController.Move(_movement * Time.deltaTime);

        // Animation       
        _animatorController.Movement(_movement, _movementState);

        //Sound
        _canPlayFootstep = _characterController.isGrounded && _characterController.velocity.magnitude != 0;
        footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
    }

    private void StopMovement()
    {
        _inputMovement = Vector2.zero;
        _inputMovementAux = _inputMovement;
        _movement = Vector3.zero;

        _animatorController.Walk(true);
        _animatorController.Movement(_movement, _movementState);
    }

    private void Interaction()
    {
        _isInteracting = !_isInteracting;

        _interactionEvent.isStart = _isInteracting;
        _interactionEvent.lastPlayerPosition = transform.position;
        EventController.TriggerEvent(_interactionEvent);
    }

    private void Run(bool active)
    {
        if (_characterController.isGrounded && active)
        {
            Crouch(true);
            ChangeMovementState(MOVEMENT_STATE.Run);
        }
        else
        {
            ChangeMovementState();
        }
    }

    private void Crouch(bool cancel = false)
    {
        if (cancel)_isCrouching = true;

        _isCrouching = !_isCrouching;

        if (_characterController.isGrounded && _isCrouching)
        {
            ChangeMovementState(MOVEMENT_STATE.Crouch);

            _characterController.height = _playerConfig.height / 2;
            _characterController.center = _crouchHeight;
        }
        else
        {
            ChangeMovementState();

            _characterController.height = _playerConfig.height;
            _characterController.center = Vector3.zero;
        }
    }

    // private void Jump()
    // {
    //     if (_characterController.isGrounded)
    //     {
    //         ChangeMovementState(MOVEMENT_STATE.Jump);

    //         _isJumping = true;

    //         // if !canMove drop 4 raycast 
    //         if (_isJumping)
    //         {

    //             if (Physics.Raycast(wallCheck.transform.position, Vector3.right, out RaycastHit hitWallFront, wallCheckDistance, _playerConfig.layerClimbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.right, out RaycastHit hitLedgeFront, wallCheckDistance, _playerConfig.layerClimbable)
    //                 /*Physics.Raycast(wallCheck.transform.position, Vector3.forward, out RaycastHit hitWallLeft, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.forward, out RaycastHit hitLedgeLeft, wallCheckDistance, Climbable)*/
    //             )
    //             {
    //                 if (hitWallFront.collider.CompareTag(Tags.Climbable) && hitLedgeFront.collider.CompareTag(Tags.Climbable) && !ledgeDetected)
    //                 {
    //                     SetNewPosition(transform.position.x + .5f, hitLedgeFront.collider.bounds.size.y, transform.position.z);

    //                     newPos = new Vector3(.7f + hitLedgeFront.transform.position.x - hitLedgeFront.collider.bounds.size.x / hitLedgeFront.transform.position.x - hitLedgeFront.collider.bounds.size.x / 2,
    //                         hitLedgeFront.transform.position.y + hitLedgeFront.collider.bounds.size.y / hitLedgeFront.transform.position.y + hitLedgeFront.collider.bounds.size.y / 2,
    //                         hitLedgeFront.transform.position.z);

    //                     Debug.Log($"<b> {newPos} </b>");

    //                     ledgeDetected = true;

    //                     StartClimb();

    //                     FMODUnity.RuntimeManager.PlayOneShot(_fmodConfig.climbing, transform.position);
    //                 }

    //                 //     if (hitWallLeft.collider.tag == "Climbable" && hitLedgeLeft.collider.tag == "Climbable" && !ledgeDetected)
    //                 //     {
    //                 //         _inLedge = true;

    //                 //         newPos = hitLedgeFront.collider.gameObject.transform.GetChild(0).transform.position;

    //                 //         ledgeDetected = true;

    //                 //         StartCoroutine(AnimClimb());

    //                 //     }
    //                 // }
    //             }

    //             else
    //             {
    //                 if (Physics.Raycast(wallCheck.transform.position, Vector3.left, out RaycastHit hitWallBack, wallCheckDistance, _playerConfig.layerClimbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.left, out RaycastHit hitLedgeBack, wallCheckDistance, _playerConfig.layerClimbable)
    //                     /*Physics.Raycast(wallCheck.transform.position, Vector3.back, out RaycastHit hitWallRight, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.back, out RaycastHit hitLedgeRight, wallCheckDistance, Climbable)*/
    //                 )
    //                 {
    //                     if (hitWallBack.collider.CompareTag(Tags.Climbable) && hitLedgeBack.collider.CompareTag(Tags.Climbable) && !ledgeDetected)
    //                     {
    //                         SetNewPosition(transform.position.x - .5f, hitLedgeBack.collider.bounds.size.y + .5f, transform.position.z);

    //                         newPos = new Vector3(-.7f + hitLedgeBack.transform.position.x + hitLedgeBack.collider.bounds.size.x / hitLedgeBack.transform.position.x + hitLedgeBack.collider.bounds.size.x / 2,
    //                             hitLedgeBack.transform.position.y + hitLedgeBack.collider.bounds.size.y / hitLedgeBack.transform.position.y + hitLedgeBack.collider.bounds.size.y / 2,
    //                             hitLedgeBack.transform.position.z);

    //                         ledgeDetected = true;

    //                         StartClimb();
    //                     }

    //                     // if (hitWallRight.collider.tag == "Climbable" && hitLedgeRight.collider.tag == "Climbable" && !ledgeDetected)
    //                     // {
    //                     //     _inLedge = true;

    //                     //     newPos = hitLedgeBack.collider.gameObject.transform.GetChild(0).transform.position;

    //                     //     ledgeDetected = true;

    //                     //     StartCoroutine(AnimClimb());

    //                     // }
    //                 }
    //             }
    //         }
    //     }
    // }

    // private void IvyMovement()
    // {
    //     DetectBot();

    //     if (!_inIvy)
    //     {
    //         return;
    //     }

    //     _inIvy = true;

    //     _movement.x = _inputMovement.x * _speedIvy;
    //     _movement.z = 0;
    //     _movement.y = _inputMovement.y * _speedIvy;
    //     _characterController.Move(_movement * Time.deltaTime);

    //     _animatorController.Movement(_movement, _isRunning, _characterController.isGrounded);
    // }

    // private void DetectBot()
    // {
    //     if (_characterController.isGrounded && !_inIvy)
    //     {
    //         _animatorController.PreClimbLadder(false);
    //     }

    //     else
    //     {
    //         _animatorController.PreClimbLadder(true);
    //     }

    //     // _botPosition = new Vector3(
    //     //     transform.position.x,
    //     //     transform.position.y - _characterController.height / 2 - _characterController.center.y,
    //     //     transform.position.z);

    //     // if (Physics.Raycast(_botPosition, Vector3.down, out _hitBot, .1f))
    //     // {
    //     //     if (_hitBot.collider.tag == "Interaction")
    //     //     {
    //     //         _inLadder = false;
    //     //         _ladderEvent.ladderExit = LADDER_EXIT.Bot;
    //     //         EventController.TriggerEvent(_ladderEvent);
    //     //     }
    //     // }
    // }

    private void ChangeMovementState()
    {
        _movementState = _lastMovementState;
    }

    private void ChangeMovementState(MOVEMENT_STATE newState)
    {
        _lastMovementState = _movementState;
        _movementState = newState;
    }

    public void SetPlayerData(PlayerSO data)
    {
        _playerData = data;

        gameObject.name = string.Format("[Player] {0}", _playerData.name);
        GetComponent<SpriteRenderer>().sprite = _playerData.Sprite;
        GetComponent<Animator>().runtimeAnimatorController = _playerData.AnimatorController;
    }

    public void SwitchMovement()
    {
        _canMove = !_canMove;
    }

    public void SwitchLadderMovement(bool inLadder)
    {
        _inIvy = inLadder;
    }

    public void SetNewPosition(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    public bool GetPlayerInMovement()
    {
        return _characterController.isGrounded && _canMove && !_isJumping && !_inIvy && _movement.magnitude > 0.1f;
    }

    #region FMOD

    public void FMODPlayFootstep()
    {
        if (!_canPlayFootstep)return;

        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 2, _playerConfig.layerGround))
        {
            switch (_hit.collider.gameObject.tag)
            {
                case Tags.Ground_Grass:
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 2);
                    break;

                case Tags.Ground_Dirt:
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;

                case Tags.Ground_Wood:
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                case Tags.Ground_Cement:
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 0);
                    break;

                case Tags.Ground_Ceramic:
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                default:
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;
            }
        }

        footstepSound.Play();
    }

    #endregion

    #region Events

    private void OnStopMovement(EnableMovementEvent evt)
    {
        _canMove = evt.canMove;

        if (_canMove)
        {
            _input.Player.Enable();
            _characterController.enabled = true;
        }
        else
        {
            _input.Player.Disable();
            _characterController.enabled = false;
            StopMovement();
        }

        if (evt.canMove)
        {
            _isInteracting = false;

            // Crouch(true);
        }
    }

    private void OnChangePosition(ChangePositionEvent evt)
    {
        transform.position = evt.newPosition;
    }

    private void OnCurrentInteraction(CurrentInteractEvent evt)
    {
        _currentInteraction = evt.currentInteraction;
    }

    private void OnDeviceChange(DeviceChangeEvent evt)
    {
        InputUtility.DeviceRebind(evt.device, _input);
    }

    #endregion

}