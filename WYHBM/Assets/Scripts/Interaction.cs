using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class Interaction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Turn on box collider 
        // Character can interact
        Debug.Log ($"<b> Interaction </b>");
    }
    private void OnTriggerExit(Collider other)
    {
        // Turn off box collider 
        // Character can't interact
        Debug.Log ($"<b> No Interaction </b>");
    }
}