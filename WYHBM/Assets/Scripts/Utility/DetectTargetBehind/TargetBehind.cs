using UnityEngine;

public class TargetBehind : MonoBehaviour
{
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        // gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(GameData.Instance.worldConfig.layerOcclusionMask));
    }

    public void Detected(bool isDetected)
    {
        _material.SetFloat("_isDetected", isDetected ? 1 : 0);
    }

}