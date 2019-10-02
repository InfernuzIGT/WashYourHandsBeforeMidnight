using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : CharacterEditor
{
    Enemy gameData;

    private void OnEnable()
    {
        gameData = target as Enemy;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (gameData.character != null)
        {
            EditorGUILayout.Space();
            DrawHorizontalLine();

            GUILayout.Label("Character Data", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(gameData.CharacterData.ToString(), GUI.skin.GetStyle(typeHelpBox));
        }

    }

}