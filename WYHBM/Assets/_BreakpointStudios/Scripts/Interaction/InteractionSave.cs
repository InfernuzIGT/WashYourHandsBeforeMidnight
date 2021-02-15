using Events;
using UnityEngine;
using UnityEngine.Localization;

public class InteractionSave : Interaction, IInteractable
{
    [Header("Save")]
    // TODO Mariano: Ver si tira los 3 Dialogos
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
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(true);
            EventController.AddListener<InteractionEvent>(OnInteractSave);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Execute(false);
            EventController.RemoveListener<InteractionEvent>(OnInteractSave);
        }
    }

    public override void Execute(bool enable)
    {
        base.Execute();

        _interactionDialogEvent.enable = enable;
        _interactionDialogEvent.localizedString = _localizedDialog[Random.Range(0, _localizedDialog.Length)];

        EventController.TriggerEvent(_interactionDialogEvent);
    }

    private void OnInteractSave(InteractionEvent evt)
    {
        _animatorController.SpecialAnimation();

        EventController.TriggerEvent(_sessionEvent);
        EventController.RemoveListener<InteractionEvent>(OnInteractSave);
    }
}