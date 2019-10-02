using UnityEditor;
using UnityEngine;

public class CharacterEditor : Editor
{
    public readonly string typeHelpBox = "HelpBox";
    public readonly string typeGroupBox = "GroupBox";

    public virtual void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    
}