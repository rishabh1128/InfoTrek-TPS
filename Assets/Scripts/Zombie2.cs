using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //added for NavMeshAgent

public class Zombie2 : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private NavMeshAgent zombieAgent;
    [SerializeField] private Transform lookPoint;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Camera attackingRayCastArea;
    [SerializeField] private GameObject goreEffect;

    [Header("Zombie health and dmg")]
    [SerializeField] private float zombieHealth = 100f;
    private float curHealth;
    [SerializeField] private float giveDmg = 2.5f;
    [SerializeField] private float timeBetweenAttack = 1f;
    private bool previouslyAttacked;

   
    /*[SerializeField] private float zombieSpeed = 3f;*/
    
    [Header("Zombie states")]
    [SerializeField] private float visionRadius = 10f;
    [SerializeField] private float attackRadius = 1.5f;
    public bool canSeePlayer;
    public bool canAttackPlayer;

    [Header("Zombie animations")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        zombieAgent = GetComponent<NavMeshAgent>();
        curHealth = zombieHealth;
    }

    private void Update()
    {
        canSeePlayer = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        canAttackPlayer = Physics.CheckSphere(transform.position, attackRadius, playerLayer);

        if (!canSeePlayer && !canAttackPlayer)
        {
            Idle();
        }
        else if (canSeePlayer && !canAttackPlayer)
        {
            Pursue();
        }
        else
        {
            Attack();
        }
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
        animator.SetBool("Idle", true);
        animator.SetBool("Running", false);
    }

    private void Pursue()
    {
        if (zombieAgent.SetDestination(playerBody.position))
        {
            animator.SetBool("Running", true);
            animator.SetBool("Attacking", false);
            animator.SetBool("Idle", false);
        }
        
    }

    private void Attack()
    {
        zombieAgent.SetDestination(transform.position); //done to stop zombie action and look at the look point at player feet (instead of player body to avoid zombie rotating along x axis when player jumps)
        transform.LookAt(lookPoint);

        if (!previouslyAttacked)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(attackingRayCastArea.transform.position, attackingRayCastArea.transform.forward, out hitInfo, attackRadius))
            {
                Debug.Log("Attacking" + hitInfo.transform.name);

                Player player = hitInfo.transform.GetComponent<Player>();

                if (player != null)
                {
                    player.playerHitDmg(giveDmg);
                    GameObject gore = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(gore, 2.5f);
                }

                animator.SetBool("Running", false);
                animator.SetBool("Attacking", true);
            }
            previouslyAttacked = true;
            Invoke(nameof(ActivateAttacking), timeBetweenAttack);
        }
    }

    private void ActivateAttacking()
    {
        previouslyAttacked = false;
    }

    public void zombieHitDmg(float takeDmg)
    {
        curHealth -= takeDmg;
        if (curHealth <= 0)
        {
            animator.SetBool("Dying", true);
            zombieDeath();
        }
    }

    private void zombieDeath()
    {
        zombieAgent.SetDestination(transform.position);
        /*zombieSpeed = 0f;*/
        attackRadius = 0f;
        visionRadius = 0f;
        canAttackPlayer = false;
        canSeePlayer = false;
        //necessary to allow death animation to play

        Destroy(gameObject, 5f);
    }
}
