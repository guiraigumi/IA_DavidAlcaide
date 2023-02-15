using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,
        Chasing,
        Travelling,
        Waiting,
        Attacking
    }

    State currentState;
    NavMeshAgent agent;

    public Transform[] destinationPoints;
    int destinationIndex = 0;

    
    public Transform player;

    [SerializeField]
    float visionRange;
    [SerializeField] 
    private float hitRange;
    [SerializeField] 

    private float wait = 5f;

    /*[SerializeField]
    [Range (0, 360)]

    float visionAngle;

    [SerializeField]
    LayerMask obstaclesMask;

    /*[SerializeField]
    float patrolRange = 10f;
    [SerializeField]
    Transform patrolZone;*/

    void Awake() {
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    void Start()
    {
        currentState = State.Patrolling;
        //destinationIndex = Random.Range(0, destinationPoints.Length);
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Chasing:
                Chase();
            break;
            default:
                Chase();
            break;
            case State.Waiting:
                Wait();
                break;
            case State.Attacking:
                Attack();
                break;
                /*case State.Travelling:
                Travel();
            break;*/

        }
    }

    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < 1)
        {
            if (destinationIndex < destinationPoints.Length)
            {
                destinationIndex += 1;
            }

            if (destinationIndex == destinationPoints.Length)
            {
                destinationIndex = 0;
            }

            currentState = State.Waiting;
        }

        if (Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        agent.destination = player.position;

        if (Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }
        if (Vector3.Distance(transform.position, player.position) < hitRange)
        {
            currentState = State.Attacking;
            Debug.Log("Ataque");
        }
    }

    void Wait()
    {

        wait -= Time.deltaTime;

        if (wait <= 0)
        {
            currentState = State.Patrolling;

            wait = 5;
        }
    }

    void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) > hitRange)
        {
            currentState = State.Chasing;
        }
    }

    void OnDrawGizmos()
    {
        {
            foreach (Transform point in destinationPoints)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(point.position, 1);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, visionRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, hitRange);//patrolZone.position, patrolRange);

        }
    }

    /*void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < 1)
        {
            //Te pilla un punto random entre 0 y un punto del array.
            destinationIndex = Random.Range(0, destinationPoints.Length);
        }

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }

    }

    /*void Patrol()
    {
        Vector3 randomPosition;
        if(RandomPoint(patrolZone.position, patrolRange, out randomPosition))
        {
            agent.destination = randomPosition;
            Debug.DrawRay(randomPosition, Vector3.up * 5, Color.blue, 5f);
        }

        if(FindTarget())
        {
            currentState = State.Chasing;
        }  

        currentState = State.Travelling;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 point)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 4, NavMesh.AllAreas))
        {
            point = hit.position;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    void Travel()
    {
        if(agent.remainingDistance <= 0.2)
        {
            currentState =State.Patrolling;
        }

        if(FindTarget())
        {
            currentState = State.Chasing;
        }  
    }*/




    /*bool FindTarget()
    {
        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            Vector3 directionToTarget = (player.position = transform.position).normalized;
            if(Vector3.Angle(transform.forward, directionToTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, player.position);
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstaclesMask))
                {
                    return true;
                }
            }
        }

        return false;
    }*/

}
