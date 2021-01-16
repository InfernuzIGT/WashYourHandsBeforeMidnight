using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private FieldOfView _fov;

    private Vector3 _viewAngleA;
    private Vector3 _viewAngleB;

    private void OnEnable()
    {
        _fov = target as FieldOfView;
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(_fov.transform.position, Vector3.up, Vector3.forward, 360, _fov.viewRadius);

        _viewAngleA = _fov.DirFromAngle(-_fov.viewAngle / 2, false);
        _viewAngleB = _fov.DirFromAngle(_fov.viewAngle / 2, false);

        Handles.DrawLine(_fov.transform.position, _fov.transform.position + _viewAngleA * _fov.viewRadius);
        Handles.DrawLine(_fov.transform.position, _fov.transform.position + _viewAngleB * _fov.viewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in _fov.visibleTargets)
        {
            Handles.DrawLine(_fov.transform.position, visibleTarget.position);
        }
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}