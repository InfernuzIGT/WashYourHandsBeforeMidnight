using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class CombatPlayerEditor : Editor
{
    [Header("Combat Character")]
    private Player _controller;

    private void OnEnable()
    {
        _controller = target as Player;
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