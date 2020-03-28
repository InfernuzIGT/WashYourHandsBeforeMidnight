using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header ("Positions")]
    public Vector3[] positions;
    [Space]
    [Header ("Colliders")]
    public BoxCollider NPCBox;
    public CapsuleCollider CHABox;
    [Space]

    public DialogManager dialogManager;
    public GameObject enemy;
    private bool isStopped = true;
    public NavMeshAgent agent;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(MovementAgent());
    }

    IEnumerator MovementAgent()
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

    void OnTriggerEnter(Collider NPCBox)
    {
        if (NPCBox == CHABox)
        {
            dialogManager.isTriggerArea = true;
            agent.isStopped = true;
        }
    }
    void OnTriggerExit(Collider NPCBox)
    {
        if (NPCBox == CHABox)
        {
            dialogManager.isTriggerArea = false;
            agent.isStopped = false;
        }
            
    }
}