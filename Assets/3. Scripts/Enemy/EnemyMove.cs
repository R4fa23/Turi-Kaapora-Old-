using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [HideInInspector]
    public SOEnemy soEnemy;
    public SOPlayer soPlayer;
    SOSave soSave;
    NavMeshAgent navMeshAgent;
    Transform player;
    [HideInInspector]
    public bool detected;
    bool initialMove;
    SOEnemy.State lastState;
    bool firstEnable;
    [HideInInspector]
    public Vector3 firstLocal;
    bool alert;
    float attackConfirmed;
    
    

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        lastState = SOEnemy.State.STOPPED;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.SetDestination(transform.position);
        soEnemy = GetComponent<EnemyManager>().soEnemy;
        soSave = GetComponent<EnemyManager>().soSave;
        navMeshAgent.speed = soEnemy.vel;
        OnEnable();
        
    }

    void Update()
    {

        if(Vector3.Distance(transform.position, player.position) < soEnemy.distanceDetectation && !alert)
        {
            Detect();
        }        

        if(alert && !detected)
        {
            Vector3 dirView = player.transform.position - transform.position;
            dirView.y = 0;
            transform.forward = dirView; 
        }

        if(soEnemy.state == SOEnemy.State.WALKING && detected)
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
        alert = true;
        soEnemy.state = SOEnemy.State.WALKING;
        detected = true;
        navMeshAgent.SetDestination(player.position);
        soEnemy.MoveStart();
        StartCoroutine(StopDetect());
    }

    IEnumerator StopDetect()
    {
        yield return new WaitForSeconds(5);
        Recover();
    }

    /*
    void StartRandomize()
    {
        RandomizeAttack();
    }
    */

    void Alerting()
    {
        soEnemy.state = SOEnemy.State.WALKING;
        soEnemy.canAttack = true;
        detected = true;
        navMeshAgent.SetDestination(player.position);
        soEnemy.MoveStart();
    }

    void RandomizeAttack()
    {
        if(soEnemy.state == SOEnemy.State.STOPPED)
        {
            int i = Random.Range(0,soEnemy.divisorAttackChance);

            if((i == 0 || attackConfirmed >= soEnemy.maxSecondsToAttack) && attackConfirmed > 1)
            {
                Alerting();
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.INCENDIARY && !soPlayer.soPlayerHealth.burned && attackConfirmed > 1)
            {
                Alerting();
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.HUNTER && !soPlayer.soPlayerMove.slow && attackConfirmed > 1)
            {
                Alerting();
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.LUMBERJACK && attackConfirmed > 1)
            {
                Alerting();
            }
            else 
            {
                StartCoroutine(cooldownRandomize());
            }
        }
    }

    IEnumerator cooldownRandomize()
    {
        yield return new WaitForSeconds(1);
        attackConfirmed++;
        RandomizeAttack();
    }

    void Recover()
    {
        StopAllCoroutines();
        attackConfirmed = 0;
        soEnemy.state = SOEnemy.State.STOPPED;
        detected = false;
        navMeshAgent.SetDestination(transform.position);
        RandomizeAttack();
    }

    public void Restart()
    {
        alert = false;
        attackConfirmed = 0;
        detected = false;
        lastState = SOEnemy.State.STOPPED;
        transform.position = firstLocal;
        if(gameObject.activeInHierarchy) navMeshAgent.SetDestination(transform.position);
        
        
    }



    void OnEnable()
    {
        navMeshAgent.SetDestination(transform.position);
        if(firstEnable)
        {
            soEnemy.SummonEvent.AddListener(Detect);
            soSave.RestartEvent.AddListener(Restart);
            soEnemy.AttackEndEvent.AddListener(Recover);
        }
        firstEnable = true;
    }
    void OnDisable()
    {
        navMeshAgent.SetDestination(transform.position);
        soEnemy.SummonEvent.RemoveListener(Detect);
        soSave.RestartEvent.RemoveListener(Restart);
        soEnemy.AttackEndEvent.RemoveListener(Recover);
    }

}
