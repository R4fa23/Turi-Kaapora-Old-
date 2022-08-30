using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOEnemy : ScriptableObject
{
    public enum State { STOPPED, WALKING, ATTACKING }
    public State state = State.STOPPED;
    public int attackDamage;
    public float attackChargeDuration;
    public float attackDuration;
    public float attackCooldown;
    public float cooldownDamaged;
    public float attackRange;
    public float vel;
    public float distanceDetectation;
    public float maxHealth;
    public float health;
    public float currentCooldown;

    [System.NonSerialized]
    public UnityEvent ChargeStartEvent;
    [System.NonSerialized]
    public UnityEvent AttackStartEvent;
    [System.NonSerialized]
    public UnityEvent MoveStartEvent;
    [System.NonSerialized]
    public UnityEvent ChangeLifeEvent;
    [System.NonSerialized]
    public UnityEvent DieEvent;

    private void OnEnable() {
        if(MoveStartEvent == null)
            MoveStartEvent = new UnityEvent();

        if(AttackStartEvent == null)
            AttackStartEvent = new UnityEvent();

        if(ChargeStartEvent == null)
            ChargeStartEvent = new UnityEvent();
        
        if(ChangeLifeEvent == null)
            ChangeLifeEvent = new UnityEvent();
        
        if(DieEvent == null)
            DieEvent = new UnityEvent();
    }
    public void MoveStart(){
        MoveStartEvent.Invoke();
    }
    public void AttackStart()
    {
        AttackStartEvent.Invoke();
    }
    public void ChargeStart()
    {
        ChargeStartEvent.Invoke();
    }
    public void ChangeLife(int amount) 
    {
        health += amount;
        ChangeLifeEvent.Invoke();
        if(health <= 0) Die();
    }
    public void Die()
    {
        DieEvent.Invoke();
    }
    /*
    public SOEnemyHealth soEnemyHealth;
    public SOEnemyAttack soEnemyAttack;
    public SOEnemyMove soEnemyMove;
    */
}
