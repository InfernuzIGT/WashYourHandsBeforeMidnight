using Events;
using UnityEngine;
using UnityEngine.Localization;

public class InteractionSave : Interaction, IInteractable
{
    [Header("Save")]
    [SerializeField] private LocalizedString[] _localizedDialog;

    [Header("References")]
#pragma warning disable 0414
    [SerializeField] private bool ShowReferences = true;
#pragma warning restore 0414
    [SerializeField, ConditionalHide] private WorldAnimator _animatorController = null;

    private DialogSimpleEvent _interactionDialogEvent;
    private SessionEvent _sessionEvent;

    private void Start()
    {
        _sessionEvent = new SessionEvent();
        _sessionEvent.option = SESSION_OPTION.Save;

        _interactionDialogEvent = new DialogSimpleEvent();
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(true);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(false);
        }
    }

    public override void Execute(bool enable)
    {
        base.Execute();

        CanInteractEvent(enable);

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.localizedString = _localizedDialog[Random.Range(0, _localizedDialog.Length)];

        EventController.TriggerEvent(_interactionDialogEvent);
    }

    public override void OnInteractEvent()
    {
        base.OnInteractEvent();

        Execute(false);

        _animatorController.SpecialAnimation();

        EventController.TriggerEvent(_sessionEvent);
    }

}