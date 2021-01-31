using UnityEngine;
using FMODUnity;

public class ExteriorStreetsSound : MonoBehaviour
{
    public StudioEventEmitter streetsSound;
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        streetsSound.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        streetsSound.Stop();

    }
}
