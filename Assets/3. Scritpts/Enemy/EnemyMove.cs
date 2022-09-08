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
    bool firstEnable;

    void Start()
    {
        lastState = SOEnemy.State.STOPPED;
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.SetDestination(transform.position);
        soEnemy = GetComponent<EnemyManager>().soEnemy;
        navMeshAgent.speed = soEnemy.vel;
        OnEnable();
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < soEnemy.distanceDetectation && !detected)
        {
            Detect();
        }        

        if(soEnemy.state == SOEnemy.State.WALKING)
        {
            navMeshAgent.SetDestination(player.position);
            if(lastState != soEnemy.state)
            {
                lastState = soEnemy.state;
                soEnemy.MoveStart();
            }
            
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
        
    }

    void Detect()
    {
        soEnemy.state = SOEnemy.State.WALKING;
        detected = true;
        navMeshAgent.SetDestination(player.position);
        soEnemy.MoveStart();
    }

    void OnEnable()
    {
        if(firstEnable)
        {
            soEnemy.SummonEvent.AddListener(Detect);
        }
        firstEnable = true;
    }
    void OnDisable()
    {
        soEnemy.SummonEvent.RemoveListener(Detect);
    }

}
