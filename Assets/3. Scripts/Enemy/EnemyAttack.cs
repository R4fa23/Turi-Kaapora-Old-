using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public SOPlayer soPlayer;
    SOEnemy soEnemy;
    SOSave soSave;
    public GameObject manager;
    Transform player;
    bool attacking;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    bool rotate;
    bool firstEnable;
    public GameObject special;
    SphereCollider colliderSpecial;
    MeshRenderer rendererSpecial;
    bool startMoved;
    
    void Start()
    {
        if(special != null) 
        {
        colliderSpecial = special.GetComponent<SphereCollider>();
        rendererSpecial = special.GetComponent<MeshRenderer>();
        }
        soEnemy = manager.GetComponent<EnemyManager>().soEnemy;
        soSave = manager.GetComponent<EnemyManager>().soSave;
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        OnEnable();
    }

    
    void Update()
    {
        if(!soEnemy.canAttack)
        {
            soEnemy.attackTime += Time.deltaTime;

            if(soEnemy.attackTime >= soEnemy.timeToAttack) soEnemy.canAttack = true;
        }

        if(startMoved && soEnemy.enemyType == SOEnemy.EnemyType.LUMBERJACK && soEnemy.specialTime < soEnemy.timeToSpecial)
        {
            soEnemy.specialTime += Time.deltaTime;
        }

        if(Vector3.Distance(manager.transform.position, player.position) < soEnemy.attackRange && !attacking && soEnemy.canAttack)
        {  
            //transform.parent.transform.forward = Vector3.RotateTowards(transform.parent.transform.forward, player.position - transform.parent.transform.position, Mathf.PI / 200, 0);
            soEnemy.canAttack = false;
            soEnemy.attackTime = 0;
            soEnemy.state = SOEnemy.State.ATTACKING;
            attacking = true;
            rotate = true;
            StartCharge();
        }
        if(rotate)
        {
            Vector3 dirP= new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.parent.transform.forward = Vector3.RotateTowards(transform.parent.transform.forward, dirP - transform.parent.transform.position, Mathf.PI / soEnemy.rotationVel, 0);
        }
    }

    public void StartCharge()
    {
        soEnemy.ChargeStart();
        if(soEnemy.specialTime >= soEnemy.timeToSpecial) StartCoroutine(ChargingTime(soEnemy.attackChargeDuration * 2));
        else StartCoroutine(ChargingTime(soEnemy.attackChargeDuration));

    }
    public void StartAttack()
    {
        rotate = false;
        soEnemy.AttackStart();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Inimigos/Ataque_Machete", transform.position);

        if(soEnemy.specialTime >= soEnemy.timeToSpecial && special != null)
        {
            colliderSpecial.enabled = true;
            rendererSpecial.enabled = true;
            StartCoroutine(AttackTime(soEnemy.attackDuration * 2));
        }
        else
        {
            meshRenderer.enabled = true;
            boxCollider.enabled = true;
            StartCoroutine(AttackTime(soEnemy.attackDuration));
        }
        
        
        
    }

    public void Wait()
    {
        if(special != null)
        {
            colliderSpecial.enabled = false;
            rendererSpecial.enabled = false;
        }
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        StartCoroutine(WaitTime(soEnemy.attackWaitDuration));
    }
    public void EndAttack()
    {
        if(soEnemy.specialTime >= soEnemy.timeToSpecial) soEnemy.specialTime = 0;
        soEnemy.state = SOEnemy.State.STOPPED;
        StopAllCoroutines();
        if(special != null)
        {
            colliderSpecial.enabled = false;
            rendererSpecial.enabled = false;
        }
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        rotate = false;
        soEnemy.AttackEnd();
        StartCoroutine(AttackCooldown());

    }

    void Die() {
        StopAllCoroutines();
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        if(special != null)
        {
            colliderSpecial.enabled = false;
            rendererSpecial.enabled = false;
        }
        rotate = false;
        attacking = false;
        manager.SetActive(false);
    }

    void Restart()
    {
        StopAllCoroutines();
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        if(special != null)
        {
            colliderSpecial.enabled = false;
            rendererSpecial.enabled = false;
        }
        rotate = false;
        attacking = false;
        startMoved = false;
        soEnemy.specialTime = 0;
        soEnemy.attackTime = 0;
        soEnemy.canAttack = true;
    }
    IEnumerator ChargingTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        StartAttack();
    }
    IEnumerator AttackTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        Wait();
    }

    IEnumerator WaitTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        EndAttack();
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(soEnemy.currentCooldown);
        soEnemy.currentCooldown = soEnemy.attackCooldown;
        soEnemy.state = SOEnemy.State.STOPPED;
        attacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soEnemy.PlayerHited();
            soPlayer.soPlayerHealth.HealthChange(-soEnemy.attackDamage);
        }
    }

    void StartMoved()
    {
        startMoved = true;
    }

    void ChangeCooldown()
    {
        soEnemy.currentCooldown = soEnemy.cooldownDamaged;
    }
    public void OnEnable()
    {
        if(firstEnable)
        {
            //soEnemy.ChangeLifeEvent.AddListener(ChangeCooldown);
            //soEnemy.ChangeLifeEvent.AddListener(EndAttack);
            soEnemy.RepulsionEvent.AddListener(EndAttack);
            soEnemy.DieEvent.AddListener(Die);
            soSave.RestartEvent.AddListener(Restart);
            soEnemy.MoveStartEvent.AddListener(StartMoved);
        }
        firstEnable = true;
        
    }
    public void OnDisable()
    {
        //soEnemy.ChangeLifeEvent.RemoveListener(ChangeCooldown);
        //soEnemy.ChangeLifeEvent.RemoveListener(EndAttack);
        soEnemy.RepulsionEvent.RemoveListener(EndAttack);
        soEnemy.DieEvent.RemoveListener(Die);
        soSave.RestartEvent.RemoveListener(Restart);
        soEnemy.MoveStartEvent.RemoveListener(StartMoved);
        
    }

}
