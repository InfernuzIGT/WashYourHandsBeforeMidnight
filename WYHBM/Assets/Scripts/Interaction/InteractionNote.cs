using Events;
using UnityEngine;

public class InteractionNote : Interaction
{
    [Header("Note")]
    public NoteSO notes;
    private bool _isActiveNote = false;

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractNote);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractNote);
        }
    }

    private void OnInteractNote(InteractionEvent evt)
    {
        if (_isActiveNote)
        {
            GameManager.Instance.worldUI.ActiveNote(_isActiveNote);
            GameManager.Instance.SetPause(true);
            _isActiveNote = false;
        }
        else
        {
            GameManager.Instance.worldUI.ActiveNote(_isActiveNote);
            GameManager.Instance.SetPause(true);
            _isActiveNote = true;
        }

        GameManager.Instance.worldUI.noteTxt.text = notes.noteSentences;

    }
}