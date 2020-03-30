using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class Interaction : MonoBehaviour
{
    [System.Serializable]
    public class InteractionEvent : UnityEvent<Collider> { }

    public InteractionEvent onEnter;
    public InteractionEvent onExit;

    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke(other);
    }
}