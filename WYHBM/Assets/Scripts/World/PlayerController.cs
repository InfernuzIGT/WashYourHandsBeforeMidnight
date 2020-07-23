using System.Collections;
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

    // Zipline
    public bool _inZipline = false;
    // public float speedZipline; 
    public Vector3 endPos;

    // Ledge
    public Animator animator;
    public LayerMask Climbable;
    public bool ledgeDetected;
    private bool _isClimbing;
    private bool _inLedge;
    private Vector3 newPos;

    public Transform wallCheck;
    public Transform ledgeCheck;
    public float wallCheckDistance;

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

        InputActions.Player.Move.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        InputActions.Player.Jump.performed += ctx => Jump();
        InputActions.Player.Interaction.performed += ctx => Interaction();
        InputActions.Player.Walk.started += ctx => Walk(true);
        InputActions.Player.Walk.canceled += ctx => Walk(false);
        InputActions.Player.Pause.performed += ctx => GameManager.Instance.Pause();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _interactionEvent = new InteractionEvent();
        _ladderEvent = new LadderEvent();

        ToggleInputWorld(true);
    }

    private void OnEnable()
    {
        EventController.AddListener<EnableMovementEvent>(OnStopMovement);
        EventController.AddListener<ChangePlayerPositionEvent>(OnChangePlayerPosition);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        EventController.RemoveListener<ChangePlayerPositionEvent>(OnChangePlayerPosition);
    }

    public void ToggleInputWorld(bool isEnabled)
    {
        if (isEnabled)
        {
            InputActions.Enable();
        }
        else
        {
            InputActions.Disable();
        }
    }

    private void Update()
    {
        Movement();
        LadderMovement();
        MovementZipline();
    }


    private void MovementZipline()
    {
        if (_inZipline)
        {
            _gravity = 0;

            _animatorController.MovementZipline(true);
            // animator.SetBool("canZipline", true);

            transform.position = Vector3.MoveTowards(transform.position, endPos, .5f);

            if (Vector3.Distance(transform.position, endPos) < 1)
            {
                _animatorController.MovementZipline(false);
                // animator.SetBool("canZipline", false);
                _inZipline = false;
                _gravity = 39.24f;
            }
        }
    }

    private IEnumerator AnimClimb()
    {
        if (_inLadder)
        {
            animator.SetBool("canClimbLadder", true);
        }

        if (_inLedge)
        {
            _characterController.enabled = false;

            animator.SetBool("canClimbLedge", true);

            yield return new WaitForSeconds(.01f);

            animator.SetBool("canClimbLedge", false);

            yield return new WaitForSeconds(1.35f);

            EndClimb();
        }

    }

    private void EndClimb()
    {
        _inLedge = false;

        _isClimbing = false;
        ledgeDetected = false;

        transform.position = newPos;

        _characterController.enabled = true;
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
                // _speedVertical = _jump;
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

            // if !canMove drop 4 raycast 
            if (_isJumping)
            {

                if (Physics.Raycast(wallCheck.transform.position, Vector3.right, out RaycastHit hitWallFront, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.right, out RaycastHit hitLedgeFront, wallCheckDistance, Climbable)
                    /*Physics.Raycast(wallCheck.transform.position, Vector3.forward, out RaycastHit hitWallLeft, wallCheckDistance, Climbable) && Physics.Raycast(ledgeCheck.transform.position, Vector3.forward, out RaycastHit hitLedgeLeft, wallCheckDistance, Climbable)*/
                )
                {
                    if (hitWallFront.collider.tag == "Climbable" && hitLedgeFront.collider.tag == "Climbable" && !ledgeDetected)
                    {
                        _inLedge = true;

                        transform.position = new Vector3(transform.position.x, hitLedgeFront.collider.bounds.size.y, transform.position.z);

                        newPos = new Vector3(.5f + hitLedgeFront.transform.position.x - hitLedgeFront.collider.bounds.size.x / hitLedgeFront.transform.position.x - hitLedgeFront.collider.bounds.size.x / 2, 
                        hitLedgeFront.transform.position.y + hitLedgeFront.collider.bounds.size.y / hitLedgeFront.transform.position.y + hitLedgeFront.collider.bounds.size.y / 2, 
                        hitLedgeFront.transform.position.z);

                        Debug.Log ($"<b> {newPos} </b>");

                        ledgeDetected = true;

                        StartCoroutine(AnimClimb());

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
                            _inLedge = true;

                            transform.position = new Vector3(transform.position.x, hitLedgeBack.transform.position.y + 1, transform.position.z);

                            newPos = new Vector3(- .5f + hitLedgeBack.transform.position.x + hitLedgeBack.collider.bounds.size.x / hitLedgeBack.transform.position.x + hitLedgeBack.collider.bounds.size.x / 2, 
                            hitLedgeBack.transform.position.y + hitLedgeBack.collider.bounds.size.y / hitLedgeBack.transform.position.y + hitLedgeBack.collider.bounds.size.y / 2,
                            hitLedgeBack.transform.position.z);
                            // newPos = hitLedgeBack.collider.gameObject.transform.GetChild(0).transform.position;

                            ledgeDetected = true;

                            StartCoroutine(AnimClimb());

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

    private void LadderMovement()
    {
        if (!_inLadder)
        {
            animator.SetBool("canClimbLadder", false);
            return;
        }
        else
        {
            DetectBot();
        }

        _movement.x = _inputMovement.x * _speedLadder;
        _movement.z = 0;
        _movement.y = _inputMovement.y * _speedLadder;
        _characterController.Move(_movement * Time.deltaTime);

        _inLadder = true;
        StartCoroutine(AnimClimb());

        _animatorController.Movement(_movement, _isRunning, _characterController.isGrounded);

    }

    private void DetectBot()
    {
        if (_inLadder)
        {
            _botPosition = new Vector3(
                transform.position.x,
                transform.position.y - _characterController.height / 2 - _characterController.center.y,
                transform.position.z);

            if (Physics.Raycast(_botPosition, Vector3.down, out _hitBot, .1f))
            {
                if (_hitBot.collider.tag == "Interaction")
                {
                    _inLadder = false;
                    _ladderEvent.ladderExit = LADDER_EXIT.Bot;
                    EventController.TriggerEvent(_ladderEvent);
                }
            }
        }
    }

    private void Interaction()
    {
        if (!GameManager.Instance.inCombat)
        {
            _interactionEvent.lastPlayerPosition = transform.position;
            _interactionEvent.isRunning = _isRunning;
            EventController.TriggerEvent(_interactionEvent);
        }
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

    public bool GetPlayerInMovement()
    {
        return _characterController.isGrounded && _canMove && !_inLadder && _movement.magnitude > 0.1f;
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