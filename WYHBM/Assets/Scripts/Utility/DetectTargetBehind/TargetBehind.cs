using UnityEngine;

public class TargetBehind : MonoBehaviour
{
    public Material materialDetected;
    
    [SerializeField]
    private int layerValue = 12; // Occlusion Mask

    private MeshRenderer _meshRenderer;
    private Material _materialNormal;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialNormal = _meshRenderer.material;
        gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(layerValue));
    }

    public void Detected(bool isDetected)
    {
        _meshRenderer.material = isDetected ? materialDetected : _materialNormal;
    }

}