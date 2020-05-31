using UnityEngine;

public class TargetBehind : MonoBehaviour
{
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    public void Detected(bool isDetected)
    {
        _material.SetFloat("_isDetected", isDetected ? 1 : 0);
    }

}