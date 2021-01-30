using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Interaction), typeof(BoxCollider))]
public class DebugInteraction : MonoBehaviour
{
    public enum DEBUG_TYPE
    {
        None = 0,
        Selected = 1,
        Constant = 2
    }

    public DEBUG_TYPE debugType;
    public Color boxColor = new Color(0, 1, 0, 0.5f);

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (debugType == DEBUG_TYPE.Selected)
        {
            Gizmos.color = boxColor;
            Gizmos.DrawCube(_boxCollider.center + transform.position, _boxCollider.size);
        }
    }
    
    private void OnDrawGizmos() {
        
        if (debugType == DEBUG_TYPE.Constant)
        {
            Gizmos.color = boxColor;
            Gizmos.DrawCube(_boxCollider.center + transform.position, _boxCollider.size);
        }
    }

#endif
}