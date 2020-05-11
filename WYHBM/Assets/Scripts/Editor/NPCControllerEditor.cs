using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCController))]
public class NPCControllerEditor : Editor
{
    private NPCController _controller;

    private Vector3 _viewAngleA;
    private Vector3 _viewAngleB;

    private void OnEnable()
    {
        _controller = target as NPCController;
    }

    private void OnSceneGUI()
    {
        if (_controller == null || _controller.VisibleTargets == null)return;

        Handles.color = Color.white;
        Handles.DrawWireArc(_controller.transform.position, Vector3.up, Vector3.forward, 360, _controller.viewRadius);

        _viewAngleA = _controller.DirFromAngle(-_controller.ViewAngle / 2, false);
        _viewAngleB = _controller.DirFromAngle(_controller.ViewAngle / 2, false);

        Handles.DrawLine(_controller.transform.position, _controller.transform.position + _viewAngleA * _controller.viewRadius);
        Handles.DrawLine(_controller.transform.position, _controller.transform.position + _viewAngleB * _controller.viewRadius);

        Handles.color = Color.red;

        foreach (Transform visibleTarget in _controller.VisibleTargets)
        {
            Handles.DrawLine(_controller.transform.position, visibleTarget.position);
        }
    }

}