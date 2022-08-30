using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public SOPlayer soPlayer;
    SOEnemy soEnemy;
    Transform player;
    bool attacking;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    bool rotate;
    
    void Start()
    {
        soEnemy = transform.parent.transform.GetComponent<EnemyManager>().soEnemy;
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        if(Vector3.Distance(transform.parent.transform.position, player.position) < soEnemy.attackRange && !attacking)
        {  
            //transform.parent.transform.forward = Vector3.RotateTowards(transform.parent.transform.forward, player.position - transform.parent.transform.position, Mathf.PI / 200, 0);
            soEnemy.state = SOEnemy.State.ATTACKING;
            attacking = true;
            rotate = true;
            StartCharge();
        }
        if(rotate)
        {
            transform.parent.transform.forward = Vector3.RotateTowards(transform.parent.transform.forward, player.position - transform.parent.transform.position, Mathf.PI / 100, 0);
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
        StartCoroutine(AttackTime());
    }

    public void EndAttack()
    {
        StopAllCoroutines();
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        rotate = false;
        StartCoroutine(AttackCooldown());

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
        yield return new WaitForSeconds(soEnemy.attackCooldown);
        soEnemy.state = SOEnemy.State.WALKING;
        attacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soPlayer.soPlayerHealth.HealthChange(-soEnemy.attackDamage);
        }
    }

}
