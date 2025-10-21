using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Enemy : MonoBehaviour
{
    enum States
    {
        Patrol,
        Attack,
    }


    public Transform player;
    private NavMeshAgent agent;
    //public GameObject deathText;

    public Transform[] points;
    private NavMeshAgent nav;
    private int destPoint;
    float sightRange, attackRange;
    bool playerInSight, playerInAttackRange;
    public LayerMask playerLayer;

    States state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        state = States.Patrol;
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        Chase();
        if (state == States.Attack)
        {
            //execute attack code

            //agent.destination = player.position;

            Attack();
        }

        if (state == States.Patrol)
        {
            // check for player nearby
            if (!nav.pathPending && nav.remainingDistance < 0.5f && !playerInSight && !playerInAttackRange)
            {
                GoToNextPoint();
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            state = States.Attack;
        }
    }

    void GoToNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }

        nav.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    void Chase()
    {
        nav.SetDestination(player.transform.position);
    }

    void Attack()
    {

    }

}
