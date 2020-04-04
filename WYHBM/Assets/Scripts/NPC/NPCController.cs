using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    public bool isAlive = true;

    [Header("Movement")]
    public Transform[] movePositions;
    [Range(0f, 10f)]
    public float waitTime = 5;

    private AnimatorController _animatorController;
    private DialogController _dialogController;

    private Interaction _interaction;
    private NavMeshAgent _agent;
    private WaitForSeconds _waitForSeconds;
    private int _randomValue;

    private string _tagPlayer = "Player"; // TODO Mariano: MOVE

    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
        _dialogController = GetComponent<DialogController>();
        _interaction = GetComponentInChildren<Interaction>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _agent.updateRotation = false;

        _waitForSeconds = new WaitForSeconds(waitTime);
        StartCoroutine(MovementAgent());
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animatorController.Movement(_agent.desiredVelocity.x, _agent.desiredVelocity.z);
        }
        else
        {
            _animatorController.Movement(0, 0);
        }
    }

    private IEnumerator MovementAgent()
    {
        while (isAlive)
        {
            ChangeDestination();
            yield return _waitForSeconds;
        }
    }

    private void ChangeDestination()
    {
        _randomValue = Random.Range(0, movePositions.Length);
        _agent.SetDestination(movePositions[_randomValue].position);
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_tagPlayer))
        {
            ChangeState(true);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(_tagPlayer))
        {
            ChangeState(false);
        }
    }

    private void ChangeState(bool state)
    {
        _agent.isStopped = state;

        _animatorController.Movement(0, 0);

        if (_dialogController != null)
        {
            _dialogController.ChangeState(state);
        }
    }
}