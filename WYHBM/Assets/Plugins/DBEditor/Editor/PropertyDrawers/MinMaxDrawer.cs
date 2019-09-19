using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxValues))]
[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects)return 0f;
        // return base.GetPropertyHeight(property, label) + 16f;
        return base.GetPropertyHeight(property, label) - 16f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.isEditingMultipleObjects)return;

        Rect controlRect = EditorGUILayout.GetControlRect();
        float labelWidth = EditorGUIUtility.labelWidth;
        float floatFieldWidth = EditorGUIUtility.fieldWidth;
        float sliderWidth = controlRect.width - labelWidth - 2f * floatFieldWidth;
        float sliderPadding = 5f;

        Rect labelRect = new Rect(
            controlRect.x,
            controlRect.y,
            labelWidth,
            controlRect.height);

        Rect sliderRect = new Rect(
            controlRect.x + labelWidth + floatFieldWidth + sliderPadding,
            controlRect.y,
            sliderWidth - 2f * sliderPadding,
            controlRect.height);

        Rect minFloatFieldRect = new Rect(
            controlRect.x + labelWidth,
            controlRect.y,
            floatFieldWidth,
            controlRect.height);

        Rect maxFloatFieldRect = new Rect(
            controlRect.x + labelWidth + floatFieldWidth + sliderWidth,
            controlRect.y,
            floatFieldWidth,
            controlRect.height);

        EditorGUI.LabelField(labelRect, property.displayName);

        var minProperty = property.FindPropertyRelative("Min");
        var maxProperty = property.FindPropertyRelative("Max");
        var minmax = attribute as MinMaxAttribute ?? new MinMaxAttribute(0, 1);

        var min = minProperty.floatValue;
        var max = maxProperty.floatValue;

        min = Mathf.Clamp(EditorGUI.FloatField(minFloatFieldRect, min), minmax.Min, max);
        min = (int)min; // Convert to Int

        max = Mathf.Clamp(EditorGUI.FloatField(maxFloatFieldRect, max), min, minmax.Max);
        max = (int)max; // Convert to Int

        EditorGUI.MinMaxSlider(sliderRect, GUIContent.none, ref min, ref max, minmax.Min, minmax.Max);

        minProperty.floatValue = min;
        maxProperty.floatValue = max;
    }
}