using System.Collections.Generic;
using Events;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
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
    private Vector2 _inputMovementAux;
    private Vector3 _movement;
    private float _speedRun = 15f;
    private bool _canMove = true;
    private bool _isRunning;
    private float _speedHorizontal;
    private float _speedVertical;

    // Walk
    private float _speedWalk = 7.5f;
    private bool _isWalking;

    //Jump
    private float _jump = 9.81f;
    private float _gravity = 39.24f;
    private bool _isJumping;

    // Ladder
    private float _speedLadder = 5f;
    private bool _inLadder = false;
    private RaycastHit _hitBot;
    private Vector3 _botPosition;

    private Vector3 _lastPosition;
    private float _axisLimit = 0.7f;

    // Quest
    private bool _isOpenDiary;

    // Properties
    private InputActions _inputActions;
    public InputActions InputActions { get { return _inputActions; } set { _inputActions = value; } }

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
        InputActions.ActionPlayer.Walk.started += ctx => Walk(true);
        InputActions.ActionPlayer.Walk.canceled += ctx => Walk(false);
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

        // Ladder
        if (_inLadder) { return; }

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
                _speedVertical = _jump;
                _isJumping = false;
            }

            // Add movement
            _inputMovementAux = _inputMovement.normalized;
            _movement.x = (_inputMovement.x != 0 ? _inputMovementAux.x : 0) * _speedHorizontal;
            _movement.z = (_inputMovement.y != 0 ? _inputMovementAux.y : 0) * _speedHorizontal;
            _movement = Vector3.ClampMagnitude(_movement, _speedHorizontal);
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

    private bool CheckRun()
    {
        return !_isWalking && Mathf.Abs(_inputMovement.x) > _axisLimit || !_isWalking && Mathf.Abs(_inputMovement.y) > _axisLimit;
    }

    private void Walk(bool isWalking)
    {
        _isWalking = isWalking;
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            _isJumping = true;
        }
    }

    private void LadderMovement()
    {
        if (!_inLadder) { return; }

        DetectBot();

        _movement.x = 0;
        _movement.z = 0;
        _movement.y = _inputMovement.y * _speedLadder;
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
    }

    private void OnChangePlayerPosition(ChangePlayerPositionEvent evt)
    {
        transform.position = evt.newPosition;
    }

    #endregion

}