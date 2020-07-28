using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Note", order = 0)]
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