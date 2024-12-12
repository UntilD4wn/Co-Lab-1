using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target; 
    public GameObject player;
    public float chaseRange = 10f; 
    private NavMeshAgent agent; 

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= chaseRange)
            {
                Debug.Log("Player is close. Targetting player");
                agent.isStopped = false;
                target = player.transform;
            }
        else {
            GameObject echoTarget = GameObject.FindGameObjectWithTag("TargetPoint");
            if (echoTarget == null){
                Debug.Log("No echo point. Stopped moving");
                agent.isStopped = true;
            }
            else 
            {
                Debug.Log("Echo point exists. Targetting point");
                agent.isStopped = false;
                target = echoTarget.transform;
            }
        }

        if (target != null)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else 
        {
            agent.isStopped = true;
        }
    }
}