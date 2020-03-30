using Events;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public DialogSO dialog;

    private UIEnableDialogEvent _UIEnableDialogEvent;

    private void Start()
    {
        _UIEnableDialogEvent = new UIEnableDialogEvent();
        _UIEnableDialogEvent.dialog = dialog;
    }

    public void ChangeState(bool state)
    {
        _UIEnableDialogEvent.enable = state;
        EventController.TriggerEvent(_UIEnableDialogEvent);
    }
}