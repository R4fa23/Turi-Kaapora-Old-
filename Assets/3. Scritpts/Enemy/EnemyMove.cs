using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public SOEnemy soEnemy;
    NavMeshAgent navMeshAgent;
    Transform player;
    bool detected;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.speed = soEnemy.soEnemyMove.vel;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < soEnemy.soEnemyMove.distanceDetectation && !detected)
        {
            soEnemy.state = SOEnemy.State.WALKING;
            detected = true;
            navMeshAgent.SetDestination(player.position);
            soEnemy.soEnemyMove.MoveStart();
        }        

        if(soEnemy.state == SOEnemy.State.WALKING)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }

}
