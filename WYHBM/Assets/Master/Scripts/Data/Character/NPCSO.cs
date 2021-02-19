using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Characters/NPC", order = 2)]
public class NPCSO : CharacterSO
{
    [Header("NPC")]
    [SerializeField] private InteractionData[] _data = null;

    [Header("Combat")]
    [SerializeField] private bool _canCombat = false;
    [SerializeField, PreviewTexture(64)] private Sprite _spriteCorpse = null;
    [SerializeField] private List<Enemy> combatEnemies;

    [Header("IA")]
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _canPatrol = true;
    [SerializeField, Range(0f, 10f)] private float _speedMovement = 5;
    [SerializeField, Range(0f, 360f)] private float _speedRotation = 180;
    [SerializeField] private bool _useRandomPosition = false;
    [SerializeField, Range(0f, 10f)] private float _waitTime = 5;

    [Header("Field of View")]
    [SerializeField] private bool _detectPlayer = false;
    [SerializeField, Range(0f, 3f)] private float _timeToDetect = 1;

    // Properties
    public InteractionData[] Data { get { return _data; } }

    public Sprite SpriteCorpse { get { return _spriteCorpse; } }
    public List<Enemy> CombatEnemies { get { return combatEnemies; } }
    public bool CanCombat { get { return _canCombat; } }
    public bool CanMove { get { return _canMove; } }
    public bool CanPatrol { get { return _canPatrol; } }
    public bool UseRandomPosition { get { return _useRandomPosition; } }
    public float WaitTime { get { return _waitTime; } }
    public float SpeedMovement { get { return _speedMovement; } }
    public float SpeedRotation { get { return _speedRotation; } }
    public bool DetectPlayer { get { return _detectPlayer; } }
    public float TimeToDetect { get { return _timeToDetect; } }

}