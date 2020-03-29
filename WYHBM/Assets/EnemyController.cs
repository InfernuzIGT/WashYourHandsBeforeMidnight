using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Positions")]
    public Vector3[] positions;

    [Header("Colliders")]
    public BoxCollider NPCBox;
    public CapsuleCollider CHABox;

    public DialogManager dialogManager;
    public GameObject enemy;
    public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(MovementAgent());
    }

    private IEnumerator MovementAgent()
    {
        ChangeDestination();
        yield return new WaitForSeconds(6f);
        yield return MovementAgent();
    }

    public void ChangeDestination()
    {
        int randomValue = Random.Range(0, positions.Length);
        agent.SetDestination(positions[randomValue]);
    }

    private void OnTriggerEnter(Collider NPCBox)
    {
        if (NPCBox == CHABox)
        {
            dialogManager.isTriggerArea = true;
            agent.isStopped = true;
        }
    }
    
    private void OnTriggerExit(Collider NPCBox)
    {
        if (NPCBox == CHABox)
        {
            dialogManager.isTriggerArea = false;
            agent.isStopped = false;
        }
    }
}