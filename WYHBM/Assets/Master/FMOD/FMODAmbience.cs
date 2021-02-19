using FMODUnity;
using UnityEngine;

public class FMODAmbience : MonoBehaviour
{
    [Header("FMOD")]
    [SerializeField] private bool _playInStart = false;
    [Space]
    [SerializeField] private StudioEventEmitter _emitter = null;

    private void Start()
    {
        if (_playInStart)_emitter.Play();
    }

    public void Execute(bool isPlay)
    {
        if (isPlay)
        {
            _emitter.Play();
        }
        else
        {
            _emitter.Stop();
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     _emitter.Play();
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     _emitter.Stop();
    // }

}