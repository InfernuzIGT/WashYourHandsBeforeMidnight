using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEditor : EditorWindow
{
    // Scene Editor
    public SceneSO sceneData;

    private string _pathMainMenu = "Assets/Master/Main Menu.unity";
    private string _pathPersistent = "Assets/Master/Persistent.unity";
    private string _pathPlayer = "Assets/Master/Player.unity";
    private string _pathLevelTest = "Assets/_BreakpointStudios/Levels/Level Test/Scenes/Level Test.unity";

    private GUIStyle _styleButtons;
    private GUIStyle _styleButtonBig;

    [MenuItem("Tools/Scene Editor")]
    static void CreateColliderWindow()
    {
        var window = EditorWindow.GetWindow<SceneEditor>();
        Texture2D iconTitle = EditorGUIUtility.Load(EditorGUIUtility.isProSkin ? "SceneAsset On Icon" : "SceneAsset Icon")as Texture2D;
        GUIContent titleContent = new GUIContent("Scene Editor", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(325, 465);
        window.Show();
    }

    private void OnGUI()
    {
        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 };
        _styleButtonBig = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 60 };

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        Scene("Main Menu", _pathMainMenu);
        EditorGUILayout.Space();
        Scene("Persistent", _pathPersistent);
        EditorGUILayout.Space();
        Scene("Player", _pathPlayer);
        EditorGUILayout.Space();
        Scene("Level Test", _pathLevelTest);
        EditorGUILayout.Space();

        SceneData();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DrawHorizontalLine();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Save All", _styleButtonBig))
        {
            // EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveOpenScenes();
        }

        // DrawSize();
    }

    private void Scene(string name, string path)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Scene: " + name);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load", _styleButtons))
        {
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }

        if (GUILayout.Button("Unload", _styleButtons))
        {
            EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(path), true);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void SceneData()
    {
        EditorGUILayout.Space();

        sceneData = (SceneSO)EditorGUILayout.ObjectField("Scene Data", sceneData, typeof(SceneSO), true);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load", _styleButtons))
        {
            if (sceneData == null)return;

            for (int i = 0; i < sceneData.scenes.Length; i++)
            {
                EditorSceneManager.OpenScene(sceneData.scenes[i], OpenSceneMode.Additive);
            }
        }

        if (GUILayout.Button("Unload", _styleButtons))
        {
            if (sceneData == null)return;

            for (int i = 0; i < sceneData.scenes.Length; i++)
            {
                EditorSceneManager.CloseScene(SceneManager.GetSceneByPath(sceneData.scenes[i].ScenePath), true);
            }
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
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