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
    EnemyManager manager;
    

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        manager = GetComponent<EnemyManager>();
        lastState = SOEnemy.State.STOPPED;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent.SetDestination(transform.position);
        soEnemy = manager.soEnemy;
        soSave = manager.soSave;
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

        manager.animator.SetFloat("Velocity", navMeshAgent.remainingDistance);
        
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

    void TimeAfterAttacked()
    {
        if(!soEnemy.canAttack && !soEnemy.attacked)
        {
            StopAllCoroutines();
            soEnemy.attacked = true;
            StartCoroutine(AttackedWait());
        }
    }

    IEnumerator AttackedWait()
    {
        yield return new WaitForSeconds(soEnemy.timeToAttackAfterAttacked);
        Alerting();
    }

    void RandomizeAttack()
    {
        if(soEnemy.state == SOEnemy.State.STOPPED && !soEnemy.attacked)
        {
            int i = Random.Range(0,soEnemy.divisorAttackChance);

            if((i == 0 || attackConfirmed >= soEnemy.maxSecondsToAttack) && attackConfirmed > soEnemy.minTimeToRandomize)
            {
                Alerting();
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.INCENDIARY && !soPlayer.soPlayerHealth.burned && attackConfirmed > soEnemy.minTimeToRandomize)
            {
                Alerting();
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.HUNTER && !soPlayer.soPlayerMove.slow && attackConfirmed > soEnemy.minTimeToRandomize)
            {
                Alerting();
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.LUMBERJACK && attackConfirmed > soEnemy.minTimeToRandomize)
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
        soEnemy.attacked = false;
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
        soEnemy.attacked = false;
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
            soEnemy.ChangeLifeEvent.AddListener(TimeAfterAttacked);
        }
        firstEnable = true;
    }
    void OnDisable()
    {
        navMeshAgent.SetDestination(transform.position);
        soEnemy.SummonEvent.RemoveListener(Detect);
        soSave.RestartEvent.RemoveListener(Restart);
        soEnemy.AttackEndEvent.RemoveListener(Recover);
        soEnemy.ChangeLifeEvent.RemoveListener(TimeAfterAttacked);
    }

}
