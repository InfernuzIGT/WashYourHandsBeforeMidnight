using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationUtility))]
public class LocalizationUtilityEditor : Editor
{

    private LocalizationUtility _controller;
    private GUIStyle _styleButtons;

    private void OnEnable()
    {
        _controller = target as LocalizationUtility;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 };

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Previous", _styleButtons))
        {
            if (!Application.isPlaying)return;

            _controller.SelectNextLanguage(false);
        }

        if (GUILayout.Button("Next", _styleButtons))
        {
            if (!Application.isPlaying)return;

            _controller.SelectNextLanguage(true);
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