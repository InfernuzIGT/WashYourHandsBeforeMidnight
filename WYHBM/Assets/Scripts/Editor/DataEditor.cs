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

[Serializable]
public struct NoteData
{
    public int id;
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
    private NoteData[] _noteData;
    private JSONConverter _jsonConverter = new JSONConverter();

    private List<Dialog> listNone = new List<Dialog>();
    private List<Dialog> listReady = new List<Dialog>();
    private List<Dialog> listInProgress = new List<Dialog>();
    private List<Dialog> listCompleted = new List<Dialog>();
    private List<DialogData> listDialogData = new List<DialogData>();

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
        _serializedObject.Update();

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
        Debug.Log($"<b>[DATA EDITOR] </b> Loading {dataType}..");

        switch (dataType)
        {
            case DataType.None:
                break;

            case DataType.DialogData:
                _dialogData = _jsonConverter.GetData<DialogData>(string.Format("{0}_{1}", dataType.ToString(), language.ToString()), SuccessData, FailData);
                break;

            case DataType.NoteData:
                _noteData = _jsonConverter.GetData<NoteData>(string.Format("{0}_{1}", dataType.ToString(), language.ToString()), SuccessData, FailData);
                break;

                // case DataType.UIData:
                // break;
        }
    }

    private void ClearData()
    {
        dialogSO = null;
        noteSO = null;

        _dialogData = null;
        _noteData = null;
    }

    private void SuccessData()
    {
        Debug.Log($"<color=green><b>[DATA EDITOR] </b></color> {dataType} Loaded. Wait a few seconds to Convert Data.");
    }

    private void FailData()
    {
        Debug.Log($"<color=red><b>[DATA EDITOR] </b></color> Error loading {dataType}.");
    }

    private void ConvertData()
    {
        Debug.Log($"<b>[DATA EDITOR] </b> Converting data..");

        switch (dataType)
        {
            case DataType.None:
                break;

            case DataType.DialogData:

                if (_dialogData == null)
                {
                    Debug.Log($"<color=red><b>[DATA EDITOR] </b></color> Missing {dataType}.");
                    return;
                }

                for (int i = 0; i < dialogSO.Length; i++)
                {
                    ConvertData(dialogSO[i]);
                }
                break;

            case DataType.NoteData:

                if (_noteData == null)
                {
                    Debug.Log($"<color=red><b>[DATA EDITOR] </b></color> Missing {dataType}.");
                    return;
                }

                for (int i = 0; i < noteSO.Length; i++)
                {
                    ConvertData(noteSO[i]);
                }
                break;

                // case DataType.UIData:
                // break;
        }

        Debug.Log($"<color=green><b>[DATA EDITOR] </b></color> {dataType} Converted.");
    }

    private void ConvertData(DialogSO currentDialogSO)
    {
        listNone.Clear();
        listReady.Clear();
        listInProgress.Clear();
        listCompleted.Clear();
        listDialogData.Clear();

        Debug.Log($"<b>[DATA EDITOR] </b> Reading ID {currentDialogSO.dialogId}..");

        for (int i = 0; i < _dialogData.Length; i++)
        {
            if (_dialogData[i].id == currentDialogSO.dialogId)
            {
                listDialogData.Add(_dialogData[i]);
            }
        }

        Dialog tempDialog = new Dialog();

        for (int i = 0; i < listDialogData.Count; i++)
        {
            tempDialog.isPlayer = listDialogData[i].isPlayer;
            tempDialog.sentence = listDialogData[i].sentence;

            switch (listDialogData[i].type)
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

    private void ConvertData(NoteSO currentNoteSO)
    {
        Debug.Log($"<b>[DATA EDITOR] </b> Reading ID {currentNoteSO.noteId}..");

        for (int i = 0; i < _noteData.Length; i++)
        {
            if (_noteData[i].id == currentNoteSO.noteId)
            {
                currentNoteSO.noteSentences = _noteData[i].sentence;
                break;
            }
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