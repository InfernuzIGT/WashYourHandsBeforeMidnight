using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractionScene))]
public class InteractionSceneEditor : Editor
{
    private InteractionScene _controller;

    private void OnEnable()
    {
        _controller = target as InteractionScene;
    }

    private void OnSceneGUI()
    {
        if (_controller.ShowDebug)
        {
            _controller.newPlayerPosition = Handles.PositionHandle(_controller.newPlayerPosition, Quaternion.identity);
            Handles.Label(_controller.newPlayerPosition, "New Position", EditorStyles.whiteLargeLabel);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorUtility.SetDirty(_controller);

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        if (GUILayout.Button("Reset Position"))
        {
            _controller.ResetPosition();
        }
    }
    
    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}