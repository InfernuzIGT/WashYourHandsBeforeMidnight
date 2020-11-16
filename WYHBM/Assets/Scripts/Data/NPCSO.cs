using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC", order = 0)]
public class NPCSO : ScriptableObject
{
    public new string name;
    [Space]
    public TextAsset dialogDD;

    [Header("Emotion Icons")]
    [PreviewTexture(48), SerializeField] private Sprite iconNone = null;
    [PreviewTexture(48), SerializeField] private Sprite iconHappy = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSad = null;
    [PreviewTexture(48), SerializeField] private Sprite iconAngry = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSurprise = null;
    [PreviewTexture(48), SerializeField] private Sprite iconCurious = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSleep = null;

    [Header("DEPRECATED")]
    public NPC_INTERACTION_TYPE interactionType;
    public DialogSO dialog;
    public List<Enemy> combatEnemies;

    public Sprite GetIcon(EMOTION emotion)
    {
        switch (emotion)
        {
            case EMOTION.None:
                return iconNone;

            case EMOTION.Happy:
                return iconHappy;

            case EMOTION.Sad:
                return iconSad;

            case EMOTION.Angry:
                return iconAngry;

            case EMOTION.Surprise:
                return iconSurprise;

            case EMOTION.Curious:
                return iconCurious;

            case EMOTION.Sleep:
                return iconSleep;

        }

        return null;
    }
}