using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOEnemyAttack : ScriptableObject
{
    public int attackDamage;
    public float attackChargeDuration;
    public float attackDuration;
    public float attackCooldown;
    public float attackRange;

    [System.NonSerialized]
    public UnityEvent ChargeStartEvent;
    [System.NonSerialized]
    public UnityEvent AttackStartEvent;

    private void OnEnable() {
        if(AttackStartEvent == null)
            AttackStartEvent = new UnityEvent();

        if(ChargeStartEvent == null)
            ChargeStartEvent = new UnityEvent();
    }
    public void AttackStart()
    {
        AttackStartEvent.Invoke();
    }
    public void ChargeStart()
    {
        ChargeStartEvent.Invoke();
    }
}
