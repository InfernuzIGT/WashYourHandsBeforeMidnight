using UnityEngine;

[CreateAssetMenu(fileName = "New WorldConfig", menuName = "Config/WorldConfig", order = 0)]
public class WorldConfig : ScriptableObject
{
    [Header("General")]
    public float fovTime = 1f;
    public float randomIdleTime = 10f;

    [Header("Fade")]
    public float fadeCutsceneDuration = 1;
    public float fadeDuration = 1f;
    public float fadeDelay = 2;

    [Header("References")]
    public InteractionCorpse interactionCorpse;

    [Header("Text")]
    public float textTimeStart = 0.5f;
    public float textTimeSpeed = 0.025f;

    [Header("Ground Textures")]
    [PreviewTexture(48, FieldType.Texture)] public Texture textureDefault;
    [PreviewTexture(48, FieldType.Texture)] public Texture textureGrass;
    [PreviewTexture(48, FieldType.Texture)] public Texture textureDirt;
    [PreviewTexture(48, FieldType.Texture)] public Texture textureWood;
    [PreviewTexture(48, FieldType.Texture)] public Texture textureCement;
    [PreviewTexture(48, FieldType.Texture)] public Texture textureCeramic;

    [Header("Layers")]
    public LayerMask layerFOVTarget;
    public LayerMask layerFOVObstacle;
    [Space]
    public LayerMask layerGround;
    public LayerMask layerClimbable;
    public LayerMask layerNPC;
    [Space]
    public LayerMask layerOcclusionMask;
}