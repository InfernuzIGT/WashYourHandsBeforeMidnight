using UnityEngine;

public class InteractionCutscene : Interaction, IInteractable
{

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            TriggerCutscene();
            Destroy(this.gameObject);
        }
    }

    public void OnInteractionExit(Collider other) { }

}