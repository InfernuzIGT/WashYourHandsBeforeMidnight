using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class CombatEnemyEditor : Editor
{
    [Header("Combat Character")]
    private Enemy _controller;

    private void OnEnable()
    {
        _controller = target as Enemy;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        if (_controller.Data == null)return;

        if (GUILayout.Button("Set Data", GUILayout.Height(35)))_controller.SetData();
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}