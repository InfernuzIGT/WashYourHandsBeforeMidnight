using UnityEditor;
using UnityEngine;

public class InteractionItemCreator : EditorWindow
{
    // Interaction Item Data
    public new string name;
    public ItemSO item;
    public QuestSO quest;
    public int progress;
    public Transform customSpawnPoint;

    private string _itemName = "Item_{0}";
    private string _prefabPath = "Assets/Prefabs/Interaction/Interaction Item.prefab";
    private GUIStyle _styleButtons;
    private Vector2 _scroll;

    [MenuItem("Tools/Interaction Item Creator")]
    static void CreateInteractionItemCreator()
    {
        var window = EditorWindow.GetWindow<InteractionItemCreator>();
        Texture2D iconTitle = EditorGUIUtility.Load("d_PrefabVariant Icon")as Texture2D;
        GUIContent titleContent = new GUIContent("Interaction Item Creator", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(300, 225);
        window.Show();
    }

    private void OnGUI()
    {
        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fixedHeight = 40 };

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Interaction Item Data", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        name = EditorGUILayout.TextField("Name", name);

        EditorGUILayout.Space();
        item = (ItemSO)EditorGUILayout.ObjectField("Item", item, typeof(ItemSO), true);

        EditorGUILayout.Space();
        quest = (QuestSO)EditorGUILayout.ObjectField("Quest", quest, typeof(QuestSO), true);
        progress = EditorGUILayout.IntField("Progress", progress);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        customSpawnPoint = (Transform)EditorGUILayout.ObjectField("Custom spawn point", customSpawnPoint, typeof(Transform), true);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Create", _styleButtons))
        {
            CreateItem();
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
        GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(_prefabPath, typeof(GameObject));

        if (prefab != null)
        {
            GameObject newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newPrefab.gameObject.name = string.Format(_itemName, name);
            newPrefab.transform.SetAsLastSibling();

            InteractionItem interactionItem = newPrefab.GetComponent<InteractionItem>();
            interactionItem.InstantiatePrefab(item, quest, progress);

            if (customSpawnPoint != null)newPrefab.transform.position = customSpawnPoint.position;
        }
    }

    private void ClearItem()
    {
        name = null;
        item = null;
        quest = null;
        progress = 0;
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