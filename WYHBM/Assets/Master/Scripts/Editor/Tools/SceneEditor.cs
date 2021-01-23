using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEditor : EditorWindow
{
    // Scene Editor
    public SceneSO sceneData;

    private GUIStyle _styleButtons;

    [MenuItem("Tools/Scene Editor")]
    static void CreateColliderWindow()
    {
        var window = EditorWindow.GetWindow<SceneEditor>();
        Texture2D iconTitle = EditorGUIUtility.Load(EditorGUIUtility.isProSkin ? "SceneAsset On Icon" : "SceneAsset Icon")as Texture2D;
        GUIContent titleContent = new GUIContent("Scene Editor", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(325, 120);
        window.Show();
    }

    private void OnGUI()
    {
        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 };

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        sceneData = (SceneSO)EditorGUILayout.ObjectField("Scene Data", sceneData, typeof(SceneSO), true);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load", _styleButtons))
        {
            for (int i = 0; i < sceneData.scenes.Length; i++)
            {
                EditorSceneManager.OpenScene(sceneData.scenes[i], OpenSceneMode.Additive);
            }
        }

        if (GUILayout.Button("Unload", _styleButtons))
        {
            for (int i = 0; i < sceneData.scenes.Length; i++)
            {
                EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(sceneData.scenes[i].ScenePath), true);
            }
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Save All", _styleButtons))
        {
            // EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }

        // DrawSize();
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private void DrawSize()
    {
        EditorGUILayout.LabelField("X: " + base.position.width.ToString());
        EditorGUILayout.LabelField("Y: " + base.position.height.ToString());
    }

}