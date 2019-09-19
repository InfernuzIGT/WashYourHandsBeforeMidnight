using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TemplateEnumButton))]
public class TemplateEnumButtonEditor : Editor
{

    TemplateEnumButton templateEnumButton;

    // Toolbar
    private int toolBar;
    private string[] toolBarOptions = new string[] { "A", "B", "C" };

    private void OnEnable()
    {
        templateEnumButton = target as TemplateEnumButton;
        toolBar = EditorPrefs.GetInt("index", 0);
    }

    private void OnDisable()
    {
        EditorPrefs.SetInt("index", toolBar);
    }

    public override void OnInspectorGUI()
    {
        toolBar = GUILayout.Toolbar(toolBar, toolBarOptions);

        EditorGUILayout.Space();

        switch (toolBar)
        {
            case 0:
                templateEnumButton.option = OPTION_TEMPLATE.A;
                break;

            case 1:
                templateEnumButton.option = OPTION_TEMPLATE.B;
                break;

            case 2:
                templateEnumButton.option = OPTION_TEMPLATE.C;
                break;

            default:
                break;
        }

    }
}