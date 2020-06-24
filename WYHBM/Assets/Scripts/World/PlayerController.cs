using System.Collections.Generic;
using Events;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public QuestSO quest; // TODO Mariano: Review

    [Header("FMOD")]
    public StudioEventEmitter footstepSound;
    public StudioEventEmitter breathingSound;

    private CharacterController _characterController;
    private WorldAnimator _animatorController;

    private InteractionEvent _interactionEvent;
    private LadderEvent _ladderEvent;

    //Items
    public GameObject dropZone;

    // Movement 
    private Vector2 _inputMovement;
    private float _speedWalk = 7.5f;
    private float _speedRun = 15f;
    private bool _canMove = true;
    private bool _canJump = false;
    private bool _isRunning;
    private float _jump = 9.81f;
    private float _gravity = 29.43f;
    private Vector3 _movement;
    private float _speedHorizontal;
    private float _speedVertical;

    // Ladder
    private float _speedLadder = 5f;
    private bool _inLadder = false;
    private RaycastHit _hitBot;
    private Vector3 _botPosition;

    // Stamina
    // private float _stamina = 100;
    // private float _staminaMax = 100;
    // private float _staminaIncrease = 5f;
    // private float _staminaDecrease = 15;
    // private float _staminaRegenTimer = 0;
    // private float _staminaTimeToRegen = 3;

    private float _moveHorizontal;
    private float _moveVertical;
    private float _posX;
    private float _posZ;

    private Vector3 _lastPosition;

    // Quest
    private bool _isOpenDiary;

    // Properties
    private InputActions _inputActions;
    public InputActions InputActions { get { return _inputActions; } set { _inputActions = value; } }

    // private bool _infiniteStamina;
    // public bool InfiniteStamina { set { _infiniteStamina = value; } }

    private bool _canPlayFootstep;
    public bool CanPlayFootstep { get { return _canPlayFootstep; } }

    Dictionary<int, QuestSO> questLog = new Dictionary<int, QuestSO>();

    private void Awake()
    {
        CreateInput();

        _characterController = GetComponent<CharacterController>();
        _animatorController = GetComponent<WorldAnimator>();
    }

    private void CreateInput()
    {
        InputActions = new InputActions();

        InputActions.ActionPlayer.Move.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        InputActions.ActionPlayer.Jump.performed += ctx => Jump();
        InputActions.ActionPlayer.Interaction.performed += ctx => Interaction();
    }

    private void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        _interactionEvent = new InteractionEvent();
        _ladderEvent = new LadderEvent();
    }

    private void OnEnable()
    {
        InputActions.Enable();

        EventController.AddListener<EnableMovementEvent>(OnStopMovement);
        EventController.AddListener<ChangePlayerPositionEvent>(OnChangePlayerPosition);
    }

    private void OnDisable()
    {
        InputActions.Disable();

        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        EventController.RemoveListener<ChangePlayerPositionEvent>(OnChangePlayerPosition);
    }

    private void Update()
    {
        Movement();
        LadderMovement();
        // Stamina();
        // Interaction();

        _canPlayFootstep = _characterController.isGrounded && _characterController.velocity.magnitude != 0;
    }

    private void Movement()
    {
        // Stop animation
        if (!_canMove)
        {
            _isRunning = false;
            _animatorController.Movement(Vector3.zero, _isRunning, _characterController.isGrounded);
            return;
        }

        if (_inLadder) { return; }

        // Get input
        _moveHorizontal = _inputMovement.x;
        _moveVertical = _inputMovement.y;

        // Speed movement
        // if (Input.GetKey(KeyCode.LeftShift) && _stamina > 1 ||
        //     Input.GetKey(KeyCode.RightShift) && _stamina > 1)
        // {
        //     _isRunning = _moveHorizontal == 0 && _moveVertical == 0 || !_characterController.isGrounded ? false : true;
        //     _speedHorizontal = _speedRun;
        //     footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
        // }
        // else
        // {
        //     _isRunning = false;
        //     _speedHorizontal = _speedWalk;
        //     footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);
        // }

        // Jump
        if (_characterController.isGrounded)_speedVertical = -1;

        _isRunning = !_characterController.isGrounded ? false : true;
        _speedHorizontal = _speedRun;
        footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);

        // Add movement
        _movement.x = _moveHorizontal * _speedHorizontal;
        _movement.z = _moveVertical * _speedHorizontal;
        _movement = Vector3.ClampMagnitude(_movement, _speedHorizontal);

        // Move
        _speedVertical -= _gravity * Time.deltaTime;
        _movement.y = _speedVertical;
        _characterController.Move(_movement * Time.deltaTime);

        // Animation       
        _animatorController.Movement(_movement, _isRunning, true);
    }

    private void Jump()
    {
        // Stop animation
        if (!_canMove)
        {
            _isRunning = false;
            _animatorController.Movement(Vector3.zero, _isRunning, _characterController.isGrounded);
            return;
        }

        if (_inLadder) { return; }

        // Jump
        if (_canJump)_speedVertical = _jump;
    }

    private void LadderMovement()
    {
        if (!_inLadder) { return; }

        DetectBot();

        _moveVertical = _inputMovement.y;

        _movement.x = 0;
        _movement.z = 0;
        _movement.y = _moveVertical * _speedLadder;
        _characterController.Move(_movement * Time.deltaTime);

        // TODO Mariano: Add Animation
        // _animatorController.Movement(_movement, _isRunning, _characterController.isGrounded);
    }

    private void DetectBot()
    {
        _botPosition = new Vector3(
            transform.position.x,
            transform.position.y - _characterController.height / 2 - _characterController.center.y,
            transform.position.z);

        if (Physics.Raycast(_botPosition, Vector3.down, out _hitBot, 0.1f))
        {
            _inLadder = false;
            _ladderEvent.ladderExit = LADDER_EXIT.Bot;
            EventController.TriggerEvent(_ladderEvent);
        }
    }

    // private void Stamina()
    // {
    //     if (_isRunning && !_infiniteStamina || Input.GetKey(KeyCode.LeftShift) && _stamina < 1 && !_infiniteStamina)
    //     {
    //         _stamina = Mathf.Clamp(_stamina - (_staminaDecrease * Time.deltaTime), 0, _staminaMax);

    //         _staminaRegenTimer = 0;
    //     }
    //     else if (_stamina < _staminaMax)
    //     {
    //         if (_staminaRegenTimer >= _staminaTimeToRegen)
    //         {
    //             _stamina = Mathf.Clamp(_stamina + (_staminaIncrease * Time.deltaTime), 0, _staminaMax);
    //         }
    //         else
    //         {
    //             _staminaRegenTimer += Time.deltaTime;
    //         }
    //     }

    //     GameManager.Instance.worldUI.UpdateStamina(_stamina / _staminaMax);

    //     if (_stamina == 0f && !breathingSound.IsPlaying())
    //     {
    //         breathingSound.Play();
    //     }
    // }

    private void Interaction()
    {
        _interactionEvent.lastPlayerPosition = transform.position;
        _interactionEvent.isRunning = _isRunning;
        EventController.TriggerEvent(_interactionEvent);
    }

    public void SwitchMovement()
    {
        _canMove = !_canMove;
    }

    public void SwitchLadderMovement(bool inLadder)
    {
        _inLadder = inLadder;
    }

    public void SetNewPosition(float x, float y, float z)
    {
        transform.position = new Vector3(x, y, z);
    }

    #region FMOD

    public void FMODPlayFootstep()
    {
        footstepSound.Play();
    }

    #endregion

    #region Events

    private void OnStopMovement(EnableMovementEvent evt)
    {
        _canMove = evt.canMove;
        _canJump = !evt.enterToInterior;
    }

    private void OnChangePlayerPosition(ChangePlayerPositionEvent evt)
    {
        transform.position = evt.newPosition;
    }

    #endregion

}