using UnityEngine;
using FMODUnity;

public class InteriorSound : MonoBehaviour
{
    public StudioEventEmitter interiorWarehouse;
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // interiorWarehouse.Play();

    }
    private void OnTriggerExit(Collider other)
    {
        // interiorWarehouse.Stop();
    }
}
