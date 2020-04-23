using Events;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController : Character
{
    [Header("Movement")]
    public float speedWalk = 7.5f;
    public float speedRun = 15f;

<<<<<<< Updated upstream
=======
<<<<<<< Updated upstream
    private CharacterController _characterController;
=======

>>>>>>> Stashed changes
>>>>>>> Stashed changes
    private AnimatorController _animatorController;
    private UIExecuteDialogEvent _UIExecuteDialogEvent;

    // Movement Values
    private bool _canMove = true;
    private float _moveHorizontal;
    private float _moveVertical;
    private float _posX;
    private float _posZ;

    //Movement Input
    private string _inputHorizontal = "Horizontal";
    private string _inputVertical = "Vertical";

    public Quest quest;

    Dictionary<int, Quest> questLog = new Dictionary<int, Quest>();

    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        _UIExecuteDialogEvent = new UIExecuteDialogEvent();
    }

    private void OnEnable()
    {
        EventController.AddListener<StopMovementEvent>(OnStopMovement);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<StopMovementEvent>(OnStopMovement);
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
        if (!_canMove)
        {
            _animatorController.Movement(0, 0);
            return;
        }

        _moveHorizontal = Input.GetAxisRaw(_inputHorizontal);
        _moveVertical = Input.GetAxisRaw(_inputVertical);

        _posX = _moveHorizontal * speedRun * Time.deltaTime;
        _posZ = _moveVertical * speedRun * Time.deltaTime;

        transform.Translate(_posX, 0, _posZ);

        _animatorController.Movement(_moveHorizontal, _moveVertical);
    }

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExecuteDialog();
        }
    }

    public void ChangeMovement(bool enabled)
    {
        _canMove = enabled;
    }

    #region Events

    private void ExecuteDialog()
    {
        EventController.TriggerEvent(_UIExecuteDialogEvent);
    }

    private void OnStopMovement(StopMovementEvent evt)
    {
        _canMove = evt.enable;
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

    #endregion

}