using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Note", order = 0)]
public class NoteSO : ScriptableObject
{
    [TextArea()]
    public string noteSentences;

}
