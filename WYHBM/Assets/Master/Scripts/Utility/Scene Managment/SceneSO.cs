using UnityEngine;
// using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "New Scene", menuName = "Scene/Scene")]
public class SceneSO : ScriptableObject
{
    [Header("Information")]
    [TextArea] public string description;
    [Space]
    public SceneReference sceneMain;
    public SceneReference[] scenesAdditive;

    // TODO Mariano: Review

    // [Header("Sounds")]
    // public AudioClip music;
    // [Range(0.0f, 1.0f)]
    // public float musicVolume;

    // [Header("Visuals")]
    // public PostProcessProfile postprocess;
}