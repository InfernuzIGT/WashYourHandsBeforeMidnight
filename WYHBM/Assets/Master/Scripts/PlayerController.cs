using System.Collections;
using System.Collections.Generic;
using Events;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
// using UnityEngine.VFX;
// using UnityEngine.VFX.Utility;

[RequireComponent(typeof(CharacterController), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField, ReadOnly] private PlayerSO _playerData = null;
    [SerializeField, ReadOnly] private MOVEMENT_STATE _movementState = MOVEMENT_STATE.Walk;

    [Header("FMOD")]
    public StudioEventEmitter footstepSound;
    public StudioEventEmitter breathingSound;
    public StudioEventEmitter crouchSound;
    public StudioEventEmitter standSound;
    public StudioEventEmitter fallSound;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private PlayerConfig _playerConfig = null;
    [SerializeField, ConditionalHide] private WorldConfig _worldConfig = null;
    [SerializeField, ConditionalHide] private DeviceConfig _deviceConfig = null;
    // [SerializeField] private FMODConfig _fmodConfig = null;
    [Space]
    // [SerializeField, ConditionalHide] private MeshRenderer _shadow = null;
    // [SerializeField, ConditionalHide] private VisualEffect _vfxFootstep = null;
    [SerializeField, ConditionalHide] private CharacterController _characterController = null;
    [SerializeField, ConditionalHide] private WorldAnimator _animatorController = null;

    // Events
    private InteractionEvent _interactionEvent;
    private LadderEvent _ladderEvent;

    // Movement 
    private MOVEMENT_STATE _lastMovementState;
    private Vector2 _inputMovement;
    private Vector2 _inputMovementAux;
    private Vector3 _movement;
    private bool _canMove = true;
    private float _speedHorizontal;
    private float _speedVertical;
    private bool _isInteracting;

    // Crouch
    private Vector3 _crouchHeight;
    private bool _isCrouching;

    // Footstep
    private RaycastHit _hitFootsep;
    private Collider[] _targetsInSoundRadius;
    private float _currentSoundRadius;
    private Vector3 _groundPosition;
    private NPCController _currentNPC;
    // private bool _canPlayVFXFootstep;
    // private ExposedProperty hash_FootstepTexture = "VFX Texture";

    //Jump
    // private float _jump = 9.81f;
    private bool _isJumping;
    private bool _isClimbing;
    private RaycastHit _hitJump;
    private Vector3 _jumpDirection;

    // Ivy
    // private float _speedIvy = 5f;
    private bool _inIvy = false;
    private RaycastHit _hitBot;
    private Vector3 _botPosition;
    private bool _isFalling;

    // Ledge
    private Vector3 newPos;

    private Vector3 _lastPosition;

    // Quest
    private bool _isOpenDiary;

    public UnityAction cancelListerMode;

    // Properties
    private CustomInputAction _input;
    public CustomInputAction Input { get { return _input; } set { _input = value; } }

    private bool _canPlayFootstep;
    public bool CanPlayFootstep { get { return _canPlayFootstep; } }

    private bool _isDetectingGround;
    public bool IsDetectingGround { get { return _isDetectingGround; } set { _isDetectingGround = value; } }

    private Interaction _currentInteraction = null;
    public Interaction CurrentInteraction { get { return _currentInteraction; } }

    public bool IsCrouching { get { return _isCrouching; } }
    public PlayerSO PlayerData { get { return _playerData; } }

    private bool _devSilentSteps;
    public bool DevSilentSteps { get { return _devSilentSteps; } set { _devSilentSteps = value; } }

    Dictionary<int, QuestSO> questLog = new Dictionary<int, QuestSO>();

    private void Awake()
    {
        CreateInput();
    }

    private void CreateInput()
    {
        _input = new CustomInputAction();

        _input.Player.Move.performed += ctx => _inputMovement = ctx.ReadValue<Vector2>();
        _input.Player.Jump.performed += ctx => Jump();
        _input.Player.Interaction.performed += ctx => Interaction();
        _input.Player.Crouch.performed += ctx => Crouch();
        // _input.Player.Run.started += ctx => Run(true);
        // _input.Player.Run.canceled += ctx => Run(false);

        // _input.Player.DebugMode.performed += ctx => GameData.Instance.SelectNextQuality(true);

        _input.Player.Enable();
        _input.UI.Disable();

        _deviceConfig.UpdateDictionary();
    }

    public void SetInput(UnityAction actionPause, UnityAction actionOptions)
    {
        _input.Player.Pause.performed += ctx => actionPause.Invoke();
        _input.Player.Options.performed += ctx => actionOptions.Invoke();
    }

    private void Start()
    {
        _interactionEvent = new InteractionEvent();
        _ladderEvent = new LadderEvent();

        _crouchHeight = new Vector3(0, -(_playerConfig.height / 2) / 2, 0);
        _jumpDirection = new Vector3(0, 0, 0);

        GameData.Instance.DetectDevice();
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

    private void Update()
    {
        Movement();
    }

    // private void OnGUI()
    // {
    //     GUIStyle guiStyle = new GUIStyle();
    //     guiStyle.fontSize = 50;
    //     GUI.Label(new Rect(10, 10, 100, 20), string.Format("Velocity: {0}", _characterController.velocity.magnitude), guiStyle);
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
                _currentSoundRadius = _playerConfig.soundRadiusJogging;
                _speedHorizontal = _playerConfig.speedJogging;

                footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);

                _animatorController.Walk(false);

                // _canPlayVFXFootstep = true;

                // TODO Mariano: Enable
                // if (Mathf.Abs(_inputMovement.x) > _playerConfig.axisLimit || Mathf.Abs(_inputMovement.y) > _playerConfig.axisLimit)
                // {
                // _currentSoundRadius = _playerConfig.soundRadiusJogging;
                //     _speedHorizontal = _playerConfig.speedJogging;

                //     footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);

                //     _animatorController.Walk(false);
                // }
                // else
                // {
                // _currentSoundRadius = _playerConfig.soundRadiusWalk;
                //     _speedHorizontal = _playerConfig.speedWalk;

                //     footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);

                //     _animatorController.Walk(true);
                // }
                break;

            case MOVEMENT_STATE.Run:
                _currentSoundRadius = _playerConfig.soundRadiusRun;
                _speedHorizontal = _playerConfig.speedRun;

                // _canPlayVFXFootstep = true;

                // footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
                break;

            case MOVEMENT_STATE.Crouch:
                _currentSoundRadius = _playerConfig.soundRadiusCrouch;

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

                // _canPlayVFXFootstep = false;

                break;

            case MOVEMENT_STATE.ClimbShort:
            case MOVEMENT_STATE.ClimbLarge:
                _speedHorizontal = 0;

                break;

            default:
                break;
        }

        if (_isClimbing)return;

        if (_characterController.isGrounded)
        {
            _speedVertical = -1;

            _inputMovementAux = _inputMovement.normalized;

            _jumpDirection.x = _inputMovementAux.x;
            _jumpDirection.z = _inputMovementAux.y;

            _movement.x = (_inputMovement.x != 0 ? _inputMovementAux.x : 0) * _speedHorizontal;
            _movement.z = (_inputMovement.y != 0 ? _inputMovementAux.y : 0) * _speedHorizontal;
            _movement = Vector3.ClampMagnitude(_movement, _speedHorizontal);

            _animatorController.Falling(false);

            if (_isFalling)
            {
                // TODO Mariano: Si pasa X tiempo cayendo, cambiar animacion/sonido/ejecutar animacion land
                _isFalling = false;
                fallSound.Play();
            }
        }
        else if (Mathf.Abs(_characterController.velocity.y) > _playerConfig.magnitudeFall)
        {
            _animatorController.Falling(true);

            _isFalling = true;
        }

        // Move
        _speedVertical -= _playerConfig.gravity * Time.deltaTime;
        _movement.y = _speedVertical;
        _characterController.Move(_movement * Time.deltaTime);

        // Animation       
        // _animatorController.Movement(_inputMovementAux, _movementState);
        _animatorController.Movement(_movement.x, _movement.z, _movementState);

        //Sound
        _canPlayFootstep = _characterController.isGrounded && _characterController.velocity.magnitude != 0;
        // footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);
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

    public void Crouch(bool cancel = false)
    {
        cancelListerMode.Invoke();

        if (cancel)_isCrouching = true;

        _isCrouching = !_isCrouching;

        if (_characterController.isGrounded && _isCrouching)
        {
            crouchSound.Play();

            //footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 0);

            ChangeMovementState(MOVEMENT_STATE.Crouch);

            _characterController.height = _playerConfig.height / 2;
            _characterController.center = _crouchHeight;
        }
        else
        {
            standSound.Play();

            //footstepSound.EventInstance.setParameterByName(FMODParameters.Sprint, 1);

            ChangeMovementState();

            _characterController.height = _playerConfig.height;
            _characterController.center = Vector3.zero;
        }

    }

    public void Footstep()
    {
        if (!_canPlayFootstep)return;

        FootstepDetection();
        FootstepSound();
    }

    private void FootstepDetection()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hitFootsep, 3, _worldConfig.layerGround))
        {
            // _shadow.enabled = true;

            _groundPosition = new Vector3(transform.position.x, _hitFootsep.point.y + 0.1f, transform.position.z - 1);

            // _shadow.transform.position = _groundPosition;

            switch (_hitFootsep.collider.gameObject.tag)
            {
                case Tags.Ground_Grass:
                    // _vfxFootstep.SetTexture(hash_FootstepTexture, _worldConfig.textureGrass);
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 2);
                    break;

                case Tags.Ground_Dirt:
                    // _vfxFootstep.SetTexture(hash_FootstepTexture, _worldConfig.textureDirt);
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;

                case Tags.Ground_Wood:
                    // _vfxFootstep.SetTexture(hash_FootstepTexture, _worldConfig.textureWood);
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                case Tags.Ground_Cement:
                    // _vfxFootstep.SetTexture(hash_FootstepTexture, _worldConfig.textureCement);
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 0);
                    break;

                case Tags.Ground_Ceramic:
                    // _vfxFootstep.SetTexture(hash_FootstepTexture, _worldConfig.textureCeramic);
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                default:
                    // _vfxFootstep.SetTexture(hash_FootstepTexture, _worldConfig.textureDefault);
                    footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;
            }
        }
        // else
        // {
        // _shadow.enabled = false;
        // }

        footstepSound.Play();

        // if (_canPlayVFXFootstep)_vfxFootstep.Play();
    }

    private void FootstepSound()
    {
        if (_devSilentSteps)return;

        _targetsInSoundRadius = Physics.OverlapSphere(_groundPosition, _currentSoundRadius, _worldConfig.layerNPC);

        for (int i = 0; i < _targetsInSoundRadius.Length; i++)
        {
            _currentNPC = _targetsInSoundRadius[i].GetComponent<NPCController>();

            if (_currentNPC != null)_currentNPC.SetDestination(transform.position);
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(_groundPosition, _currentSoundRadius);
    // }

    private void Jump()
    {
        if (_characterController.isGrounded && !_isJumping)
        {
            _isJumping = true;

            if (Physics.Raycast(transform.position, _jumpDirection, out _hitJump, 1))
            {
                if ((_hitJump.collider.bounds.max.y - _characterController.bounds.size.y) < _playerConfig.height)
                {
                    _isClimbing = true;

                    _characterController.enabled = false;

                    transform.position += new Vector3(0, _hitJump.collider.bounds.max.y - 0.5f, 0) + _jumpDirection * 2;

                    // TODO Mariano: Implement animation
                    // if ((_hitJump.collider.bounds.max.y - transform.position.y) < _playerConfig.height / 2)
                    // {
                    //     ChangeMovementState(MOVEMENT_STATE.ClimbShort);
                    // }
                    // else
                    // {
                    //     ChangeMovementState(MOVEMENT_STATE.ClimbLarge);
                    // }

                    // _animatorController.Movement(_jumpDirection.x, _jumpDirection.z, _movementState);

                    StartCoroutine(Climbing());
                }
            }
            else
            {
                _isJumping = false;
            }
        }
    }

    private IEnumerator Climbing()
    {
        // TODO Mariano: Use animation duration
        yield return new WaitForSeconds(0.5f);

        _characterController.enabled = true;
        ChangeMovementState();
        _isJumping = false;
        _isClimbing = false;
    }

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

            if (evt.isDetected)_animatorController.Detected(true);
        }

        if (evt.canMove)
        {
            _isInteracting = false;

            if (evt.isDetected)_animatorController.Detected(false);
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
        InputSystemAdapter.DeviceRebind(evt.device, _input);
    }

    #endregion

}