using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public SOEnemy soEnemy;
    Transform player;
    bool attacking;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        if(Vector3.Distance(transform.parent.transform.position, player.position) < soEnemy.soEnemyAttack.attackRange && !attacking)
        {  
            soEnemy.state = SOEnemy.State.ATTACKING;
            attacking = true;
            StartCharge();
        }
    }

    public void StartCharge()
    {
        soEnemy.soEnemyAttack.ChargeStart();
        StartCoroutine(ChargingTime());

    }
    public void StartAttack()
    {
        soEnemy.soEnemyAttack.AttackStart();
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
        StartCoroutine(AttackTime());
    }

    public void EndAttack()
    {
        StopAllCoroutines();
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        StartCoroutine(AttackCooldown());

    }

    IEnumerator ChargingTime()
    {
        yield return new WaitForSeconds(soEnemy.soEnemyAttack.attackChargeDuration);
        StartAttack();
    }
    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(soEnemy.soEnemyAttack.attackDuration);
        EndAttack();
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(soEnemy.soEnemyAttack.attackCooldown);
        soEnemy.state = SOEnemy.State.WALKING;
        attacking = false;
    }

}
