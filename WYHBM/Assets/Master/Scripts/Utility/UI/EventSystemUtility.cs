using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(EventSystem), typeof(InputSystemUIInputModule))]
public class EventSystemUtility : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private InputSystemUIInputModule _inputUIModule;

    public InputSystemUIInputModule InputUIModule { get { return _inputUIModule; } set { _inputUIModule = value; } }

    private void Start()
    {
        _inputUIModule.actionsAsset.Disable();
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

    private void OnUpdateSelection(EventSystemEvent evt)
    {
        _eventSystem.firstSelectedGameObject = evt.objectSelected;
        _eventSystem.SetSelectedGameObject(evt.objectSelected);
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        if (evt.canMove)
        {
            _inputUIModule.actionsAsset.Disable();
        }
        else
        {
            _inputUIModule.actionsAsset.Enable();
            _inputUIModule.actionsAsset.actionMaps[0].Disable(); // Player
        }
    }

}