using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "DEPRECATED/Note")]
public class NoteSO : ScriptableObject
{
    public int noteId;
    [Space]
    [TextArea()] public string noteSentences;

    [ContextMenu("Clear Data")]
    public void ClearData()
    {
        noteSentences = null;
    }
}