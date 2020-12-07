using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC", order = 0)]
public class NPCSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] private string _name = "(Enter Name)";
    [Space]
    [SerializeField] private InteractionData[] _data = null;

    [Header("IA")]
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _useRandomPosition = true;
    [SerializeField, Range(0f, 10f)] private float _waitTime = 5;
    [SerializeField, Range(0f, 10f)] private float _speed = 5;
    [Space]
    [SerializeField] private bool _canDetectPlayer = false;
    [SerializeField, Range(0f, 50f)] private float _viewRadius = 10;

    [Header("Art")]
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

    [Header("DEPRECATED")]
    public List<Enemy> combatEnemies;

    // Properties
    public string Name { get { return _name; } }
    public InteractionData[] Data { get { return _data; } }

    public bool CanMove { get { return _canMove; } }
    public bool UseRandomPosition { get { return _useRandomPosition; } }
    public float WaitTime { get { return _waitTime; } }
    public float Speed { get { return _speed; } }
    public bool CanDetectPlayer { get { return _canDetectPlayer; } }
    public float ViewRadius { get { return _viewRadius; } }

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

        }

        return null;
    }
}