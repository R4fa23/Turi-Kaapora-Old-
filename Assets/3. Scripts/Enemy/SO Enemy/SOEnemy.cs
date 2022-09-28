using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOEnemy", menuName = "ScriptableObjects/Characters/Enemy")]
public class SOEnemy : ScriptableObject
{
    public enum State { STOPPED, WALKING, ATTACKING }
    public enum EnemyType { MELEE, RANGE, LUMBERJACK, INCENDIARY, HUNTER }
    public State state = State.STOPPED;
    public EnemyType enemyType;
    public int attackDamage;
    [HideInInspector]
    public float attackChargeDuration;
    [HideInInspector]
    public float attackDuration;
    public float attackCooldown;
    public float cooldownDamaged;
    public float attackRange;
    public float vel;
    public float distanceDetectation;
    public float maxHealth;
    public float health;
    [HideInInspector]
    public float currentCooldown;
    [HideInInspector]
    public bool canDamaged;
    public int divisorAttackChance;
    public int maxSecondsToAttack;
    public float forceRecover;
    [HideInInspector]
    public float specialTime;
    public float timeToSpecial;
    [HideInInspector]
    public bool canAttack;
    [HideInInspector]
    public float attackTime;
    public float timeToAttack;

    [System.NonSerialized]
    public UnityEvent ChargeStartEvent;
    [System.NonSerialized]
    public UnityEvent AttackStartEvent;
    [System.NonSerialized]
    public UnityEvent AttackEndEvent;
    [System.NonSerialized]
    public UnityEvent MoveStartEvent;
    [System.NonSerialized]
    public UnityEvent ChangeLifeEvent;
    [System.NonSerialized]
    public UnityEvent DieEvent;
    [System.NonSerialized]
    public UnityEvent SummonEvent;
    [System.NonSerialized]
    public UnityEvent RepulsionEvent;
    [System.NonSerialized]
    public UnityEvent PlayerHitedEvent;
    /*
    [System.NonSerialized]
    public UnityEvent StartAimRangeEvent;
    [System.NonSerialized]
    public UnityEvent EndAimRangeEvent;
    [System.NonSerialized]
    public UnityEvent StartAimEvent;
    [System.NonSerialized]
    public UnityEvent EndAimEvent;
    */

    private void OnEnable() {
        if(MoveStartEvent == null)
            MoveStartEvent = new UnityEvent();

        if(AttackStartEvent == null)
            AttackStartEvent = new UnityEvent();
        
        if(AttackEndEvent == null)
            AttackEndEvent = new UnityEvent();

        if(ChargeStartEvent == null)
            ChargeStartEvent = new UnityEvent();
        
        if(ChangeLifeEvent == null)
            ChangeLifeEvent = new UnityEvent();
        
        if(DieEvent == null)
            DieEvent = new UnityEvent();

        if(SummonEvent == null)
            SummonEvent = new UnityEvent();
        
        if(RepulsionEvent == null)
            RepulsionEvent = new UnityEvent();
        
        if(PlayerHitedEvent == null)
            PlayerHitedEvent = new UnityEvent();
        /*
        if(StartAimRangeEvent == null)
            StartAimRangeEvent = new UnityEvent();
        
        if(EndAimRangeEvent == null)
            EndAimRangeEvent = new UnityEvent();
        
        if(StartAimEvent == null)
            StartAimEvent = new UnityEvent();
        
        if(EndAimEvent == null)
            EndAimEvent = new UnityEvent();
        */
    }
    public void MoveStart(){
        MoveStartEvent.Invoke();
    }
    public void AttackStart()
    {
        AttackStartEvent.Invoke();
    }
    public void AttackEnd()
    {
        AttackEndEvent.Invoke();
    }
    public void ChargeStart()
    {
        ChargeStartEvent.Invoke();
    }
    public void ChangeLife(int amount) 
    {
        if(canDamaged)
        {
            health += amount;
            ChangeLifeEvent.Invoke();
            if(health <= 0) Die();
        }
        
    }
    public void Die()
    {
        DieEvent.Invoke();
    }
    public void Summon()
    {
        SummonEvent.Invoke();
    }
    public void Repulsion () 
    {
        canDamaged = false;
        RepulsionEvent.Invoke();
    }
    public void PlayerHited()
    {
        PlayerHitedEvent.Invoke();
    }
    /*
    public void StartAimRange()
    {
        StartAimRangeEvent.Invoke();
    }
    public void EndAimRange()
    {
        EndAimRangeEvent.Invoke();
    }
    public void StartAim()
    {
        StartAimEvent.Invoke();
    }
    public void EndAim()
    {
        EndAimEvent.Invoke();
    }
    */
    /*
    public SOEnemyHealth soEnemyHealth;
    public SOEnemyAttack soEnemyAttack;
    public SOEnemyMove soEnemyMove;
    */
}
