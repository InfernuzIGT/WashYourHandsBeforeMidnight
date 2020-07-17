using Events;
using UnityEngine;

public class InteractionZipline : Interaction
{
    public GameObject endPosition;

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractZipline);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractZipline);
        }
    }

    private void OnInteractZipline(InteractionEvent evt)
    {
        GameManager.Instance.globalController.player._inZipline = true;
        GameManager.Instance.globalController.player.endPos = endPosition.transform.position;

    }
}