﻿using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour, IInteractable
{
    [Header("Movement")]
    public bool canMove = true;
    public WaypointController waypoints;
    public bool useRandomPosition = true;
    [Range(0f, 10f)]
    public float waitTime = 5;

    private WorldAnimator _animatorController;
    private InteractionNPC _interactionNPC;

    private NavMeshAgent _agent;
    private WaitForSeconds _waitForSeconds;
    private bool _isMoving;
    private int _positionIndex = 0;

    private void Awake()
    {
        _animatorController = GetComponent<WorldAnimator>();
        _interactionNPC = GetComponentInChildren<InteractionNPC>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (!_agent.isOnNavMesh && canMove)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> NPC '{gameObject.name}' isn't on NavMesh!");
            return;
        }

        if (!canMove || waypoints == null)
        {
            return;
        }

        _agent.updateRotation = false;

        _waitForSeconds = new WaitForSeconds(waitTime);
        StartCoroutine(MovementAgent());
    }

    private void Update()
    {
        if (_agent.isOnNavMesh && canMove)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animatorController.Movement(_agent.desiredVelocity);
        }
        else
        {
            _animatorController.Movement(Vector3.zero);
            _isMoving = false;
        }
    }

    private IEnumerator MovementAgent()
    {
        while (canMove)
        {
            ChangeDestination();

            while (_isMoving)
            {
                yield return null;
            }

            yield return _waitForSeconds;
        }
    }

    private void ChangeDestination()
    {
        if (!_agent.isStopped)
        {
            if (useRandomPosition)
            {
                _positionIndex = Random.Range(0, waypoints.positions.Length);
            }
            else
            {
                _positionIndex = _positionIndex < waypoints.positions.Length - 1 ? _positionIndex + 1 : 0;
            }

            _agent.SetDestination(waypoints.positions[_positionIndex]);
            _isMoving = true;
        }
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<EnableMovementEvent>(OnStopMovement);

            _interactionNPC.Execute(true, this);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);

            _interactionNPC.Execute(false, this);
        }
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        if (canMove)_agent.isStopped = !evt.canMove;

        _animatorController?.Movement(Vector3.zero);
    }

    public void Kill()
    {
        EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);
        Destroy(gameObject);
    }
}