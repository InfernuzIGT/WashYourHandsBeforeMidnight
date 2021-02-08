using UnityEngine;
using FMODUnity;

public class ExteriorRooftopSound : MonoBehaviour
{
    public StudioEventEmitter rooftopSound;
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        rooftopSound.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        rooftopSound.Stop();

    }
}
