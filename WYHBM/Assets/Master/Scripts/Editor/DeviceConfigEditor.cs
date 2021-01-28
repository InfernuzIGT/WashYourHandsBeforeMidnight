using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DeviceConfig))]
public class DeviceConfigEditor : Editor
{
	private DeviceConfig _controller;

	private void OnEnable()
	{
		_controller = target as DeviceConfig;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();
		DrawHorizontalLine();
		EditorGUILayout.Space();

		if (GUILayout.Button("Update Dictionary"))
		{
			_controller.UpdateDictionary();
		}
	}

	private void DrawHorizontalLine()
	{
		Rect rect = EditorGUILayout.GetControlRect(false, 1);
		rect.height = 1;
		EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
	}

}