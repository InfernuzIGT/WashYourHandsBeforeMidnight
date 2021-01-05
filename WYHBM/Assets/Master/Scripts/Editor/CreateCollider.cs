using UnityEditor;
using UnityEngine;

public class CreateCollider : EditorWindow
{
    private Transform _transform;
    private Quaternion _rotation;
    private BoxCollider _collider;
    private Bounds _bounds;
    private Vector3 _colliderSize;
    private bool _destroyMesh = true;

    private GUIStyle _styleButtons;

    [MenuItem("Tools/Create Collider")]
    static void CreateColliderWindow()
    {
        var window = EditorWindow.GetWindow<CreateCollider>();
        Texture2D iconTitle = EditorGUIUtility.Load("BoxCollider Icon")as Texture2D;
        GUIContent titleContent = new GUIContent("Create Collider", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(200, 120);
        window.Show();
    }

    private void OnGUI()
    {
        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 };

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        _destroyMesh = EditorGUILayout.Toggle("Destroy Mesh", _destroyMesh);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Box Collider", _styleButtons))
        {
            var selection = Selection.transforms;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                _transform = selection[i];
                _rotation = _transform.rotation;
                _transform.rotation = Quaternion.identity;

                _collider = _transform.gameObject.GetComponent<BoxCollider>();

                if (_collider != null)
                {
                    continue;
                }
                else
                {
                    _transform.gameObject.AddComponent<BoxCollider>();
                    _collider = _transform.gameObject.GetComponent<BoxCollider>();
                }

                _bounds = new Bounds(_transform.position, Vector3.zero);

                ExtendBounds(_transform, ref _bounds);

                _collider.center = _bounds.center - _transform.position;

                _colliderSize = new Vector3(
                    _bounds.size.x / _transform.localScale.x,
                    _bounds.size.y / _transform.localScale.y,
                    _bounds.size.z / _transform.localScale.z
                );

                _collider.size = _colliderSize;

                _transform.rotation = _rotation;

                if (_destroyMesh)
                {
                    DestroyImmediate(_transform.gameObject.GetComponent<MeshRenderer>());
                    DestroyImmediate(_transform.gameObject.GetComponent<MeshFilter>());
                }

            }
        }

        GUI.enabled = false;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);

        // DrawSize();
    }

    private void ExtendBounds(Transform t, ref Bounds b)
    {
        Renderer rend = t.gameObject.GetComponent<Renderer>();

        if (rend != null)
        {
            b.Encapsulate(rend.bounds.min);
            b.Encapsulate(rend.bounds.max);
        }

        foreach (Transform t2 in t)
        {
            ExtendBounds(t2, ref b);
        }
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