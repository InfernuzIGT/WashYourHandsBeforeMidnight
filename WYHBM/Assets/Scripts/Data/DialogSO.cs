using System;
using UnityEngine;

[Serializable]
public struct Dialog
{
    public bool isPlayer;
    [TextArea] public string sentence;
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog", order = 0)]
public class DialogSO : ScriptableObject
{
    [PreviewTexture(48)] public Sprite icon;
    public int dialogId;
    [Space]
    public Dialog[] dialogNone;
    public Dialog[] dialogReady;
    public Dialog[] dialogInProgress;
    public Dialog[] dialogCompleted;
    
    [ContextMenu("Clear Data")]
    public void ClearData()
    {
        dialogNone = null;
        dialogReady = null;
        dialogInProgress = null;
        dialogCompleted = null;
    }
}