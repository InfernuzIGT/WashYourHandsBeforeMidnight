using System.Collections.Generic;
using UnityEngine;

public class InteractionCorpse : Interaction, IInteractable
{
    // TODO Mariano: Spawnear con Sprite de CombatCharacterSO en la escena actual
    
    // [Header("Corpse")]

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
        }
    }

}