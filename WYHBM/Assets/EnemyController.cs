using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    static NavMeshAgent agent;
    public Vector3 velocityEnemy;

    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public GameObject enemy;
    public CapsuleCollider playerCollider;
    public BoxCollider enemyCollider;
    public bool isStopped = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(MovementAgent());
    }
    void Update()
    {
    }

    IEnumerator MovementAgent()
    {
        agent.SetDestination(pos1.position);
        yield return new WaitForSeconds(6f);

        agent.SetDestination(pos2.position);
        yield return new WaitForSeconds(6f);

        agent.SetDestination(pos3.position);
        yield return new WaitForSeconds(6f);
        yield return MovementAgent();
    }
    void OnTriggerEnter(Collider enemyCollider)
    {
        if (enemyCollider == playerCollider)
        {
            Debug.Log ($"<b> Hit </b>");
            this.GetComponent<NavMeshAgent>().isStopped = true;
        }
        else
        {
            Debug.Log ($"<b> Out of Hit </b>");
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }
}