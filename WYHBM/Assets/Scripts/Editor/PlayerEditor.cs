using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : CharacterEditor
{
    Player gameData;

    private void OnEnable()
    {
        gameData = target as Player;
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