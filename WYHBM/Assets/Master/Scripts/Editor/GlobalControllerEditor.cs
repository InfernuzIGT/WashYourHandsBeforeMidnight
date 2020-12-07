using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlobalController))]
public class GlobalControllerEditor : Editor
{

    private GlobalController _controller;
    private GUIStyle _styleButtons;

    private void OnEnable()
    {
        _controller = target as GlobalController;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 };

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save", _styleButtons))
        {
            if (!Application.isPlaying)return;

            _controller.EditorSave();
        }

        if (GUILayout.Button("Load", _styleButtons))
        {
            if (!Application.isPlaying)return;

            _controller.EditorLoad();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}