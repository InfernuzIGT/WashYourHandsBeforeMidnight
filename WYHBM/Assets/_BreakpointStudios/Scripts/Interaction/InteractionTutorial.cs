using Events;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionTutorial : Interaction
{
    [Header("Tutorial")]
    [SerializeField] private TutorialSO _tutorialData = null;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private InputActionReference _actionSelect = null;
    [SerializeField, ConditionalHide] private InputActionReference _actionBack = null;

    private TutorialEvent _tutorialEvent;
    private EnableMovementEvent _enableMovementEvent;

    private bool _removed;

    private void Start()
    {
        _checkCurrentInteraction = false;

        _tutorialEvent = new TutorialEvent();
        _tutorialEvent.data = _tutorialData;

        _enableMovementEvent = new EnableMovementEvent();
        _enableMovementEvent.isDetected = false;
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(true);
        }
    }

    public void OnInteractionExit(Collider other) { }

    public override void Execute(bool enable)
    {
        base.Execute();

        // if (_removed)return;

        _enableMovementEvent.canMove = !enable;
        EventController.TriggerEvent(_enableMovementEvent);

        _tutorialEvent.show = enable;
        EventController.TriggerEvent(_tutorialEvent);

        if (enable)
        {
            _actionSelect.action.performed += RemoveUI;
            _actionBack.action.performed += RemoveUI;
        }
        else
        {
            _removed = true;

            _actionSelect.action.performed -= RemoveUI;
            _actionBack.action.performed -= RemoveUI;

            Destroy(gameObject);
        }

    }

    private void RemoveUI(InputAction.CallbackContext context)
    {
        Execute(false);
    }
}