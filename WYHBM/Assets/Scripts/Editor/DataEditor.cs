using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct DialogData
{
    public int id;
    public DIALOG_TYPE type;
    public bool isPlayer;
    public string sentence;
}

public class DataEditor : EditorWindow
{
    public enum DataType
    {
        None = 0,
        DialogData = 1,
        NoteData = 2,
        // UIData = 3
    }

    // Data Editor Data
    public DataType dataType;
    public LANGUAGE language;
    public DialogSO[] dialogSO;
    public NoteSO[] noteSO;

    private DialogData[] _dialogData;
    private JSONConverter _jsonConverter = new JSONConverter();

    private GUIStyle _styleButtons;
    private Vector2 _scroll;
    private SerializedObject _serializedObject;
    private SerializedProperty _dialogProperty;
    private SerializedProperty _noteProperty;
    private SerializedProperty _uiProperty;

    [MenuItem("Tools/Data Editor")]
    static void CreateDataEditor()
    {
        var window = EditorWindow.GetWindow<DataEditor>();
        Texture2D iconTitle = EditorGUIUtility.Load("Clipboard")as Texture2D;
        GUIContent titleContent = new GUIContent("Data Editor", iconTitle);
        window.titleContent = titleContent;
        window.minSize = new Vector2(300, 250);
        window.Show();
    }

    private void OnGUI()
    {
        _styleButtons = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fontSize = 15, fixedHeight = 40 };

        EditorWindow target = this;
        _serializedObject = new SerializedObject(target);
        _dialogProperty = _serializedObject.FindProperty("dialogSO");
        _noteProperty = _serializedObject.FindProperty("noteSO");
        // _uiProperty = _serializedObject.FindProperty("");

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        EditorGUILayout.Space();
        dataType = (DataType)EditorGUILayout.EnumPopup("Data Type", dataType);

        EditorGUILayout.Space();
        language = (LANGUAGE)EditorGUILayout.EnumPopup("Language", language);
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        switch (dataType)
        {
            case DataType.None:
                break;

            case DataType.DialogData:
                EditorGUILayout.PropertyField(_dialogProperty, true);
                break;

            case DataType.NoteData:
                EditorGUILayout.PropertyField(_noteProperty, true);
                break;

                // case DataType.UIData:
                // break;
        }

        _serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        DrawHorizontalLine();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Load", _styleButtons))
        {
            LoadData();
        }

        if (GUILayout.Button("Clear", _styleButtons))
        {
            ClearData();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (GUILayout.Button("Convert", _styleButtons))
        {
            ConvertData();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // DrawSize();

        EditorGUILayout.EndScrollView();
    }

    private void LoadData()
    {
        Debug.Log($"<b>[DATA EDITOR] </b> Loading Data..");
        _dialogData = _jsonConverter.GetData<DialogData>(string.Format("{0}_{1}", dataType.ToString(), language.ToString()), SuccessData, FailData);
    }

    private void ClearData()
    {
        dialogSO = null;
        noteSO = null;
        _dialogData = null;
    }

    private void SuccessData()
    {
        Debug.Log($"<color=green><b>[DATA EDITOR] </b></color> Data Loaded. Wait a few seconds to Convert Data.");
    }

    private void FailData()
    {
        Debug.Log($"<color=red><b>[DATA EDITOR] </b></color> Error loading data.");
    }

    private void ConvertData()
    {
        Debug.Log($"<b>[DATA EDITOR] </b> Converting Data..");

        if (_dialogData == null)
        {
            Debug.Log($"<color=red><b>[DATA EDITOR] </b></color> Missing data.");
            return;
        }

        switch (dataType)
        {
            case DataType.None:
                break;

            case DataType.DialogData:
                for (int i = 0; i < dialogSO.Length; i++)
                {
                    ConvertData(dialogSO[i]);
                }
                _dialogData = null;
                break;

            case DataType.NoteData:
                break;

                // case DataType.UIData:
                // break;
        }

        Debug.Log($"<color=green><b>[DATA EDITOR] </b></color> Data Converted.");
    }

    private void ConvertData(DialogSO currentDialogSO)
    {
        Dialog tempDialog = new Dialog();
        List<Dialog> listNone = new List<Dialog>();
        List<Dialog> listReady = new List<Dialog>();
        List<Dialog> listInProgress = new List<Dialog>();
        List<Dialog> listCompleted = new List<Dialog>();
        List<DialogData> listDialogData = new List<DialogData>();

        for (int i = 0; i < _dialogData.Length; i++)
        {
            if (_dialogData[i].id == currentDialogSO.dialogId)
            {
                listDialogData.Add(_dialogData[i]);
            }
        }

        Debug.Log($"<b>[DATA EDITOR] </b> Reading ID {currentDialogSO.dialogId}..");

        for (int i = 0; i < listDialogData.Count; i++)
        {
            tempDialog.isPlayer = _dialogData[i].isPlayer;
            tempDialog.sentence = _dialogData[i].sentence;

            switch (_dialogData[i].type)
            {
                case DIALOG_TYPE.None:
                    listNone.Add(tempDialog);
                    break;
                case DIALOG_TYPE.Ready:
                    listReady.Add(tempDialog);
                    break;
                case DIALOG_TYPE.InProgress:
                    listInProgress.Add(tempDialog);
                    break;
                case DIALOG_TYPE.Completed:
                    listCompleted.Add(tempDialog);
                    break;
            }
        }

        currentDialogSO.dialogNone = listNone.ToArray();
        currentDialogSO.dialogReady = listReady.ToArray();
        currentDialogSO.dialogInProgress = listInProgress.ToArray();
        currentDialogSO.dialogCompleted = listCompleted.ToArray();
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