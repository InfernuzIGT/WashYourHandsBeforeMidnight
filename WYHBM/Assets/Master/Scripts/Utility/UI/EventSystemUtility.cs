using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(EventSystem), typeof(InputSystemUIInputModule))]
public class EventSystemUtility : MonoBehaviour
{
    [Header("Event System")]
    [SerializeField] private bool _startEnable = false;
    [Space]
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private InputSystemUIInputModule _inputUIModule;

    public InputSystemUIInputModule InputUIModule { get { return _inputUIModule; } set { _inputUIModule = value; } }

    private void Start()
    {
        if (!_startEnable)_inputUIModule.actionsAsset.Disable();
    }

    private void OnEnable()
    {
        EventController.AddListener<EventSystemEvent>(OnUpdateSelection);
        EventController.AddListener<EnableMovementEvent>(OnStopMovement);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<EventSystemEvent>(OnUpdateSelection);
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
    }

    public void SetSelectedGameObject(GameObject objectSelected)
    {
        _eventSystem.firstSelectedGameObject = objectSelected;
        _eventSystem.SetSelectedGameObject(objectSelected);
    }

    public void DisableInput(bool disable)
    {
        if (disable)
        {
            _inputUIModule.actionsAsset.Disable();
        }
        else
        {
            _inputUIModule.actionsAsset.Enable();
            _inputUIModule.actionsAsset.actionMaps[0].Disable(); // Player
        }
    }

    private void OnUpdateSelection(EventSystemEvent evt)
    {
        SetSelectedGameObject(evt.objectSelected);
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        DisableInput(evt.canMove);
    }

}