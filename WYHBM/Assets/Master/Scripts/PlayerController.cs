using System.Collections.Generic;
using Events;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(CharacterController), typeof(DeviceUtility))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, ReadOnly] private PlayerSO _playerData;

    [Header("Review")]
    public bool _inZipline = false;
    public Vector3 endPos;
    public LayerMask Climbable;
    public bool ledgeDetected;
    public Transform wallCheck;
    public Transform ledgeCheck;
    public float wallCheckDistance;

    [Header("FMOD")]
    public LayerMask layerMask;
    [Space]
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
    private Vector2 _inputMovement;
    private Vector2 _inputMovementAux;
    private Vector3 _movement;
    private float _speedRun = 15f;
    private bool _canMove = true;
    private bool _isRunning;
    private float _speedHorizontal;
    private float _speedVertical;
    private bool _isDetectingGround;
    private bool _interaction;

    // Walk
    private float _speedWalk = 3.5f;
    private bool _isWalking;

    //Jump
    // private float _jump = 9.81f;
    private float _gravity = 39.24f;
    private float _magnitudeFall = 20f;
    private bool _isJumping;

    // Ivy
    private float _speedIvy = 5f;
    private bool _inIvy = false;
    private RaycastHit _hitBot;
    private Vector3 _botPosition;

    // Zipline
    private float _speedZipline = .35f;

    // Ledge
    private Vector3 newPos;

    private Vector3 _lastPosition;
    private float _axisLimit = 0.7f;

    // Footstep
    private RaycastHit _hit;

    // Quest
    private bool _isOpenDiary;

    // Properties
    private InputActions _input;
    public InputActions Input { get { return _input; } set { _input = value; } }

    private bool _canPlayFootstep;
    public bool CanPlayFootstep { get { return _canPlayFootstep; } }

    public bool IsDetectingGround { get => _isDetectingGround; set => _isDetectingGround = value; }
    public PlayerSO PlayerData { get { return _playerData; } }

    Dictionary<int, QuestSO> questLog = new Dictionary<int, QuestSO>();

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animatorController = GetComponent<WorldAnimator>();
        _deviceUtility = GetComponent<DeviceUtility>();

        CreateInput();
    }

    private void CreateInput()
    {
        _input = new InputActions();

        _input.Player.Move.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        _input.Player.Jump.started += ctx => Jump();
        _input.Player.Interaction.performed += ctx => Interaction();
        _input.Player.Pause.performed += ctx => Pause(PAUSE_TYPE.PauseMenu);
        _input.Player.Options.performed += ctx => Pause(PAUSE_TYPE.Inventory);

        _input.Player.Enable();
        _input.UI.Disable();
    }

    private void Start()
    {
        _interactionEvent = new InteractionEvent();
        _ladderEvent = new LadderEvent();
        _pauseEvent = new PauseEvent();

        _deviceUtility.DetectDevice();
    }

    private void OnEnable()
    {
        EventController.AddListener<EnableMovementEvent>(OnStopMovement);
        EventController.AddListener<ChangePlayerPositionEvent>(OnChangePlayerPosition);
        EventController.AddListener<DeviceChangeEvent>(OnDeviceChange);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        EventController.RemoveListener<ChangePlayerPositionEvent>(OnChangePlayerPosition);
        EventController.RemoveListener<DeviceChangeEvent>(OnDeviceChange);
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
        // MovementZipline();
    }

    // private void MovementZipline()
    // {
    //     if (_inZipline)
    //     {
    //         _animatorController.MovementZipline(true);

    //         _gravity = 0;

    //         transform.position = Vector3.MoveTowards(transform.position, endPos, _speedZipline);

    //         if (Vector3.Distance(transform.position, endPos) < 1)
    //         {
    //             _animatorController.MovementZipline(false);
    //             _inZipline = false;
    //             _gravity = 39.24f;
    //         }
    //     }
    // }

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

    private void Movement()
    {
        // Stop animation
        if (!_canMove)
        {
            StopMovement();
            return;
        }

        // Ladder
        if (_inIvy) { return; }

        // Run / Walk
        if (CheckRun())
        {
            _isRunning = _inputMovement.x == 0 && _inputMovement.y == 0 || !_characterController.isGrounded ? false : true;
            _speedHorizontal = _speedRun;
            footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
        }

        else
        {
            _isRunning = false;
            _speedHorizontal = _speedWalk;
            footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);
        }

        // Jump
        if (_characterController.isGrounded)
        {
            _speedVertical = -1;

            if (_isJumping)
            {
                // _speedVertical = _jump;
                _isJumping = false;

            }

            // Add movement
            _inputMovementAux = _inputMovement.normalized;
            _movement.x = (_inputMovement.x != 0 ? _inputMovementAux.x : 0) * _speedHorizontal;
            _movement.z = (_inputMovement.y != 0 ? _inputMovementAux.y : 0) * _speedHorizontal;
            _movement = Vector3.ClampMagnitude(_movement, _speedHorizontal);

            _animatorController.Falling(false);
        }

        if (_characterController.velocity.magnitude > _magnitudeFall && !_characterController.isGrounded && !_inIvy)
        {
            _animatorController.Falling(true);
        }

        // Move
        _speedVertical -= _gravity * Time.deltaTime;
        _movement.y = _speedVertical;
        _characterController.Move(_movement * Time.deltaTime);

        // Animation       
        _animatorController.Movement(_movement, _isRunning, true);

        //Sound
        _canPlayFootstep = _characterController.isGrounded && _characterController.velocity.magnitude != 0;
        footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
    }

    private void StopMovement()
    {
        _inputMovement = Vector2.zero;
        _inputMovementAux = _inputMovement;

        _isRunning = false;
        _animatorController.Movement(Vector3.zero, _isRunning, _characterController.isGrounded);
    }

    private bool CheckRun()
    {
        return !_isWalking && Mathf.Abs(_inputMovement.x) > _axisLimit || !_isWalking && Mathf.Abs(_inputMovement.y) > _axisLimit;
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _isJumping = true;

            // if !canMove drop 4 raycast 
            if (_isJumping)
            {

                if (Physics.Raycast(wallCheck.transform.position, Vector3.right, out RaycastHit hitWallFront, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.right, out RaycastHit hitLedgeFront, wallCheckDistance, Climbable)
                    /*Physics.Raycast(wallCheck.transform.position, Vector3.forward, out RaycastHit hitWallLeft, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.forward, out RaycastHit hitLedgeLeft, wallCheckDistance, Climbable)*/
                )
                {
                    if (hitWallFront.collider.tag == "Climbable" && hitLedgeFront.collider.tag == "Climbable" && !ledgeDetected)
                    {
                        SetNewPosition(transform.position.x + .5f, hitLedgeFront.collider.bounds.size.y, transform.position.z);

                        newPos = new Vector3(.7f + hitLedgeFront.transform.position.x - hitLedgeFront.collider.bounds.size.x / hitLedgeFront.transform.position.x - hitLedgeFront.collider.bounds.size.x / 2,
                            hitLedgeFront.transform.position.y + hitLedgeFront.collider.bounds.size.y / hitLedgeFront.transform.position.y + hitLedgeFront.collider.bounds.size.y / 2,
                            hitLedgeFront.transform.position.z);

                        Debug.Log($"<b> {newPos} </b>");

                        ledgeDetected = true;

                        StartClimb();

                        FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Climbing", GetComponent<Transform>().position);
                    }

                    //     if (hitWallLeft.collider.tag == "Climbable" && hitLedgeLeft.collider.tag == "Climbable" && !ledgeDetected)
                    //     {
                    //         _inLedge = true;

                    //         newPos = hitLedgeFront.collider.gameObject.transform.GetChild(0).transform.position;

                    //         ledgeDetected = true;

                    //         StartCoroutine(AnimClimb());

                    //     }
                    // }
                }

                else
                {
                    if (Physics.Raycast(wallCheck.transform.position, Vector3.left, out RaycastHit hitWallBack, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.left, out RaycastHit hitLedgeBack, wallCheckDistance, Climbable)
                        /*Physics.Raycast(wallCheck.transform.position, Vector3.back, out RaycastHit hitWallRight, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.back, out RaycastHit hitLedgeRight, wallCheckDistance, Climbable)*/
                    )
                    {
                        if (hitWallBack.collider.tag == "Climbable" && hitLedgeBack.collider.tag == "Climbable" && !ledgeDetected)
                        {
                            SetNewPosition(transform.position.x - .5f, hitLedgeBack.collider.bounds.size.y + .5f, transform.position.z);

                            newPos = new Vector3(-.7f + hitLedgeBack.transform.position.x + hitLedgeBack.collider.bounds.size.x / hitLedgeBack.transform.position.x + hitLedgeBack.collider.bounds.size.x / 2,
                                hitLedgeBack.transform.position.y + hitLedgeBack.collider.bounds.size.y / hitLedgeBack.transform.position.y + hitLedgeBack.collider.bounds.size.y / 2,
                                hitLedgeBack.transform.position.z);

                            ledgeDetected = true;

                            StartClimb();
                        }

                        // if (hitWallRight.collider.tag == "Climbable" && hitLedgeRight.collider.tag == "Climbable" && !ledgeDetected)
                        // {
                        //     _inLedge = true;

                        //     newPos = hitLedgeBack.collider.gameObject.transform.GetChild(0).transform.position;

                        //     ledgeDetected = true;

                        //     StartCoroutine(AnimClimb());

                        // }
                    }
                }
            }
        }
    }

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

    private void DetectBot()
    {
        if (_characterController.isGrounded && !_inIvy)
        {
            _animatorController.PreClimbLadder(false);
        }

        else
        {
            _animatorController.PreClimbLadder(true);
        }

        // _botPosition = new Vector3(
        //     transform.position.x,
        //     transform.position.y - _characterController.height / 2 - _characterController.center.y,
        //     transform.position.z);

        // if (Physics.Raycast(_botPosition, Vector3.down, out _hitBot, .1f))
        // {
        //     if (_hitBot.collider.tag == "Interaction")
        //     {
        //         _inLadder = false;
        //         _ladderEvent.ladderExit = LADDER_EXIT.Bot;
        //         EventController.TriggerEvent(_ladderEvent);
        //     }
        // }
    }

    private void Interaction()
    {
        _interaction = !_interaction;

        _interactionEvent.isStart = _interaction;
        _interactionEvent.lastPlayerPosition = transform.position;
        _interactionEvent.isRunning = _isRunning;
        EventController.TriggerEvent(_interactionEvent);
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

        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 2, layerMask))
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

        if (evt.canMove)_interaction = false;
    }

    private void OnChangePlayerPosition(ChangePlayerPositionEvent evt)
    {
        transform.position = evt.newPosition;
    }

    private void OnDeviceChange(DeviceChangeEvent evt)
    {
        InputUtility.DeviceRebind(evt.device, _input);
    }

    #endregion

}