using UnityEngine;

public class InteractionEncounter : Interaction, IInteractable
{
    [Header ("Encounter Zone")]
    public ENCOUNTER_ZONE encounterZone;
    
    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            GameManager.Instance.ChangeEncounterZone(true, encounterZone);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            GameManager.Instance.ChangeEncounterZone(false);
        }
    }

    
}