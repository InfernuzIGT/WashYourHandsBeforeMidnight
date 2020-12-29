using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC", order = 0)]
public class NPCSO : CharacterSO
{
    [Header("NPC")]
    [SerializeField] private InteractionData[] _data = null;
    
    [Header("IA")]
    [SerializeField] private bool _canMove = true;
    [SerializeField] private bool _useRandomPosition = true;
    [SerializeField, Range(0f, 10f)] private float _waitTime = 5;
    [SerializeField, Range(0f, 10f)] private float _speed = 5;
    [Space]
    [SerializeField] private bool _canDetectPlayer = false;
    [SerializeField, Range(0f, 50f)] private float _viewRadius = 10;

    [Header("DEPRECATED")]
    public List<Enemy> combatEnemies;

    // Properties
    public InteractionData[] Data { get { return _data; } }

    public bool CanMove { get { return _canMove; } }
    public bool UseRandomPosition { get { return _useRandomPosition; } }
    public float WaitTime { get { return _waitTime; } }
    public float Speed { get { return _speed; } }
    public bool CanDetectPlayer { get { return _canDetectPlayer; } }
    public float ViewRadius { get { return _viewRadius; } }
   
}