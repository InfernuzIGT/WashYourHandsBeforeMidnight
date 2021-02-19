using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCController))]
public class NPCControllerEditor : Editor
{
    private NPCController _controller;
    private FieldOfView _fov;

    private Vector3 _viewAngleA;
    private Vector3 _viewAngleB;

    // Toolbar
    private int toolBarDirection;
    private string[] toolBarOptions = new string[] { "-", "-", "-", "-" };

    private void OnEnable()
    {
        _controller = target as NPCController;
        _fov = _controller.FieldOfView;

        // EditorUtility.SetDirty(_controller);

        // toolBarDirection = EditorPrefs.GetInt($"NPC_{_controller.gameObject.GetInstanceID()}", 0);

        // toolBarOptions[0] = DIRECTION.UP.ToString();
        // toolBarOptions[1] = DIRECTION.DOWN.ToString();
        // toolBarOptions[2] = DIRECTION.LEFT.ToString();
        // toolBarOptions[3] = DIRECTION.RIGHT.ToString();

        // UpdateStartLookDirection();
    }

    private void SaveOption()
    {
        EditorPrefs.SetInt($"NPC_{_controller.gameObject.GetInstanceID()}", toolBarDirection);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying)return;

        // EditorGUILayout.Space();
        // DrawHorizontalLine();
        // EditorGUILayout.Space();

        // GUILayout.Label("Look Direction", EditorStyles.boldLabel);

        // GUILayout.BeginHorizontal();

        // toolBarDirection = GUILayout.Toolbar(toolBarDirection, toolBarOptions, GUILayout.Height(35));

        // UpdateStartLookDirection();

        // SaveOption();

        // GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        if (_controller.Data == null)return;

        if (GUILayout.Button("Set Data", GUILayout.Height(35)))_controller.SetData();
    }

    private void UpdateStartLookDirection()
    {
        switch (toolBarDirection)
        {
            case 0:
                _controller.StartLookDirection = DIRECTION.UP;
                break;

            case 1:
                _controller.StartLookDirection = DIRECTION.DOWN;
                break;

            case 2:
                _controller.StartLookDirection = DIRECTION.LEFT;
                break;

            case 3:
                _controller.StartLookDirection = DIRECTION.RIGHT;
                break;
        }
    }

    private void OnSceneGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            Handles.color = Color.white;
            Handles.DrawWireArc(_fov.transform.position, Vector3.up, Vector3.forward, 360, _controller.ViewRadius);

            _viewAngleA = _fov.DirFromAngle(-_controller.ViewAngle / 2 + _controller.GetLookDirection(_controller.StartLookDirection), false);
            _viewAngleB = _fov.DirFromAngle(_controller.ViewAngle / 2 + _controller.GetLookDirection(_controller.StartLookDirection), false);

            Handles.DrawLine(_fov.transform.position, _fov.transform.position + _viewAngleA * _controller.ViewRadius);
            Handles.DrawLine(_fov.transform.position, _fov.transform.position + _viewAngleB * _controller.ViewRadius);
        }
        else
        {
            if (_controller.Data == null)return;

            if (!_controller.Data.DetectPlayer)return;

            Handles.color = Color.red;

            foreach (Transform visibleTarget in _fov.VisibleTargets)
            {
                Handles.DrawLine(_fov.transform.position, visibleTarget.position);
            }
        }
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}