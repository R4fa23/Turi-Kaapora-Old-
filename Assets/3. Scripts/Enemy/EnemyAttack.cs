using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOEnemy soEnemy;
    SOSave soSave;
    public GameObject manager;
    Transform player;
    bool attacking;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    bool rotate;
    bool firstEnable;
    
    void Start()
    {
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
        if(Vector3.Distance(manager.transform.position, player.position) < soEnemy.attackRange && !attacking)
        {  
            //transform.parent.transform.forward = Vector3.RotateTowards(transform.parent.transform.forward, player.position - transform.parent.transform.position, Mathf.PI / 200, 0);
            soEnemy.state = SOEnemy.State.ATTACKING;
            attacking = true;
            rotate = true;
            StartCharge();
        }
        if(rotate)
        {
            Vector3 dirP= new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.parent.transform.forward = Vector3.RotateTowards(transform.parent.transform.forward, dirP - transform.parent.transform.position, Mathf.PI / 100, 0);
        }
    }

    public void StartCharge()
    {
        soEnemy.ChargeStart();
        StartCoroutine(ChargingTime());

    }
    public void StartAttack()
    {
        rotate = false;
        soEnemy.AttackStart();
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Inimigos/Ataque_Machete", transform.position);
        StartCoroutine(AttackTime());
    }

    public void EndAttack()
    {
        soEnemy.state = SOEnemy.State.STOPPED;
        StopAllCoroutines();
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
        rotate = false;
        attacking = false;
        manager.SetActive(false);
    }

    void Restart()
    {
        StopAllCoroutines();
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        rotate = false;
        attacking = false;
    }
    IEnumerator ChargingTime()
    {
        yield return new WaitForSeconds(soEnemy.attackChargeDuration);
        StartAttack();
    }
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(soEnemy.attackDuration);
        EndAttack();
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(soEnemy.currentCooldown);
        soEnemy.currentCooldown = soEnemy.attackCooldown;
        soEnemy.state = SOEnemy.State.WALKING;
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
        
    }

}
