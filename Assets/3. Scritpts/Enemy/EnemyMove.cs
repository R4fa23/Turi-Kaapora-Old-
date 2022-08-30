using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    SOEnemy soEnemy;
    NavMeshAgent navMeshAgent;
    Transform player;
    bool detected;
    bool initialMove;
    SOEnemy.State lastState;

    void Start()
    {
        lastState = SOEnemy.State.STOPPED;
        soEnemy = GetComponent<EnemyManager>().soEnemy;
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.speed = soEnemy.vel;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < soEnemy.distanceDetectation && !detected)
        {
            soEnemy.state = SOEnemy.State.WALKING;
            detected = true;
            navMeshAgent.SetDestination(player.position);
            soEnemy.MoveStart();
        }        

        if(lastState != soEnemy.state)
        {
            lastState = soEnemy.state;
            ChangeState();
        }
        
    }

    void ChangeState()
    {
        if(soEnemy.state == SOEnemy.State.WALKING)
        {
            navMeshAgent.SetDestination(player.position);
            soEnemy.MoveStart();
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }

}
