using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCController))]
public class NPCControllerEditor : Editor
{
    private NPCController _controller;

    private void OnEnable()
    {
        _controller = target as NPCController;
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