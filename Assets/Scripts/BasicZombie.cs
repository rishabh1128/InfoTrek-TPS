using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //added for NavMeshAgent

public class BasicZombie : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private NavMeshAgent zombieAgent;
    [SerializeField] private Transform lookPoint;

    [Header("Zombie walk")]
    public GameObject[] walkpoints;
    private int currZombiePosition = 0; //denotes current walkpoint in the array
    [SerializeField] private float zombieSpeed = 3f;
    private float walkPointRadius = 2f;

    [Header("Zombie states")]
    [SerializeField] private float visionRadius=10f;
    [SerializeField] private float attackRadius=2f;
    public bool canSeePlayer;
    public bool canAttackPlayer;

    private void Awake()
    {
        zombieAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        canSeePlayer = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        canAttackPlayer = Physics.CheckSphere(transform.position, attackRadius, playerLayer);

        if(!canSeePlayer && !canAttackPlayer)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if(Vector3.Distance(walkpoints[currZombiePosition].transform.position,transform.position) < walkPointRadius) //if reached current walkpoint, change walkpoint
        {
            currZombiePosition = Random.Range(0, walkpoints.Length);
            if(currZombiePosition >= walkpoints.Length)
            {
                currZombiePosition = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, walkpoints[currZombiePosition].transform.position, Time.deltaTime * zombieSpeed);
        //TODO: change zombie facing


    }
}
