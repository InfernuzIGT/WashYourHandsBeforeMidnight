using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : Character, IInteractable
{
    public bool canMove = true;
    public bool isEnemy = false;

    [Header("Movement")]
    public Transform[] movePositions;
    [Range(0f, 10f)]
    public float waitTime = 5;

    private AnimatorController _animatorController;
    private InteractionDialog _interactionDialog;

    private NavMeshAgent _agent;
    private WaitForSeconds _waitForSeconds;
    private int _randomValue;

    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
        _interactionDialog = GetComponentInChildren<InteractionDialog>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (!_agent.isOnNavMesh && canMove)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> NPC '{gameObject.name}' isn't on NavMesh!");
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
        }
    }

    private IEnumerator MovementAgent()
    {
        while (canMove)
        {
            ChangeDestination();
            yield return _waitForSeconds;
        }
    }

    private void ChangeDestination()
    {
        if (!_agent.isStopped)
        {
            _randomValue = Random.Range(0, movePositions.Length);
            _agent.SetDestination(movePositions[_randomValue].position);
        }
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            EventController.AddListener<EnableMovementEvent>(OnStopMovement);

            if (isEnemy && GameManager.Instance.currentAmbient != AMBIENT.Combat)
            {
                GameManager.Instance.ChangeAmbient(AMBIENT.Combat);
            }
            else
            {
                _interactionDialog?.Execute(true);
            }
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(GameData.Instance.gameConfig.tagPlayer))
        {
            EventController.RemoveListener<EnableMovementEvent>(OnStopMovement);

            _interactionDialog?.Execute(false);
        }
    }

    private void OnStopMovement(EnableMovementEvent evt)
    {
        if (canMove)_agent.isStopped = !evt.canMove;

        _animatorController?.Movement(Vector3.zero);
    }
}