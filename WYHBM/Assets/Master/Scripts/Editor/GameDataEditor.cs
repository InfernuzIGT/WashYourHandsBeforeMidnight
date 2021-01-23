using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class GameDataEditor : Editor
{
    private GameData _controller;
    private GUIStyle _styleButtons;

    private void OnEnable()
    {
        _controller = target as GameData;
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
            _controller.Save();
        }

        if (GUILayout.Button("Load", _styleButtons))
        {
            _controller.Load();
        }

        if (GUILayout.Button("Delete All", _styleButtons))
        {
            _controller.DeleteAll();
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