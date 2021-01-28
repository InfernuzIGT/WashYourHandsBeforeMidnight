using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuController))]
public class MenuControllerEditor : Editor
{
    private MenuController _controller;

    private void OnEnable()
    {
        _controller = target as MenuController;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        if (GUILayout.Button("Set Version", GUILayout.Height(35)))_controller.SetVersion();
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

}