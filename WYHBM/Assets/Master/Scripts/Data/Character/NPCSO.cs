using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Characters/NPC", order = 2)]
public class NPCSO : CharacterSO
{
    [Header("NPC")]
    [SerializeField] private InteractionData[] _data = null;

    [Header("IA")]
    [SerializeField] private bool _canMove = true;
    [SerializeField, Range(0f, 10f)] private float _speedMovement = 5;
    [SerializeField, Range(0f, 360f)] private float _speedRotation = 180;
    [SerializeField] private bool _useRandomPosition = true;
    [SerializeField, Range(0f, 10f)] private float _waitTime = 5;
    
    [Header("Field of View")]
    [SerializeField] private bool _detectPlayer = false;
    [SerializeField, Range(0f, 50f)] private float _viewRadius = 7.5f;
    [SerializeField, Range(0f, 360f)] private float _viewAngle = 135;

    [Header("DEPRECATED")]
    public List<Enemy> combatEnemies;

    // Properties
    public InteractionData[] Data { get { return _data; } }

    public bool CanMove { get { return _canMove; } }
    public bool UseRandomPosition { get { return _useRandomPosition; } }
    public float WaitTime { get { return _waitTime; } }
    public float SpeedMovement { get { return _speedMovement; } }
    public float SpeedRotation { get { return _speedRotation; } }
    public bool DetectPlayer { get { return _detectPlayer; } }
    public float ViewRadius { get { return _viewRadius; } }
    public float ViewAngle { get { return _viewAngle; } }

}