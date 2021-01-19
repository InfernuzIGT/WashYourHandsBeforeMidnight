﻿using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Interaction), typeof(BoxCollider))]
public class DebugInteraction : MonoBehaviour
{
    public bool showDebug;
    public Color boxColor = new Color(0, 1, 0, 0.5f);

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (showDebug)
        {
            Gizmos.color = boxColor;
            Gizmos.DrawCube(_boxCollider.center + transform.position, _boxCollider.size);
        }
    }

#endif
}