using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum States
    {
        Patrol,
        Attack,
        Chase,
    }

    public Transform player;
    public Transform[] points;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float attackRange = 2f;
    public int damage = 10;
    public int distance = 50;

    private NavMeshAgent nav;
    private int destPoint = 0;
    private States state;
    bool playerInRange;
    bool attack;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        state = States.Patrol;
        GoToNextPoint();
        playerInRange = false;
        attack = false;
    }

    void Update()
    {
        if (state == States.Patrol)
        {
            Patrol();
        }

        if (state == States.Attack)
        {
            Attack();
        }

        if (state == States.Chase)
        {
            Chase();
        }

    }

    void Patrol()
    {
        nav.speed = patrolSpeed;

        if (!nav.pathPending && nav.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }

        if (playerInRange)
        {
            state = States.Chase;
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
        nav.speed = chaseSpeed;
        nav.SetDestination(player.position);

        if(Vector3.Distance(transform.position, player.position) < distance)
        {
            attack = true;
        }
        //check for player being close
        //if true, change to attack
    }

    void Attack()
    {
        if (attack == true)
        {
            Debug.Log("Attaked player");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Chase();
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }
}
