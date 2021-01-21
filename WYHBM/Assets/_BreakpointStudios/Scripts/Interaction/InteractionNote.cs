using Events;
using UnityEngine;

public class InteractionNote : Interaction
{
    [Header("Note")]
    public NoteSO notes;
    private bool _isActive;

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

            if (_isActive)
            {
                _isActive = false;
                GameManager.Instance.Pause(PAUSE_TYPE.Note);
            }
        }
    }

    private void OnInteractNote(InteractionEvent evt)
    {
        _isActive = !_isActive;

        GameManager.Instance.worldUI.SetNoteText(notes.noteSentences);
        GameManager.Instance.Pause(PAUSE_TYPE.Note);
    }
}