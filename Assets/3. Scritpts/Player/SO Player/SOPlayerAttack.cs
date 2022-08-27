using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerAttack : ScriptableObject
{
    public float attackDuration;
    public float attackCooldown;
    public float attackComboFinalDuration;
    public float attackComboFinalCooldown;
    public float attackDamage;
    public float comboTime;
    public float comboDamage;
    public float currentCooldown;
    public float currentDuration;
    public int comboIndex = 0;



    [System.NonSerialized]
    public UnityEvent AttackStartEvent;

    private void OnEnable() {
        if(AttackStartEvent == null)
            AttackStartEvent = new UnityEvent();
    }

    public void AttackStart()
    {
        if(comboIndex == 2)
        {
            currentCooldown = attackComboFinalCooldown;
            currentDuration = attackComboFinalDuration;
        }
        else{
            currentCooldown = attackCooldown;
            currentDuration = attackDuration;
        }
        AttackStartEvent.Invoke();
    }
}
