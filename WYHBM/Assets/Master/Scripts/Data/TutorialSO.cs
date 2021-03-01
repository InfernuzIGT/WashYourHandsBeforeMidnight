using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Tutorial Data", menuName = "Tutorial", order = 0)]
public class TutorialSO : ScriptableObject
{
    [Header("Tutorial Data")]
    [PreviewTexture(48)] public Sprite image;
    public LocalizedString description;
}