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
    public float attackRange;
    public float vel;
    public float distanceDetectation;

    [System.NonSerialized]
    public UnityEvent ChargeStartEvent;
    [System.NonSerialized]
    public UnityEvent AttackStartEvent;
    [System.NonSerialized]
    public UnityEvent MoveStartEvent;

    private void OnEnable() {
        if(MoveStartEvent == null)
            MoveStartEvent = new UnityEvent();

        if(AttackStartEvent == null)
            AttackStartEvent = new UnityEvent();

        if(ChargeStartEvent == null)
            ChargeStartEvent = new UnityEvent();
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
    /*
    public SOEnemyHealth soEnemyHealth;
    public SOEnemyAttack soEnemyAttack;
    public SOEnemyMove soEnemyMove;
    */
}
