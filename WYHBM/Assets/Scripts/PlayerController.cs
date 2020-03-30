using Events;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Velocity")]
    public float speedWalk = 7.5f;
    public float speedRun = 15f;

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

    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
    }
    private void Start()
    {
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
        Movement();
        Interaction();
    }

    private void Movement()
    {
        if (!_canMove)return;

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

}