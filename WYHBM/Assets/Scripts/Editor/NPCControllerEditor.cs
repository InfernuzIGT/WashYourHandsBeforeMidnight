using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCController))]
public class NPCControllerEditor : Editor
{
    private NPCController _controller;

    private Vector3 _viewAngleA;
    private Vector3 _viewAngleB;

    private void OnEnable()
    {
        _controller = target as NPCController;
    }

    private void OnSceneGUI()
    {
        if (_controller == null ||
            _controller.VisibleTargets == null ||
            _controller.Data == null ||
            !_controller.Data.CanDetectPlayer)
        {
            return;
        }

        Handles.color = Color.white;
        Handles.DrawWireArc(_controller.transform.position, Vector3.up, Vector3.forward, 360, _controller.Data.ViewRadius);

        _viewAngleA = _controller.DirFromAngle(-_controller.ViewAngle / 2, false);
        _viewAngleB = _controller.DirFromAngle(_controller.ViewAngle / 2, false);

        Handles.DrawLine(_controller.transform.position, _controller.transform.position + _viewAngleA * _controller.Data.ViewRadius);
        Handles.DrawLine(_controller.transform.position, _controller.transform.position + _viewAngleB * _controller.Data.ViewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in _controller.VisibleTargets)
        {
            Handles.DrawLine(_controller.transform.position, visibleTarget.position);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        if (GUILayout.Button("Set Data"))
        {
            if (_controller.Data != null)
            {
                _controller.SetData();
            }
            else
            {
                Debug.LogError($"<color=red><b>[ERROR]</b></color> No data available!");
            }
        }
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}