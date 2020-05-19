using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemCreator : EditorWindow
{
    // Item Data
    public new string name;
    public string description;
    public Texture2D texture;
    public ITEM_TYPE itemType = ITEM_TYPE.None;
    public int valueMin = 0;
    public int valueMax = 100;

    private string _pathAndName = "Assets/Data/Item/New Item.asset";
    private string _itemPathAndName;
    private string _spritePath;
    private ItemSO _itemSO;
    private GUIStyle _styleButtons;
    private Vector2 _scroll;
    private Sprite[] _sprites;

    [MenuItem("Tools/Item Creator")]
    static void CreateItemCreator()
    {
        var window = EditorWindow.GetWindow<ItemCreator>();
        Texture2D iconTitle = EditorGUIUtility.Load("d_UnityEditor.ConsoleWindow")as Texture2D;
        GUIContent titleContent = new GUIContent("Item Creator", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(300, 355);
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

        EditorGUILayout.Space();
        texture = (Texture2D)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture2D), true);

        EditorGUILayout.Space();

        itemType = (ITEM_TYPE)EditorGUILayout.EnumPopup("Item Type", itemType);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        valueMin = EditorGUILayout.IntSlider("Value Min", valueMin, 0, 100);
        valueMax = EditorGUILayout.IntSlider("Value Max", valueMax, 0, 100);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create", _styleButtons))
        {
            CreateItem();

            _itemPathAndName = AssetDatabase.GenerateUniqueAssetPath(_pathAndName);

            AssetDatabase.CreateAsset(_itemSO, _itemPathAndName);
            AssetDatabase.RenameAsset(_itemPathAndName, name);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _itemSO;
        }

        if (GUILayout.Button("Clear", _styleButtons))
        {
            ClearItem();
        }

        EditorGUILayout.EndHorizontal();

        // DrawSize();
    }

    private void CreateItem()
    {
        _itemSO = ScriptableObject.CreateInstance<ItemSO>();

        _spritePath = AssetDatabase.GetAssetPath(texture);
        _sprites = AssetDatabase.LoadAllAssetsAtPath(_spritePath).OfType<Sprite>().ToArray();

        _itemSO.name = name;
        _itemSO.description = description;
        _itemSO.sprite = _sprites[0];
        _itemSO.previewSprite = _sprites[1];
        _itemSO.itemType = itemType;
        _itemSO.valueMin = valueMin;
        _itemSO.valueMax = valueMax;
    }

    private void ClearItem()
    {
        name = null;
        description = null;
        texture = null;
        itemType = ITEM_TYPE.None;
        valueMin = 0;
        valueMax = 100;
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