using UnityEngine;

public class TargetBehind : MonoBehaviour
{
    public Material materialDetected;
    
    private MeshRenderer _meshRenderer;
    private Material _materialNormal;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialNormal = _meshRenderer.material;
        gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(GameData.Instance.worldConfig.layerOcclusionMask));
    }

    public void Detected(bool isDetected)
    {
        _meshRenderer.material = isDetected ? materialDetected : _materialNormal;
    }

}