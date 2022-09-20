using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SOEnemy reference;
    [HideInInspector]
    public SOEnemy soEnemy;
    public SOSave soSave;
    public SOPlayer soPlayer;
    GameObject player;

    void Awake()
    {
        soEnemy = (SOEnemy)ScriptableObject.CreateInstance(typeof(SOEnemy));
        SetConfiguration();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Repulse()
    {
        transform.forward = player.transform.position - transform.position;
        StartCoroutine(Repulsion());
    }

    IEnumerator Repulsion()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(-transform.forward * 50, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = true;
    }

    void RecoverDamage()
    {
        StartCoroutine(TimeRecoverDamage());
    }

    IEnumerator TimeRecoverDamage()
    {
        yield return new WaitForSeconds(0.1f);
        soEnemy.canDamaged = true;
    }

    public void OnEnable()
    {
        soEnemy.canDamaged = true;
        soEnemy.health = soEnemy.maxHealth;
        soEnemy.RepulsionEvent.AddListener(RecoverDamage);
        soEnemy.DieEvent.AddListener(OnDie);
        soEnemy.RepulsionEvent.AddListener(Repulse);
    }
    public void OnDisable()
    {
        soEnemy.RepulsionEvent.RemoveListener(RecoverDamage);
        soEnemy.DieEvent.RemoveListener(OnDie);
        soEnemy.RepulsionEvent.RemoveListener(Repulse);
    }

    private void OnDie() 
    {
        soPlayer.soPlayerAttack.EnemyDie(this.gameObject);
    }

    void SetConfiguration()
    {
        soEnemy.attackDamage = reference.attackDamage;
        soEnemy.attackChargeDuration = reference.attackChargeDuration;
        soEnemy.attackDuration = reference.attackDuration;
        soEnemy.attackCooldown = reference.attackCooldown;
        soEnemy.attackRange = reference.attackRange;
        soEnemy.vel = reference.vel;
        soEnemy.distanceDetectation = reference.distanceDetectation;
        soEnemy.maxHealth = reference.maxHealth;
        soEnemy.health = reference.health;
        soEnemy.cooldownDamaged = reference.cooldownDamaged;
        soEnemy.currentCooldown = reference.currentCooldown;
        soEnemy.canDamaged = true;


        soEnemy.currentCooldown = soEnemy.attackCooldown;
        soEnemy.health = soEnemy.maxHealth;
    }
}
