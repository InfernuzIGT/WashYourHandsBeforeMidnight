using UnityEngine;

public enum EMOTION
{
    None = 0,
    Happy = 1,
    Sad = 2,
    Angry = 3,
    Surprise = 4,
    Curious = 5,
    Sleep = 6,
    Special1 = 7,
    Special2 = 8
}

public class CharacterSO : ScriptableObject
{
    [Header("Character")]
    [SerializeField] private string _name = "-";
    [SerializeField, PreviewTexture(64)] private Sprite _sprite = null;
    [SerializeField] private AnimatorOverrideController _animatorController = null;
    [Space(24)]
    [PreviewTexture(48), SerializeField] private Sprite iconNone = null;
    [PreviewTexture(48), SerializeField] private Sprite iconHappy = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSad = null;
    [PreviewTexture(48), SerializeField] private Sprite iconAngry = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSurprise = null;
    [PreviewTexture(48), SerializeField] private Sprite iconCurious = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSleep = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSpecial1 = null;
    [PreviewTexture(48), SerializeField] private Sprite iconSpecial2 = null;

    // Properties
    public string Name { get { return _name; } }
    public Sprite Sprite { get { return _sprite; } }
    public AnimatorOverrideController AnimatorController { get { return _animatorController; } }

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

            case EMOTION.Special1:
                return iconSpecial1;

            case EMOTION.Special2:
                return iconSpecial2;
        }

        return null;
    }
}