using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DBEditor
{
    public class DBEditorWindow : EditorWindow
    {
        private static bool _loaded;
        private static float _delay;
        private static double _lastTime;

        public DBEditorConfig config;

        private Editor editor;
        private Object[] _selectedActual;
        private string _selectedLastName;

        private string _searchString;
        private bool _inspectorLock;
        private TreeViewState _treeViewState;
        private DBEditorTreeView _dbEditorTreeView;
        private Object[] _selected;
        private GenericMenu _createMenu;
        private float _treeViewLayoutWidth = 185f;
        private Rect _treeViewRect;
        private bool _dragging;
        private float _splitterWidth = 0;
        private float _labelWidth;

        [MenuItem("Tools/DBEditor", priority = 1100)]
        static void Init()
        {
            if (!_loaded)
            {
                _loaded = true;
                _delay = 0.1f;
                _lastTime = EditorApplication.timeSinceStartup;
                EditorApplication.update += OnEditorUpdate;
                return;
            }

            LoadWindow();
        }

        private static void OnEditorUpdate()
        {
            _delay -= (float)(EditorApplication.timeSinceStartup - _lastTime);
            if (_delay > 0)
            {
                _lastTime = EditorApplication.timeSinceStartup;
                return;
            }

            LoadWindow();
            EditorApplication.update -= OnEditorUpdate;
        }

        private static void LoadWindow()
        {
            var window = EditorWindow.GetWindow<DBEditorWindow>();
            Texture2D iconTitle = EditorGUIUtility.Load("Icons/d_Project.png")as Texture2D;
            GUIContent titleContent = new GUIContent("Database", iconTitle);
            window.titleContent = titleContent;
            window.minSize = new Vector2(685, 400);
            window.Show();
        }

        private void Awake()
        {
            _searchString = "";
            _treeViewRect = new Rect(10, 25, _treeViewLayoutWidth - 15, position.height - 35);
        }

        private void OnEnable()
        {
            if (_treeViewState == null)
                _treeViewState = new TreeViewState();

            _dbEditorTreeView = new DBEditorTreeView(_treeViewState, config);
            _labelWidth = config.LabelWidth;

            LoadCreateMenu();
        }

        private void LoadCreateMenu()
        {
            var possibleElements = _dbEditorTreeView.GetElementTypes();
            _createMenu = new GenericMenu();
            var en = possibleElements.GetEnumerator();
            while (en.MoveNext())
            {
                var item = en.Current;
                _createMenu.AddItem(new GUIContent(item.Value), false, _dbEditorTreeView.CreateNewElement, item.Key);
            }
        }

        private GUIStyle GetStyle(string name)
        {
            //if (_editorSkin == null)
            //	_editorSkin = ScriptableObject.Instantiate(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector)) as GUISkin;
            //GUIStyle style =  _editorSkin.FindStyle(name);
            GUIStyle style = (GUIStyle)name;

            return style;
        }

        private void OnGUI()
        {
            DrawToolBar();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(_treeViewLayoutWidth));

            GUILayout.FlexibleSpace();

            _treeViewRect.height = position.height - 35;
            _treeViewRect.width = _treeViewLayoutWidth - 15;

            _dbEditorTreeView.OnGUI(_treeViewRect);
            _dbEditorTreeView.searchString = _searchString;

            EditorGUILayout.EndVertical();

            // Splitter
            GUILayout.Box(GUIContent.none, GUILayout.Width(_splitterWidth), GUILayout.ExpandHeight(true));
            HandleSplitView();
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical();

            EditorGUIUtility.labelWidth = _labelWidth;

            if (!_inspectorLock)
            {
                _selected = _dbEditorTreeView.GetSelectedObjects();
                _selectedActual = _selected;
            }

            if (_selected != null)
            {
                EditorGUILayout.BeginHorizontal(GetStyle("ProjectBrowserTopBarBg"));
                GUILayout.FlexibleSpace();
                _inspectorLock = GUILayout.Toggle(_inspectorLock, "", GetStyle("IN LockButton"));
                GUILayout.Space(10);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();

                if (editor == null || _selectedLastName != _selected[0].name)
                {
                    editor = Editor.CreateEditor(_selected);
                    _selectedLastName = _selected[0].name;
                }

                editor.DrawHeader();
                editor.OnInspectorGUI();

                GUILayout.Space(30);

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolBar()
        {
            GUILayout.BeginHorizontal(GetStyle("toolbar"));

            if (GUILayout.Button("Expand All", GetStyle("toolbarbutton")))
            {
                _dbEditorTreeView.ExpandAll();
            }

            if (GUILayout.Button("Collapse All", GetStyle("toolbarbutton")))
            {
                _dbEditorTreeView.CollapseAll();
            }

            GUILayout.Space(10);

            // if (GUILayout.Button("AZ↓", GetStyle("toolbarbutton")))
            // {
            // 	_dbEditorTreeView.SortAlphabetically();
            // }

            GUILayout.Space(10);

            var selected = _dbEditorTreeView.GetSelectedObjects();
            if (selected != null && selected.Length == 1)
            {
                GUILayout.Space(5);

                if (GUILayout.Button("Find in Project", GetStyle("toolbarbutton")))
                {
                    EditorGUIUtility.PingObject(selected[0]);
                }
            }

            GUILayout.FlexibleSpace();

            // _searchString = GUILayout.TextField(_searchString, GetStyle("ToolbarSeachTextFieldPopup"), GUILayout.Width(200));
            // var editorStyle = string.IsNullOrEmpty(_searchString) ? "ToolbarSeachCancelButtonEmpty" : "ToolbarSeachCancelButton";
            // if (GUILayout.Button("", GetStyle(editorStyle)))
            // {
            //     _searchString = "";
            //     GUI.FocusControl(null);
            //     _dbEditorTreeView.FocusSelected();
            // }

            GUILayout.EndHorizontal();
        }

        private void HandleSplitView()
        {
            Rect splitterRect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);

            if (Event.current != null)
            {
                switch (Event.current.rawType)
                {
                    case EventType.MouseDown:
                        if (splitterRect.Contains(Event.current.mousePosition))
                            _dragging = true;
                        break;
                    case EventType.MouseDrag:
                        if (_dragging)
                        {
                            splitterRect.x += Event.current.delta.x;
                            _treeViewLayoutWidth += Event.current.delta.x;
                            Repaint();
                        }
                        break;
                    case EventType.MouseUp:
                        if (_dragging)
                            _dragging = false;
                        break;
                }
            }
        }

    }
}