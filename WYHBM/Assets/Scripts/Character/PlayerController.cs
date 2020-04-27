using Events;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Character
{
    private CharacterController _characterController;
    private AnimatorController _animatorController;

    private InteractionEvent _interactionEvent;

    // Movement 
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

    private bool _canPlayFootstep;
    public bool CanPlayFootstep { get { return _canPlayFootstep; } }

    // Stamina
    private float _stamina = 100;
    private float _staminaMax = 100;
    private float _staminaIncrease = 2.5f;
    private float _staminaDecrease = 20;
    private float _staminaRegenTimer = 0;
    private float _staminaTimeToRegen = 3;

    private float _moveHorizontal;
    private float _moveVertical;
    private float _posX;
    private float _posZ;

    private Vector3 _lastPosition;

    // Input
    private string _inputHorizontal = "Horizontal";
    private string _inputVertical = "Vertical";

    // Cheats
    private bool _infiniteStamina;
    public bool InfiniteStamina { set { _infiniteStamina = value; } }

    public QuestSO quest;

    Dictionary<int, QuestSO> questLog = new Dictionary<int, QuestSO>();

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animatorController = GetComponent<AnimatorController>();
    }

    private void Start()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        _interactionEvent = new InteractionEvent();
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

    private void Update()
    {
        CloseGame();

        if (InCombat)return;

        Movement();
        Stamina();
        Interaction();

        _canPlayFootstep = _characterController.isGrounded && _characterController.velocity.magnitude != 0;
    }


    private void CloseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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

        // Get input
        _moveHorizontal = Input.GetAxisRaw(_inputHorizontal);
        _moveVertical = Input.GetAxisRaw(_inputVertical);

        // Speed movement
        if (Input.GetKey(KeyCode.LeftShift) && _stamina > 1)
        {
            _isRunning = _moveHorizontal == 0 && _moveVertical == 0 || !_characterController.isGrounded ? false : true;
            _speedHorizontal = _speedRun;
        }
        else
        {
            _isRunning = false;
            _speedHorizontal = _speedWalk;
        }

        // Add movement
        _movement.x = _moveHorizontal * _speedHorizontal;
        _movement.z = _moveVertical * _speedHorizontal;
        _movement = Vector3.ClampMagnitude(_movement, _speedHorizontal);

        // Jump
        if (_characterController.isGrounded)
        {
            _speedVertical = -1;

            if (Input.GetKey(KeyCode.Space) && _canJump)
            {
                _speedVertical = _jump;
            }
        }

        // Move
        _speedVertical -= _gravity * Time.deltaTime;
        _movement.y = _speedVertical;
        _characterController.Move(_movement * Time.deltaTime);

        // Animation       
        _animatorController.Movement(_movement, _isRunning, _characterController.isGrounded);
    }

    private void Stamina()
    {
        if (_isRunning && !_infiniteStamina || Input.GetKey(KeyCode.LeftShift) && _stamina < 1 && !_infiniteStamina)
        {
            _stamina = Mathf.Clamp(_stamina - (_staminaDecrease * Time.deltaTime), 0, _staminaMax);

            _staminaRegenTimer = 0;
        }
        else if (_stamina < _staminaMax)
        {
            if (_staminaRegenTimer >= _staminaTimeToRegen)
            {
                _stamina = Mathf.Clamp(_stamina + (_staminaIncrease * Time.deltaTime), 0, _staminaMax);
            }
            else
            {
                _staminaRegenTimer += Time.deltaTime;
            }
        }

        GameManager.Instance.worldUI.UpdateStamina(_stamina / _staminaMax);
    }

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactionEvent.lastPlayerPosition = transform.position;
            EventController.TriggerEvent(_interactionEvent);
        }

        if (Input.GetKey(KeyCode.L))
        {
            GameManager.Instance.worldUI.Diary(true);
        }
        else
        {
            GameManager.Instance.worldUI.Diary(false);
        }

    }

    public void ChangeMovement(bool enabled)
    {
        _canMove = enabled;
    }

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

    #region Quest

    // TODO Mariano: MOVE TO GAME MANAGER
    public void Quests()
    {

        // if (QuestSO.isActive)
        // {
        //     if (quest.goal.isReached())
        //     {
        //         // experience += _quest.experienceReward; ADD VARIABLE IN SO CHARACTER
        //         // gold += _quest.goldReward; ADD VARIABLE IN SO CHARACTER
        //         quest.goal.Complete();
        //     }
            
        // }
        
    }

    // public void AddQuest()
    // {
    //     questLog.Add( 1, quest);
        
    // }
    public void RemoveQuest()
    {
        GameManager.Instance.worldUI._questLogTitleTxt.alpha = 0;
    }

    #endregion

}