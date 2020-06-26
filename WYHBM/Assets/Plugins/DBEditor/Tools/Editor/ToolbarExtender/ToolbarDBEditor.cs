// using UnityEditor;
// using UnityEngine;

// namespace UnityToolbarExtender
// {
//     [InitializeOnLoad]
//     public class ToolbarDBEditor
//     {
//         private static string titleText = "Developer: Mariano N. Sosa";

//         private static GUIContent _guiContent = new GUIContent(EditorGUIUtility.FindTexture("d_Project"), "Open Database");
//         private static Rect _titlePos;

//         static ToolbarDBEditor()
//         {
//             ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
//             _titlePos = new Rect(50, 8, 50, 5);
//         }

//         private static void OnToolbarGUI()
//         {

//             if (GUILayout.Button(_guiContent, ToolbarStyles.commandButtonStyle))
//             {
//                 EditorApplication.ExecuteMenuItem("Tools/DBEditor");
//             }

//             GUILayout.Space(10);

//             GUI.Label(_titlePos, titleText, ToolbarStyles.normalStyle);

//             GUILayout.Space(20);

//             GUILayout.FlexibleSpace();
//         }

//         public static class ToolbarStyles
//         {
//             public static readonly GUIStyle commandButtonStyle;
//             public static readonly GUIStyle normalStyle;

//             static ToolbarStyles()
//             {
//                 commandButtonStyle = new GUIStyle("Command")
//                 {
//                     fontSize = 16,
//                     alignment = TextAnchor.MiddleCenter,
//                     imagePosition = ImagePosition.ImageAbove,
//                     fontStyle = FontStyle.Bold
//                 };

//                 normalStyle = new GUIStyle()
//                 {
//                     fontSize = 14,
//                     alignment = TextAnchor.MiddleLeft,
//                     font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
//                 };

//                 normalStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
//             }
//         }

//     }
// }