using Events;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Character
{
    [Header("Movement")]
    public float speedWalk = 7.5f;
    public float speedRun = 15f;

    private CharacterController _characterController;
    private AnimatorController _animatorController;

    private InteractionEvent _interactionEvent;

    // Movement 
    private bool _canMove = true;
    private bool _canJump = true;
    private bool _isRunning;
    private float _jump = 9.81f;
    private float _gravity = 29.43f;
    private Vector3 _movement;
    private float _speedHorizontal;
    private float _speedVertical;

    private float _moveHorizontal;
    private float _moveVertical;
    private float _posX;
    private float _posZ;

    private Vector3 _lastPosition;

    // Input
    private string _inputHorizontal = "Horizontal";
    private string _inputVertical = "Vertical";

    public Quest quest;

    Dictionary<int, Quest> questLog = new Dictionary<int, Quest>();

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
        Interaction();
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isRunning = _moveHorizontal == 0 && _moveVertical == 0 || !_characterController.isGrounded ? false : true;
            _speedHorizontal = speedRun;
        }
        else
        {
            _isRunning = false;
            _speedHorizontal = speedWalk;
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

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactionEvent.lastPlayerPosition = transform.position;
            EventController.TriggerEvent(_interactionEvent);
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

    public void QuestDiary()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            quest.giver.OpenDiary();
        }
        else
        {
            quest.giver.CloseDiary();
        }
        
    }

    #endregion

    #region Quest

    public void Quests()
    {

        if (Quest.isActive)
        {
            if (quest.goal.isReached())
            {
                // experience += _quest.experienceReward; ADD VARIABLE IN SO CHARACTER
                // gold += _quest.goldReward; ADD VARIABLE IN SO CHARACTER
                quest.goal.Complete();
            }
            
        }
        
    }

    public void AddQuest()
    {
        questLog.Add( 1, quest);
        
    }
    public void RemoveQuest()
    {
        questLog.Remove(1);
        
    }

    #endregion

}