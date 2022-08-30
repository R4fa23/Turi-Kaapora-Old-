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
    public int attackDamage;
    public float comboTime;
    public int comboDamage;
    public float damagedCooldown;
    public float currentCooldown;
    public float currentDuration;
    public int currentDamage;
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
            currentDamage = comboDamage;
            currentCooldown = attackComboFinalCooldown;
            currentDuration = attackComboFinalDuration;
        }
        else{
            currentDamage = attackDamage;
            currentCooldown = attackCooldown;
            currentDuration = attackDuration;
        }
        AttackStartEvent.Invoke();
    }
}
