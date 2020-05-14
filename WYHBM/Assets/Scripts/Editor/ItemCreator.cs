using UnityEditor;
using UnityEngine;

public class ItemCreator : EditorWindow
{
    // Item Data
    public new string name;
    public string description;
    public Sprite sprite;
    public Sprite previewSprite;

    private string _pathAndName = "Assets/Data/Item/New item.asset";
    private string _itemPathAndName;
    private ItemSO _itemSO;
    private GUIStyle _styleButtons;
    private Vector2 _scroll;

    [MenuItem("Tools/Item Creator")]
    static void CreateReplaceWithPrefab()
    {
        var window = EditorWindow.GetWindow<ItemCreator>();
        Texture2D iconTitle = EditorGUIUtility.Load("d_UnityEditor.ConsoleWindow")as Texture2D;
        GUIContent titleContent = new GUIContent("Item Creator", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(300, 340);
        window.Show();
    }

    private void OnGUI()
    {
        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fixedHeight = 40 };

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Item Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        name = EditorGUILayout.TextField("Name", name);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Description");
        description = EditorGUILayout.TextArea(description, GUILayout.Height(50));
        EditorGUILayout.Space();

        sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", sprite, typeof(Sprite), true);
        EditorGUILayout.Space();
        previewSprite = (Sprite)EditorGUILayout.ObjectField("Preview Sprite", previewSprite, typeof(Sprite), true);

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create", _styleButtons))
        {
            CreateItem();

            _itemPathAndName = AssetDatabase.GenerateUniqueAssetPath(_pathAndName);

            AssetDatabase.CreateAsset(_itemSO, _itemPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("Clear", _styleButtons))
        {
            ClearItem();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void CreateItem()
    {
        _itemSO = ScriptableObject.CreateInstance<ItemSO>();

        _itemSO.name = name;
        _itemSO.description = description;
        _itemSO.previewSprite = previewSprite;
        _itemSO.Sprite = sprite;
    }

    private void ClearItem()
    {
        name = null;
        description = null;
        sprite = null;
        previewSprite = null;
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