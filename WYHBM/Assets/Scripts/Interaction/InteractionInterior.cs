using UnityEngine;

public class InteractionInterior : Interaction, IInteractable
{
    [Header("Interior")]
    public bool inInterior;

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            // GameManager.Instance.globalController.ChangeWorldCamera();
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player)) { }
    }

}