using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaypointController))]
public class WaypointControllerEditor : Editor
{
    private WaypointController _controller;

    private void OnEnable()
    {
        _controller = target as WaypointController;
    }

    private void OnSceneGUI()
    {
        if (_controller.showDebug && _controller.positions.Length != 0)
        {
            Handles.color = _controller.colorLine;
            Handles.DrawAAPolyLine(_controller.positions);

            for (int i = 0; i < _controller.positions.Length; i++)
            {
                _controller.positions[i] = Handles.PositionHandle(_controller.positions[i], Quaternion.identity);
                Handles.Label(_controller.positions[i], i.ToString(), EditorStyles.whiteLargeLabel);
            }
        }
    }

}